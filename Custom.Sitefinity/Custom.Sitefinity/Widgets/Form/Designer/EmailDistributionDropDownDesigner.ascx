<%@ Control %>
<%@ Register Assembly="Telerik.Sitefinity" TagPrefix="sf" Namespace="Telerik.Sitefinity.Web.UI" %>
<%@ Register Assembly="Telerik.Sitefinity" TagPrefix="sitefinity" Namespace="Telerik.Sitefinity.Web.UI" %>
<%@ Register Assembly="Telerik.Sitefinity" TagPrefix="sfFields" Namespace="Telerik.Sitefinity.Web.UI.Fields" %>
<%@ Register Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Modules.Forms.Web.UI.Fields" TagPrefix="sfForms" %>

<sf:ResourceLinks ID="resourcesLinks" runat="server">
    <sf:ResourceFile Name="Styles/Ajax.css" />
</sf:ResourceLinks>
<script src="//kendo.cdn.telerik.com/2016.2.504/js/kendo.all.min.js"></script>
<link rel="stylesheet" href="//kendo.cdn.telerik.com/2016.2.504/styles/kendo.common.min.css" />
<link rel="stylesheet" href="//kendo.cdn.telerik.com/2016.2.504/styles/kendo.silver.min.css" />
<div id="designerLayoutRoot" class="sfContentViews sfSingleContentView" style="max-height: 400px; overflow: auto; width: 500px;">
    <ol>
        <li class="sfFormCtrl">
            <asp:Label runat="server" AssociatedControlID="Title" CssClass="sfTxtLbl">Title</asp:Label>
            <asp:TextBox ID="Title" runat="server" CssClass="sfTxt" />
            <div class="sfExample">The widget's title</div>
        </li>

        <li class="sfFormCtrl">
            <span>
                <asp:Label ID="RequiredLabel" runat="server" AssociatedControlID="Required" CssClass="sfTxtLbl">Required
					<asp:CheckBox runat="server" ID="Required" /></asp:Label>
            </span>
            <span>
                <asp:Label ID="Label1" runat="server" AssociatedControlID="RequiredMessage" CssClass="sfTxtLbl">Required Message</asp:Label>
                <asp:TextBox ID="RequiredMessage" runat="server" CssClass="sfTxt" />
            </span>
        </li>
        <li>
            <div id="optionsForm">
                <div class="k-toolbar k-grid-toolbar">
                    <a class="k-button k-button-icontext k-grid-add" href="#">
                        <span class="k-icon k-add"></span>
                        Add Option
                    </a>
                </div>
                <div id="optionsTable" data-role="grid"
                    data-bind="source: options"
                    data-editable="true"
                    data-columns='["Text",
						"Email",
						"Selected", 
						{"command":"destroy", "title":" "}]'>
                </div>
            </div>
        </li>
    </ol>
</div>
<style>
    .k-grid-content tr[role="row"]:hover { cursor: move; }
</style>
<script>
   
</script>
