using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;

namespace TotalService.Inventories
{
    public class GoodsReceiptService : GoodsReceiptBaseService, IGoodsReceiptService
    {
        public GoodsReceiptService(IGoodsReceiptRepository goodsReceiptRepository)
            : base(goodsReceiptRepository)
        {
        }

        public override bool Approvable(GoodsReceiptDTO dto)
        {
            return (dto.WarehouseAdjustmentID == null) && base.Approvable(dto);
        }

        public override bool UnApprovable(GoodsReceiptDTO dto)
        {
            return (dto.WarehouseAdjustmentID == null) && base.UnApprovable(dto);
        }

        public override bool Editable(GoodsReceiptDTO dto)
        {
            return (dto.WarehouseAdjustmentID == null) && base.Editable(dto);
        }
    }

    public class GoodsReceiptBaseService : GenericWithViewDetailService<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, GoodsReceiptDTO, GoodsReceiptPrimitiveDTO, GoodsReceiptDetailDTO>, IGoodsReceiptBaseService
    {
        public GoodsReceiptBaseService(IGoodsReceiptRepository goodsReceiptRepository)
            : base(goodsReceiptRepository, "GoodsReceiptPostSaveValidate", "GoodsReceiptSaveRelative", "GoodsReceiptToggleApproved", null, null, "GetGoodsReceiptViewDetails")
        {
        }

        public new bool Save(GoodsReceiptDTO goodsReceiptDTO, bool useExistingTransaction)
        {
            goodsReceiptDTO.GoodsReceiptViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(goodsReceiptDTO, useExistingTransaction);
        }

        public new bool Delete(int id, bool useExistingTransaction)
        {
            return base.Delete(id, useExistingTransaction);
        }

        public override ICollection<GoodsReceiptViewDetail> GetViewDetails(int goodsReceiptID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("GoodsReceiptID", goodsReceiptID) };
            return this.GetViewDetails(parameters);
        }
    }

}