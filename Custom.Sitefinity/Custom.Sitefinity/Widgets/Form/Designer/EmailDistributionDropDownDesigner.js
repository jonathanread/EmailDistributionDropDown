Type.registerNamespace("Custom.Sitefinity.Widgets.Form.Designer");
var viewModel, optionsCount = 0;

var DistributionObjectModel = kendo.data.Model.define({
    id: "Ordinal",
    fields: {
        Ordinal: { type: "number" },
        Text: { type: "string", validation: { required: true } },
        Email: { type: "string", validation: { required: false } },
        Selected: { type: "boolean", defaultValue: false }
    }
});

Custom.Sitefinity.Widgets.Form.Designer.EmailDistributionDropDownDesigner = function (element) {
    /* Initialize Title fields */
    this._title = null;

    this._requiredMessage = null;

    this._required = null;

    /* Calls the base constructor */
    Custom.Sitefinity.Widgets.Form.Designer.EmailDistributionDropDownDesigner.initializeBase(this, [element]);
}

Custom.Sitefinity.Widgets.Form.Designer.EmailDistributionDropDownDesigner.prototype = {
    /* --------------------------------- set up and tear down --------------------------------- */
    initialize: function () {
        /* Here you can attach to events or do other initialization */
        Custom.Sitefinity.Widgets.Form.Designer.EmailDistributionDropDownDesigner.callBaseMethod(this, 'initialize');
    },
    dispose: function () {
        /* this is the place to unbind/dispose the event handlers created in the initialize method */
        Tenet.Sitefinity.Widgets.Form.Designer.EmailDistributionDropDownDesigner.callBaseMethod(this, 'dispose');
    },

    /* --------------------------------- public methods ---------------------------------- */

    findElement: function (id) {
        var result = jQuery(this.get_element()).find("#" + id).get(0);
        return result;
    },

    /* Called when the designer window gets opened and here is place to "bind" your designer to the control properties */
    refreshUI: function () {
        var controlData = this._propertyEditor.get_control(); /* JavaScript clone of your control - all the control properties will be properties of the controlData too */

        jQuery(this.get_title()).val(controlData.Title);

        jQuery(this.get_requiredMessage()).val(controlData.RequiredMessage);

        if (controlData.Required) {
            jQuery(this.get_required()).attr("checked", "checked");
        }
        else {
            jQuery(this.get_required()).attr("checked", "")
        }

        var data = (controlData.DistributionList !== null && controlData.DistributionList.length > 0) ?
					JSON.parse(controlData.DistributionList) :
					[{ Ordinal: 1, Text: "Please Select", Email: "", Selected: true }];

        var distributionDataSource = new kendo.data.DataSource({
            data: data,
            schema: {
                model: DistributionObjectModel
            },
            sort: { field: "Ordinal", dir: "asc" }
        });

        optionsCount = data.length;

        distributionDataSource.bind("change", function (e) {
            if (e.action === "itemchange") {
                if (e.field === "Text" || e.field === "Email" || e.field === "Selected") {
                    e.items[0].dirty = true;
                }
            }
            if (e.action === "add") {
                optionsCount += 1;
                e.items[0].Ordinal = optionsCount;
                distributionDataSource.sort({ field: "Ordinal", dir: "asc" });
            }
            if (e.action === "remove") {
                optionsCount -= 1;
            }
        });

        viewModel = kendo.observable({
            options: distributionDataSource
        });

        kendo.bind($("#optionsForm"), viewModel);

        var grid = $("div[data-role='grid']").data("kendoGrid");

        $(".k-grid-toolbar").delegate(".k-grid-add", "click", function (e) {
            e.preventDefault();
            grid.addRow();
        });

        grid.table.kendoDraggable({
            filter: "tbody tr",
            group: "gridGroup",
            axis: "y",
            hint: function (e) {
                return $('<div class="k-grid k-widget"><table><tbody></tbody></table></div>').find("tbody").append(e.clone());
            }
        });

        grid.table/*.find("tbody > tr")*/.kendoDropTarget({
            group: "gridGroup",
            drop: function (e) {

                var draggedRow = e.draggable.hint.find("tr");
                e.draggable.hint.hide();
                var dropLocation = $(document.elementFromPoint(e.clientX, e.clientY)),
			    dropGridRecord = distributionDataSource.getByUid(dropLocation.parent().attr("data-uid"));
                if (dropLocation.is("th")) {
                    return;
                }
                var beginningRangePosition = distributionDataSource.indexOf(dropGridRecord);
                var itemToMove = distributionDataSource.getByUid(draggedRow.data("uid"));

                distributionDataSource.remove(itemToMove);
                distributionDataSource.insert(beginningRangePosition, itemToMove);

                //not on same item
                if (dropGridRecord.get("Text") !== itemToMove.get("Text")) {
                    //reorder the items
                    var tmp = dropGridRecord.get("Ordinal");
                    dropGridRecord.set("Ordinal", itemToMove.get("Ordinal"));
                    itemToMove.set("Ordinal", tmp);

                    distributionDataSource.sort({ field: "Ordinal", dir: "asc" });
                }
            }
        });
    },

    /* Called when the "Save" button is clicked. Here you can transfer the settings from the designer to the control */
    applyChanges: function (e) {
        var count = 0;
        for (var i = 0; i < viewModel.options.view().length; i++) {
            if (viewModel.options.view()[i].Selected !== false) { count++; }
        }
        if (count > 1) {
            alert("ERROR! Multiple emails selected. Please choose one option, then click save again.")
            e.preventDefault();
        }
        else {
            var controlData = this._propertyEditor.get_control();

            /* ApplyChanges Title */
            controlData.Title = jQuery(this.get_title()).val();

            controlData.RequiredMessage = jQuery(this.get_requiredMessage()).val();

            controlData.DistributionList = JSON.stringify(viewModel.options.data());


            controlData.Required = jQuery(this.get_required()).attr("checked") === "checked" ? true : false;
        }


    },

    /* --------------------------------- event handlers ---------------------------------- */

    /* --------------------------------- private methods --------------------------------- */

    /* --------------------------------- properties -------------------------------------- */

    /* Title properties */
    get_title: function () { return this._title; },
    set_title: function (value) { this._title = value; },

    get_requiredMessage: function () { return this._requiredMessage; },
    set_requiredMessage: function (value) { this._requiredMessage = value; },

    get_required: function () { return this._required; },
    set_required: function (value) { this._required = value; }
}

Custom.Sitefinity.Widgets.Form.Designer.EmailDistributionDropDownDesigner.registerClass('Custom.Sitefinity.Widgets.Form.Designer.EmailDistributionDropDownDesigner', Telerik.Sitefinity.Web.UI.ControlDesign.ControlDesignerBase);
