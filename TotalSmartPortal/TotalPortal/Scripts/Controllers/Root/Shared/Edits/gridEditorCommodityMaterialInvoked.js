define(["gridEditorCommodityMaterial"], (function (gridEditorCommodityMaterial) {

    gridEditorCommodityMaterialSelect = function (e) {
        var gridEditorCommodityMaterialInstance = new gridEditorCommodityMaterial("kendoGridDetails");
        gridEditorCommodityMaterialInstance.handleSelect(e);
    }

    gridEditorCommodityMaterialChange = function (e) {
        var gridEditorCommodityMaterialInstance = new gridEditorCommodityMaterial("kendoGridDetails");
        gridEditorCommodityMaterialInstance.handleChange(e);
    }

    gridEditorCommodityMaterialDataBound = function (e) {
        $(".k-animation-container:has(#CommodityMaterialCode-list)").css("width", "382");
        $("#CommodityMaterialCode-list").css("width", "382");
        //$("#CommodityMaterialCode-list").css("height", $("#CommodityMaterialCode-list").height() + 1);
    }

}));