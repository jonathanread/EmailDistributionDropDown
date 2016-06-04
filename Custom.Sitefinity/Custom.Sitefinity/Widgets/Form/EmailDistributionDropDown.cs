using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.Data.Metadata;
using Telerik.Sitefinity.Metadata.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Web.UI;
using Telerik.Sitefinity.Web.UI.ControlDesign;
using Telerik.Sitefinity.Web.UI.Fields;
using Telerik.Sitefinity.Web.UI.Fields.Enums;
using Custom.Sitefinity.Model;

[assembly: WebResource(Custom.Sitefinity.Widgets.Form.EmailDistributionDropDown.scriptReference, "application/x-javascript")]
namespace Custom.Sitefinity.Widgets.Form
{
	/// <summary>
	/// Class used to create custom control for Form Builder
	/// </summary>
	/// <remarks>
	/// If this form widget is a part of a Sitefinity module,
	/// you can register it in the site's toolbox by adding this to the module's Install/Upgrade method(s):
	/// initializer.Installer
	///    .Toolbox(CommonToolbox.FormWidgets)
	///        .LoadOrAddSection(SectionName) // "TwoColumns" is Sitefinity's default
	///            .SetTitle(SectionTitle) // When creating a new section
	///            .SetDescription(SectionDescription) // When creating a new section
	///            .LoadOrAddWidget<EmailDistributionDropDown>("EmailDistributionDropDown")
	///                .SetTitle("EmailDistributionDropDown")
	///                .SetDescription("EmailDistributionDropDown")
	///                .LocalizeUsing<ModuleResourceClass>() // Optional
	///                .SetCssClass(WidgetCssClass) // You can use a css class to add an icon (this is optional)
	///            .Done()
	///        .Done()
	///    .Done();
	/// </remarks>
	/// <see cref="http://www.sitefinity.com/documentation/gettingstarted/creating-custom-form-field-controls"/>
	[DatabaseMapping(UserFriendlyDataType.ShortText)]
	[PropertyEditorTitle("EmailDistributionDropDown Properties"), Telerik.Sitefinity.Web.UI.ControlDesign.ControlDesigner(typeof(Custom.Sitefinity.Widgets.Form.Designer.EmailDistributionDropDownDesigner))]
	public class EmailDistributionDropDown : FieldControl, IFormFieldControl
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the EmailDistributionDropDown class.
		/// </summary>
		public EmailDistributionDropDown()
		{
			this.Title = "Email Distribution Drop Down";
		}
		#endregion

		#region Public properties (will show up in dialog)
		/// <summary>
		/// Example string
		/// </summary>
		public override string Example { get; set; }

		/// <summary>
		/// Title string
		/// </summary>
		public override string Title { get; set; }

		/// <summary>
		/// Description string
		/// </summary>
		public override string Description { get; set; }

		/// <summary>
		/// Is Field Required
		/// </summary>
		public bool Required { get; set; }

		/// <summary>
		/// Message to display if field is required
		/// </summary>
		public string RequiredMessage { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string DistributionList { get; set; }

		#endregion

		#region IFormFieldControl members
		/// <summary>
		/// Gets or sets MetaField property to persist data from control to the DB when form is submitted
		/// </summary>
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public IMetaField MetaField
		{
			get
			{
				if (this.metaField == null)
				{
					this.metaField = this.LoadDefaultMetaField();
				}

				return this.metaField;
			}
			set
			{
				this.metaField = value;
			}
		}
		#endregion

		#region Value method
		/// <summary>
		/// Get and set the value of the field. 
		/// </summary>
		public override object Value
		{
			get
			{
				string val = string.Empty;
				if (this.DropDownList.SelectedItem != null)
				{
					val = this.DropDownList.SelectedItem.Text;
				}
				return val;
			}
		}
		#endregion

		#region Template
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
					return EmailDistributionDropDown.layoutTemplatePath;
				return base.LayoutTemplatePath;
			}
			set
			{
				base.LayoutTemplatePath = value;
			}
		}
		#endregion

		#region Labels on control template
		/// <summary>
		/// Gets reference to the TitleLabel
		/// </summary>
		protected internal virtual Label TitleLabel
		{
			get
			{
				return this.Container.GetControl<Label>("titleLabel", true);
			}
		}

		/// <summary>
		/// Gets reference to the DescriptionLabel
		/// </summary>
		protected internal virtual Label DescriptionLabel
		{
			get
			{
				return Container.GetControl<Label>("descriptionLabel", true);
			}
		}

		/// <summary>
		/// Gets reference to the ExampleLabel
		/// </summary>
		protected internal virtual Label ExampleLabel
		{
			get
			{
				return this.Container.GetControl<Label>("exampleLabel", this.DisplayMode == FieldDisplayMode.Write);
			}
		}

		/// <summary>
		/// Reference to the TitleControl
		/// </summary>
		protected override WebControl TitleControl
		{
			get
			{
				return this.TitleLabel;
			}
		}

		/// <summary>
		/// Reference to the DescriptionControl
		/// </summary>
		protected override WebControl DescriptionControl
		{
			get
			{
				return this.DescriptionLabel;
			}
		}

		/// <summary>
		/// Gets the reference to the control that represents the example of the field control.
		/// Return null if no such control exists in the template.
		/// </summary>
		/// <value></value>
		protected override WebControl ExampleControl
		{
			get
			{
				return this.ExampleLabel;
			}
		}
		#endregion

		#region DropDownList on control
		/// <summary>
		/// Gets reference to the DropDownList control
		/// </summary>
		protected virtual DropDownList DropDownList
		{
			get
			{
				return this.Container.GetControl<DropDownList>("DropDownList", true);
			}
		}

		#endregion

		#region InitializeControls method
		/// <summary>
		/// Initializes the controls.
		/// </summary>
		/// <remarks>
		/// Initialize your controls in this method. Do not override CreateChildControls method.
		/// </remarks>
		protected override void InitializeControls(GenericContainer container)
		{
			// Set the label values
			this.ExampleLabel.Text = this.Example;
			this.TitleLabel.Text = this.Title;
			this.DescriptionLabel.Text = this.Description;

			if (DistributionList != null)
			{
				JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

				var items = jsonSerializer.Deserialize<List<DistributionObject>>(DistributionList);

				foreach (var item in items.OrderBy(i => i.Ordinal))
				{
					DropDownList.Items.Add(new ListItem() { Text = item.Text, Value = item.Email, Selected = item.Selected });
				}
				if (Required)
				{
					DropDownList.Attributes.Add("required", "required");
					DropDownList.Attributes.Add("validationMessage", RequiredMessage);
				}
			}
		}
		#endregion

		#region Script methods
		/// <summary>
		/// Get list of all scripts used by control
		/// </summary>
		/// <returns>List of all scripts used by control</returns>
		public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
		{
			var descriptor = new ScriptControlDescriptor(typeof(EmailDistributionDropDown).FullName, this.ClientID);
			descriptor.AddComponentProperty("dropDownList", this.DropDownList.ClientID);
			descriptor.AddProperty("dataFieldName", this.MetaField.FieldName); //the field name of the corresponding widget
			return new[] { descriptor };
		}

		/// <summary>
		/// Get reference to all scripts
		/// </summary>
		/// <returns>Reference to all scripts</returns>
		public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
		{
			var scripts = new List<ScriptReference>(base.GetScriptReferences())
            {
                new ScriptReference(EmailDistributionDropDown.scriptReference, typeof(EmailDistributionDropDown).Assembly.FullName),
                new ScriptReference("Telerik.Sitefinity.Web.UI.Fields.Scripts.FieldDisplayMode.js", "Telerik.Sitefinity"),
            };
			return scripts;
		}
		#endregion

		#region Private fields and constants
		private IMetaField metaField = null;
		public static readonly string layoutTemplatePath = "~/TenetSitefinity/Tenet.Sitefinity.Widgets.Form.EmailDistributionDropDown.ascx";
		public const string scriptReference = "Tenet.Sitefinity.Widgets.Form.EmailDistributionDropDown.js";
		#endregion
	}
}