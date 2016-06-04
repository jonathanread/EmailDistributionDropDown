Type.registerNamespace("Custom.Sitefinity.Widgets.Form");

Custom.Sitefinity.Widgets.Form.EmailDistributionDropDown = function (element) {
    this._dropDownList = null;
    this._dataFieldName = null;
    Custom.Sitefinity.Widgets.Form.EmailDistributionDropDown.initializeBase(this, [element]);
}

Custom.Sitefinity.Widgets.Form.EmailDistributionDropDown.prototype = {
    /* --------------------------------- set up and tear down ---------------------------- */
    /* --------------------------------- public methods ---------------------------------- */

    // Gets the value of the field control.
    get_value: function () {
        return jQuery(this._dropDownList).val();
    },

    // Sets the value of the text field control depending on DisplayMode.
    set_value: function (value) {
        //Probably need to modify this for dropdownlist
        jQuery(this._dropDownList).val(value);
    },

    /* --------------------------------- event handlers ---------------------------------- */

    /* --------------------------------- private methods --------------------------------- */

    /* --------------------------------- properties -------------------------------------- */

    get_dropDownList: function () {
        return this._dropDownList;
    },

    set_dropDownList: function (value) {
        this._dropDownList = value;
    },

    get_dataFieldName: function () {
        return this._dataFieldName;
    },

    set_dataFieldName: function (value) {
        this._dataFieldName = value;
    }
}

Custom.Sitefinity.Widgets.Form.EmailDistributionDropDown.registerClass('Custom.Sitefinity.Widgets.Form.EmailDistributionDropDown', Telerik.Sitefinity.Web.UI.Fields.FieldControl);