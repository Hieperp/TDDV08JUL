define(["superBase", "gridEditorTemplate"], (function (superBase, gridEditorTemplate) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridEditorTemplate);


    //The commodityMaterial here is AutoComplete Widget
    definedExemplar.prototype.handleSelect = function (e) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined) {
            var dataItem = e.sender.dataItem(e.item.index());

            currentDataSourceRow.set("CommodityMaterialID", dataItem.CommodityMaterialID);
            currentDataSourceRow.set("CommodityMaterialCode", dataItem.CommodityMaterialCode);
        }

        window.commodityMaterialCodeBeforeChange = dataItem.CommodityMaterialCode;
    };


    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("CommodityMaterialCode", window.commodityMaterialCodeBeforeChange);
    };




    return definedExemplar;
}));