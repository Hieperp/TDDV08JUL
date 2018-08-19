﻿using System.Web.Mvc;

using TotalCore.Services.Inventories;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Controllers
{

    public class OtherMaterialReceiptsController : WarehouseAdjustmentsController
    {
        public OtherMaterialReceiptsController(IWarehouseAdjustmentService warehouseAdjustmentService, IWarehouseAdjustmentViewModelSelectListBuilder warehouseAdjustmentViewModelSelectListBuilder)
            : base(warehouseAdjustmentService, warehouseAdjustmentViewModelSelectListBuilder)
        {
        }

        protected override WarehouseAdjustmentViewModel InitViewModel(WarehouseAdjustmentViewModel simpleViewModel)
        {
            WarehouseAdjustmentViewModel warehouseAdjustmentViewModel = base.InitViewModel(simpleViewModel);
            warehouseAdjustmentViewModel.SpecialNMVNTaskID = TotalBase.Enums.GlobalEnums.NmvnTaskID.OtherMaterialReceipt;

            return warehouseAdjustmentViewModel;
        }

    }
}