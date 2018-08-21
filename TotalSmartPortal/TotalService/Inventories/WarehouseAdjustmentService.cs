﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;
using TotalDAL.Repositories.Inventories;
using TotalBase.Enums;


namespace TotalService.Inventories
{
    public class WarehouseAdjustmentService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<WarehouseAdjustment, WarehouseAdjustmentDetail, WarehouseAdjustmentViewDetail, TDto, TPrimitiveDto, TDtoDetail>, IWarehouseAdjustmentService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IWarehouseAdjustmentDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public WarehouseAdjustmentService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository, "WarehouseAdjustmentPostSaveValidate", "WarehouseAdjustmentSaveRelative", "WarehouseAdjustmentToggleApproved", null, null, "GetWarehouseAdjustmentViewDetails")
        {
        }

        public override ICollection<WarehouseAdjustmentViewDetail> GetViewDetails(int warehouseAdjustmentID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("WarehouseAdjustmentID", warehouseAdjustmentID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(TDto dto)
        {
            dto.WarehouseAdjustmentViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(dto);
        }

        protected override void SaveRelative(WarehouseAdjustment warehouseAdjustment, SaveRelativeOption saveRelativeOption)
        {
            base.SaveRelative(warehouseAdjustment, saveRelativeOption);

            if (warehouseAdjustment.HasPositiveLine)
            {
                IGoodsReceiptAPIRepository goodsReceiptAPIRepository = new GoodsReceiptAPIRepository(this.GenericWithDetailRepository.TotalSmartPortalEntities);
                IGoodsReceiptBaseService goodsReceiptBaseService = new GoodsReceiptBaseService(new GoodsReceiptRepository(this.GenericWithDetailRepository.TotalSmartPortalEntities));

                //VERY IMPORTANT: THE BaseService.UserID IS AUTOMATICALLY SET BY CustomControllerAttribute OF CONTROLLER, ONLY WHEN BaseService IS INITIALIZED BY CONTROLLER. BUT HERE, THE this.goodsReceiptBaseService IS INITIALIZED BY VehiclesInvoiceService => SO SHOULD SET goodsReceiptBaseService.UserID = this.UserID
                goodsReceiptBaseService.UserID = this.UserID;

                if (saveRelativeOption == SaveRelativeOption.Update)
                {
                    GoodsReceiptDTO goodsReceiptDTO = new GoodsReceiptDTO();

                    goodsReceiptDTO.EntryDate = warehouseAdjustment.EntryDate;
                    goodsReceiptDTO.Warehouse = new TotalDTO.Commons.WarehouseBaseDTO() { WarehouseID = warehouseAdjustment.WarehouseReceiptID };

                    goodsReceiptDTO.WarehouseAdjustmentID = warehouseAdjustment.WarehouseAdjustmentID;

                    goodsReceiptDTO.GoodsReceiptTypeID = (int)GlobalEnums.GoodsReceiptTypeID.WarehouseAdjustments;

                    goodsReceiptDTO.StorekeeperID = warehouseAdjustment.StorekeeperID;
                    goodsReceiptDTO.PreparedPersonID = warehouseAdjustment.PreparedPersonID;
                    goodsReceiptDTO.ApproverID = warehouseAdjustment.PreparedPersonID;

                    goodsReceiptDTO.Description = warehouseAdjustment.Description;
                    goodsReceiptDTO.Remarks = warehouseAdjustment.Remarks;

                    List<PendingWarehouseAdjustmentDetail> pendingWarehouseAdjustmentDetails = goodsReceiptAPIRepository.GetPendingWarehouseAdjustmentDetails(warehouseAdjustment.LocationID, null, warehouseAdjustment.WarehouseAdjustmentID, warehouseAdjustment.WarehouseReceiptID, null, false);
                    foreach (PendingWarehouseAdjustmentDetail pendingWarehouseAdjustmentDetail in pendingWarehouseAdjustmentDetails)
                    {
                        GoodsReceiptDetailDTO goodsReceiptDetailDTO = new GoodsReceiptDetailDTO()
                        {
                            GoodsReceiptID = goodsReceiptDTO.GoodsReceiptID,

                            WarehouseAdjustmentID = pendingWarehouseAdjustmentDetail.WarehouseAdjustmentID,
                            WarehouseAdjustmentDetailID = pendingWarehouseAdjustmentDetail.WarehouseAdjustmentDetailID,
                            WarehouseAdjustmentReference = pendingWarehouseAdjustmentDetail.PrimaryReference,
                            WarehouseAdjustmentEntryDate = pendingWarehouseAdjustmentDetail.PrimaryEntryDate,

                            WarehouseAdjustmentTypeID = pendingWarehouseAdjustmentDetail.WarehouseAdjustmentTypeID,

                            BatchID = pendingWarehouseAdjustmentDetail.BatchID,
                            BatchEntryDate = pendingWarehouseAdjustmentDetail.BatchEntryDate,

                            CommodityID = pendingWarehouseAdjustmentDetail.CommodityID,
                            CommodityCode = pendingWarehouseAdjustmentDetail.CommodityCode,
                            CommodityName = pendingWarehouseAdjustmentDetail.CommodityName,

                            Quantity = (decimal)pendingWarehouseAdjustmentDetail.QuantityRemains,
                        };
                        goodsReceiptDTO.ViewDetails.Add(goodsReceiptDetailDTO);
                    }

                    goodsReceiptBaseService.Save(goodsReceiptDTO, true);
                }

                if (saveRelativeOption == SaveRelativeOption.Undo)
                {//NOTES: THIS UNDO REQUIRE: JUST SAVE ONLY ONE GoodsReceipt FOR AN WarehouseAdjustment
                    int? goodsReceiptID = goodsReceiptAPIRepository.GetGoodsReceiptIDofWarehouseAdjustment(warehouseAdjustment.WarehouseAdjustmentID);
                    if (goodsReceiptID != null)
                        goodsReceiptBaseService.Delete((int)goodsReceiptID, true);
                    else
                        throw new Exception("Lỗi không tìm thấy phiếu nhập kho cũ của phiếu điều chỉnh kho này!" + "\r\n" + "\r\n" + "Vui lòng kiểm tra lại dữ liệu trước khi tiếp tục.");
                }
            }
        }

    }




    public class OtherMaterialReceiptService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMtlRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlRct>, WarehouseAdjustmentDetailDTO>, IOtherMaterialReceiptService
    {
        public OtherMaterialReceiptService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
    public class OtherMaterialIssueService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMtlIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlIss>, WarehouseAdjustmentDetailDTO>, IOtherMaterialIssueService
    {
        public OtherMaterialIssueService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
    public class MaterialAdjustmentService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMtlAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlAdj>, WarehouseAdjustmentDetailDTO>, IMaterialAdjustmentService
    {
        public MaterialAdjustmentService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }

    public class OtherProductReceiptService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionPrdRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdRct>, WarehouseAdjustmentDetailDTO>, IOtherProductReceiptService
    {
        public OtherProductReceiptService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
    public class OtherProductIssueService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionPrdIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdIss>, WarehouseAdjustmentDetailDTO>, IOtherProductIssueService
    {
        public OtherProductIssueService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
    public class ProductAdjustmentService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionPrdAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdAdj>, WarehouseAdjustmentDetailDTO>, IProductAdjustmentService
    {
        public ProductAdjustmentService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
}
