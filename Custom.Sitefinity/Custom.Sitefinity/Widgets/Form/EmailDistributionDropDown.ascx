<%@ Control %>
<%@ Register Assembly="Telerik.Sitefinity" TagPrefix="sf" Namespace="Telerik.Sitefinity.Web.UI" %>
<div class="sfFormDropdown">
	<asp:Label runat="server" ID="titleLabel" CssClass="sfTxtLbl" Text="title label" AssociatedControlID="DropDownList" />
	<span class="sfFieldWrp sfDropdownList ">
	    <asp:DropDownList runat="server" ID="DropDownList"></asp:DropDownList>
	    <sf:SitefinityLabel runat="server" ID="descriptionLabel" WrapperTagName="div" CssClass="sfDescription"/>
	    <sf:SitefinityLabel runat="server" ID="exampleLabel" WrapperTagName="div" CssClass="sfExample"/>
	</span>
	<asp:HiddenField runat="server" ID="IsRequired" />
	<asp:HiddenField runat="server" ID="RequiredMsg" />
</div>
<div class="status"></div>
<script>
	var validator;
	$(document).ready(function () {

		validator = $("form").kendoValidator().data("kendoValidator"),
                    status = $(".status");

		$(".sfFormSubmit").on("click", function (e) {
			validateForm(e);
		});
	});
	function validateForm(e) {
		if (validator.validate()) {
			return true;
		} else {
			e.preventDefault();
			$(".container").scrollTop($(".k-invalid-msg").offset().top + 30);
			$(window).scrollTop($(".k-invalid-msg").offset().top + 30);
			return false;
		}
	}
</script>