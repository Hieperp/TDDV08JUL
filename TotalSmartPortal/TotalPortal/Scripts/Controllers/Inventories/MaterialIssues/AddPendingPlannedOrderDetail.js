function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(materialIssueGridDataSource, pendingProductionOrderDetailGridDataSource) {
    if (materialIssueGridDataSource != undefined && pendingProductionOrderDetailGridDataSource != undefined) {
        var pendingProductionOrderDetailGridDataItems = pendingProductionOrderDetailGridDataSource.view();
        var materialIssueJSON = materialIssueGridDataSource.data().toJSON();
        for (var i = 0; i < pendingProductionOrderDetailGridDataItems.length; i++) {
            if (pendingProductionOrderDetailGridDataItems[i].IsSelected === true)
                _setParentInput(materialIssueJSON, pendingProductionOrderDetailGridDataItems[i]);
        }

        materialIssueJSON.push(new Object()); //Add a temporary empty row

        materialIssueGridDataSource.data(materialIssueJSON);

        var rawData = materialIssueGridDataSource.data()
        materialIssueGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

        cancelButton_Click();
    }


    //http://www.telerik.com/forums/adding-multiple-rows-performance
    //By design the dataSource does not provide an opportunity for inserting multiple records via one operation. The performance is low, because each time when you add row through the addRow method, the DataSource throws its change event which forces the Grid to refresh and re-paint the content.
    //To avoid the problem you may try to modify the data of the DataSource manually.
    //var grid = $("#grid").data("kendoGrid");
    //var data = gr.dataSource.data().toJSON(); //the data of the DataSource

    ////change the data array
    ////any changes in the data array will not automatically reflect in the Grid

    //grid.dataSource.data(data); //set changed data as data of the Grid


    function _setParentInput(materialIssueJSON, productionOrderGridDataItem) {

        //var dataRow = materialIssueJSON.add({});

        var dataRow = new Object();

        dataRow.LocationID = null;
        dataRow.EntryDate = null;

        dataRow.MaterialIssueDetailID = 0;
        dataRow.MaterialIssueID = window.parent.$("#MaterialIssueID").val();

        dataRow.PlannedOrderID = productionOrderGridDataItem.PlannedOrderID;
        dataRow.PlannedOrderDetailID = productionOrderGridDataItem.PlannedOrderDetailID;
        dataRow.PlannedOrderMaterialID = productionOrderGridDataItem.PlannedOrderMaterialID;
        dataRow.PlannedOrderCode = productionOrderGridDataItem.PlannedOrderCode;
        dataRow.PlannedOrderReference = productionOrderGridDataItem.PlannedOrderReference;
        dataRow.PlannedOrderEntryDate = productionOrderGridDataItem.PlannedOrderEntryDate;

        dataRow.ProductionOrderID = productionOrderGridDataItem.ProductionOrderID;
        dataRow.ProductionOrderDetailID = productionOrderGridDataItem.ProductionOrderDetailID;

        dataRow.WorkshiftID = productionOrderGridDataItem.WorkshiftID;

        dataRow.CustomerID = productionOrderGridDataItem.CustomerID;
        dataRow.CustomerName = productionOrderGridDataItem.CustomerName;
        dataRow.CustomerCode = productionOrderGridDataItem.CustomerCode;

        dataRow.ProductionLineID = productionOrderGridDataItem.ProductionLineID;
        dataRow.ProductionLineCode = productionOrderGridDataItem.ProductionLineCode;

        dataRow.MoldID = productionOrderGridDataItem.MoldID;
        dataRow.MoldCode = productionOrderGridDataItem.MoldCode;

        dataRow.ProductID = productionOrderGridDataItem.ProductID;
        dataRow.ProductName = productionOrderGridDataItem.ProductName;
        dataRow.ProductCode = productionOrderGridDataItem.ProductCode;
        
        dataRow.GoodsReceiptID = productionOrderGridDataItem.GoodsReceiptID;
        dataRow.GoodsReceiptDetailID = productionOrderGridDataItem.GoodsReceiptDetailID;
        dataRow.GoodsReceiptCode = productionOrderGridDataItem.GoodsReceiptCode;
        dataRow.GoodsReceiptReference = productionOrderGridDataItem.GoodsReceiptReference;
        dataRow.GoodsReceiptEntryDate = productionOrderGridDataItem.GoodsReceiptEntryDate;

        dataRow.CommodityID = productionOrderGridDataItem.CommodityID;
        dataRow.CommodityName = productionOrderGridDataItem.CommodityName;
        dataRow.CommodityCode = productionOrderGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = productionOrderGridDataItem.CommodityTypeID;

        dataRow.WorkshiftPlannedOrderRemains = productionOrderGridDataItem.WorkshiftPlannedOrderRemains;
        dataRow.QuantityAvailables = productionOrderGridDataItem.QuantityAvailables;
        dataRow.QuantityRemains = productionOrderGridDataItem.QuantityRemains;
        dataRow.Quantity = productionOrderGridDataItem.Quantity;

        dataRow.Remarks = null;


        materialIssueJSON.push(dataRow);
    }
}

