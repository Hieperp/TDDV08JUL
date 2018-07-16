using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;

namespace TotalService.Inventories
{
    public class GoodsReceiptService : GenericWithViewDetailService<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, GoodsReceiptDTO, GoodsReceiptPrimitiveDTO, GoodsReceiptDetailDTO>, IGoodsReceiptService
    {
        public GoodsReceiptService(IGoodsReceiptRepository goodsReceiptRepository)
            : base(goodsReceiptRepository, "GoodsReceiptPostSaveValidate", "GoodsReceiptSaveRelative", "GoodsReceiptToggleApproved", null, null, "GetGoodsReceiptViewDetails")
        {
        }

        public override ICollection<GoodsReceiptViewDetail> GetViewDetails(int goodsReceiptID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("GoodsReceiptID", goodsReceiptID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(GoodsReceiptDTO goodsReceiptDTO)
        {
            goodsReceiptDTO.GoodsReceiptViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(goodsReceiptDTO);
        }
    }
}