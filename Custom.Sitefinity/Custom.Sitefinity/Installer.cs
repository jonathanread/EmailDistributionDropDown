using Custom.Sitefinity.Model;
using Custom.Sitefinity.Widgets.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Abstractions.VirtualPath.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Model.ContentLinks;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.Modules.Forms.Configuration;
using Telerik.Sitefinity.Modules.Forms.Events;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Project.Configuration;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Services.Notifications;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity;

namespace Custom.Sitefinity
{
    public static class Installer
    {
        public static void PreApplicationStart()
        {
            Bootstrapper.Initialized += Installer.OnBootstrapperInitialized;
        }

        private static void OnBootstrapperInitialized(object sender, Telerik.Sitefinity.Data.ExecutedEventArgs e)
        {
            if (e.CommandName == "Bootstrapped")
            {
                RegisterWidgetInToolbox();
                EventHub.Subscribe<IFormEntryCreatedEvent>(evt => FormsEventHandler(evt));
            }

            if (e.CommandName == "RegisterRoutes")
            {
                RegisterVirtualPath();
            }
        }

        private static void FormsEventHandler(IFormEntryCreatedEvent eventInfo)
        {
            var controls = eventInfo.Controls;

            FormsManager man = FormsManager.GetManager();
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            List<string> emails = new List<string>();
            foreach (var item in controls.Where(c => man.LoadControl(man.GetControl<ObjectData>(c.Id)) is EmailDistributionDropDown))
            {
                var data = jsonSerializer.Deserialize<List<DistributionObject>>((man.LoadControl(man.GetControl<ObjectData>(item.Id)) as EmailDistributionDropDown).DistributionList);
                var d = data.Where(i => i.Text == item.Value.ToString()).FirstOrDefault();
                foreach (var emailItem in d.Email.Split(','))
                {
                    emails.Add(emailItem);
                }
            }
            if (Config.Get<FormsConfig>().Notifications.Enabled)
            {
                List<ISubscriberRequest> subscribers = new List<ISubscriberRequest>();
                var ns = SystemManager.GetNotificationService();
                var serviceContext = new ServiceContext("ThisApplicationKey", "Forms");
                Guid subscriptionListId = eventInfo.FormSubscriptionListId;
                IEnumerable<ISubscriberRequest> currentSubscribers = ns.GetSubscribers(serviceContext, subscriptionListId, null);
                var currentSubscribersLookup = currentSubscribers.ToLookup(s => s.Email);
                IDictionary<string, string> notificationTemplateItems = GetNotificationTemplateItems(eventInfo);

                foreach (var item in emails.Where(r => !currentSubscribersLookup.Contains(r)))
                {
                    subscribers.Add(new SubscriberRequestProxy() { Email = item, ResolveKey = item });
                }

                MessageJobRequestProxy messageJobRequestProxy = new MessageJobRequestProxy()
                {
                    Subscribers = subscribers,
                    MessageTemplateId = Config.Get<FormsConfig>().Notifications.FormEntrySubmittedNotificationTemplateId,
                    SenderProfileName = Config.Get<FormsConfig>().Notifications.SenderProfile,
                    Description = string.Format("Form entry submission email notification for form '{0}'", eventInfo.FormTitle)
                };

                // Make it so, Number One.
                ns.SendMessage(serviceContext, messageJobRequestProxy, notificationTemplateItems);
            }
        }

        #region Custom Notifaction Helpers
        // Borrowed from the Telerik Sitefinity Forms module
        // It constructs the items we're going to plug into our notification template
        private static IDictionary<string, string> GetNotificationTemplateItems(IFormEntryCreatedEvent evt)
        {
            Dictionary<string, string> strs = new Dictionary<string, string>();
            string projectName = Config.Get<ProjectConfig>().ProjectName;
            strs.Add("Form.ProjectName", projectName);
            string domainUrl = UrlPath.GetDomainUrl();

            strs.Add("Form.Host", domainUrl);
            string empty = string.Empty;
            HttpContextBase currentHttpContext = SystemManager.CurrentHttpContext;
            if (currentHttpContext != null && currentHttpContext.Request != null)
            {
                empty = currentHttpContext.Request.Url.AbsoluteUri;
            }
            strs.Add("Form.Url", empty);
            strs.Add("Form.Title", evt.FormTitle);
            strs.Add("Form.IpAddress", evt.IpAddress);
            DateTime sitefinityUITime = evt.SubmissionTime.ToSitefinityUITime();
            strs.Add("Form.SubmittedOn", sitefinityUITime.ToString("dd MMMM yyyy"));
            string username = evt.Username;
            if (string.IsNullOrWhiteSpace(username))
            {
                username = Res.Get<FormsResources>("AnonymousUsername");
            }
            strs.Add("Form.Username", username);
            if (evt.Controls.Any<IFormEntryEventControl>())
            {
                strs.Add("Form.Fields", GetNotificationControlsHtml(evt.Controls));
            }
            return strs;
        }

        // Borrowed from the Telerik Sitefinity Forms module
        // This could be customized to return whatever HTML you want.
        private static string GetNotificationControlsHtml(IEnumerable<IFormEntryEventControl> controls)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table width=\"580px\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"width: 580px; margin: 0 auto; padding: 0; background-color: #fff; border-left: 1px solid #e4e4e4; border-right: 1px solid #e4e4e4; border-top: 1px solid #e4e4e4;\">");
            foreach (IFormEntryEventControl control in controls)
            {
                switch (control.Type)
                {
                    case FormEntryEventControlType.FieldControl:
                    case FormEntryEventControlType.FileFieldControl:
                        {
                            stringBuilder.AppendFormat("<tr><th width=\"190px\" bgcolor=\"#f2f2f2\" style=\"width: 190px; padding: 10px 9px; border-bottom: 1px solid #e4e4e4; border-right: 1px solid #e4e4e4; font-family: arial,verdana,sans-serif; line-height: 1.2; font-size: 11px; font-weight: normal; font-style: normal; text-align: left; background-color: #f2f2f2;\">{0}</th>", control.Title ?? "");
                            stringBuilder.Append("<td style=\"padding: 10px 9px; border-bottom: 1px solid #e4e4e4; font-family: arial,verdana,sans-serif; line-height: 1.2; font-size: 12px; font-weight: normal; font-style: normal; text-align: left;\">");
                            string contentLinksHtml = "";
                            if (control.Type != FormEntryEventControlType.FieldControl)
                            {
                                IEnumerable<ContentLink> value = control.Value as IEnumerable<ContentLink>;
                                if (value != null)
                                {
                                    contentLinksHtml = GetContentLinksHtml(value);
                                }
                            }
                            else
                            {
                                contentLinksHtml = control.Value.ToString();
                            }
                            stringBuilder.Append(contentLinksHtml);
                            stringBuilder.Append("</td></tr>");
                            continue;
                        }
                    case FormEntryEventControlType.SectionHeader:
                        {
                            stringBuilder.AppendFormat("<tr><th colspan='2' style=\"padding: 12px 9px 12px; border-bottom: 1px solid #e4e4e4; font-family: arial,verdana,sans-serif; line-height: 1.2; font-size: 15px; font-weight: normal; font-style: normal; text-align: left;\">{0}<th/></tr>", control.Title);
                            continue;
                        }
                    case FormEntryEventControlType.InstructionalText:
                        {
                            stringBuilder.AppendFormat("<tr><th colspan='2' style=\"padding: 9px; border-bottom: 1px solid #e4e4e4; font-family: arial,verdana,sans-serif; line-height: 1.2; font-size: 12px; font-weight: normal; font-style: italic; color: #666; text-align: left;\">{0}<th/></tr>", control.Title);
                            continue;
                        }
                    default:
                        {
                            continue;
                        }
                }
            }
            stringBuilder.Append("</table>");
            return stringBuilder.ToString();
        }

        // Borrowed from the Telerik Sitefinity Forms module
        private static string GetContentLinksHtml(IEnumerable<ContentLink> contentLinks)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (ContentLink contentLink in contentLinks)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(", ");
                }
                string childItemAdditionalInfo = contentLink.ChildItemAdditionalInfo;
                int num = childItemAdditionalInfo.IndexOf('|');
                if (num == -1)
                {
                    continue;
                }
                string str = childItemAdditionalInfo.Substring(0, num);
                string str1 = childItemAdditionalInfo.Substring(num + 1);
                stringBuilder.AppendFormat("<a href=\"{0}\">{1}</a>", str1, HttpUtility.HtmlEncode(str));
            }
            return stringBuilder.ToString();
        }
        #endregion


        private static void RegisterWidgetInToolbox()
        {
            var configManager = ConfigManager.GetManager();
            var config = configManager.GetSection<ToolboxesConfig>();


            var formControlsSection = config.Toolboxes["FormControls"].Sections.Where<ToolboxSection>(i => i.Name == "Common").FirstOrDefault();

            if (formControlsSection != null)
            {
                var emailDistributionWidgetName = "EmailDistributionDropDown";
                var emailDistributionWidgetType = typeof(EmailDistributionDropDown);
                if (!formControlsSection.Tools.Any<ToolboxItem>(e => e.Name == emailDistributionWidgetName))
                {
                    try
                    {
                        var tool = new ToolboxItem(formControlsSection.Tools)
                        {
                            Name = emailDistributionWidgetName,
                            Title = "Email Distribution Drop Down",
                            Description = string.Empty,
                            ControlType = emailDistributionWidgetType.AssemblyQualifiedName,
                            CssClass = "sfDropdownIcn"
                        };
                        formControlsSection.Tools.Add(tool);
                    }
                    catch (Exception e)
                    {
                        Log.Write(e.Message + e.StackTrace);
                    }
                }
            }

            configManager.Provider.SuppressSecurityChecks = true;
            configManager.SaveSection(config);
            configManager.Provider.SuppressSecurityChecks = false;
        }

        private static void RegisterVirtualPath()
        {

            var virtualPathConfig = Config.Get<VirtualPathSettingsConfig>();
            var jobsModuleVirtualPathConfig = new VirtualPathElement(virtualPathConfig.VirtualPaths)
            {
                VirtualPath = "~/CustomSitefinity/*",
                ResolverName = "EmbeddedResourceResolver",
                ResourceLocation = "Custom.Sitefinity"
            };
            virtualPathConfig.VirtualPaths.Add(jobsModuleVirtualPathConfig);
        }

    }
}

