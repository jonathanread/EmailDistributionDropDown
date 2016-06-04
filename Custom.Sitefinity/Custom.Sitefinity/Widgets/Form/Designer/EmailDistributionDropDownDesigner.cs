using System;
using System.Linq;
using System.Web.UI;
using Telerik.Sitefinity.Web.UI;
using Telerik.Sitefinity.Web.UI.ControlDesign;
using System.Collections.Generic;
using Telerik.Sitefinity.Forms.Model;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;
using Custom.Sitefinity.Model;

[assembly: WebResource(Custom.Sitefinity.Widgets.Form.Designer.EmailDistributionDropDownDesigner.scriptReference, "application/x-javascript")]
namespace Custom.Sitefinity.Widgets.Form.Designer
{
    /// <summary>
    /// Represents a designer for the <typeparamref name="Tenet.Sitefinity.Widgets.Form.EmailDistributionDropDown"/> widget
    /// </summary>
    public class EmailDistributionDropDownDesigner : ControlDesignerBase
    {
        #region Properties
        /// <summary>
        /// Obsolete. Use LayoutTemplatePath instead.
        /// </summary>
        protected override string LayoutTemplateName
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the layout template's relative or virtual path.
        /// </summary>
        public override string LayoutTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(base.LayoutTemplatePath))
                    return EmailDistributionDropDownDesigner.layoutTemplatePath;
                return base.LayoutTemplatePath;
            }
            set
            {
                base.LayoutTemplatePath = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        #endregion

        #region Control references
        /// <summary>
        /// Gets the control that is bound to the Title property
        /// </summary>
        protected virtual Control Title
        {
            get
            {
                return this.Container.GetControl<Control>("Title", true);
            }
        }

	   /// <summary>
	   /// Gets the control that is bound to the Required Message property
	   /// </summary>
	   protected virtual Control RequiredMessage
	   {
		   get
		   {
			   return this.Container.GetControl<Control>("RequiredMessage", true);
		   }
	   }

	   protected virtual Control Required
	   {
		   get
		   {
			   return this.Container.GetControl<Control>("Required", true);
		   }
	   }

        #endregion

        #region Methods
        protected override void InitializeControls(Telerik.Sitefinity.Web.UI.GenericContainer container)
        {
            // Place your initialization logic here
            
        }


        #endregion

        #region IScriptControl implementation
        /// <summary>
        /// Gets a collection of script descriptors that represent ECMAScript (JavaScript) client components.
        /// </summary>
        public override System.Collections.Generic.IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            var scriptDescriptors = new List<ScriptDescriptor>(base.GetScriptDescriptors());
            var descriptor = (ScriptControlDescriptor)scriptDescriptors.Last();

            descriptor.AddElementProperty("title", this.Title.ClientID);
		  descriptor.AddElementProperty("requiredMessage", this.RequiredMessage.ClientID);
		  descriptor.AddElementProperty("required", this.Required.ClientID);

            return scriptDescriptors;
        }

        /// <summary>
        /// Gets a collection of ScriptReference objects that define script resources that the control requires.
        /// </summary>
        public override System.Collections.Generic.IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            var scripts = new List<ScriptReference>(base.GetScriptReferences());
            scripts.Add(new ScriptReference(EmailDistributionDropDownDesigner.scriptReference, typeof(EmailDistributionDropDownDesigner).Assembly.FullName));
            return scripts;
        }
        #endregion

        #region Private members & constants
        public static readonly string layoutTemplatePath = "~/CustomSitefinity/Tenet.Sitefinity.Widgets.Form.Designer.EmailDistributionDropDownDesigner.ascx";
        public const string scriptReference = "Custom.Sitefinity.Widgets.Form.Designer.EmailDistributionDropDownDesigner.js";
        #endregion
    }
}
 
