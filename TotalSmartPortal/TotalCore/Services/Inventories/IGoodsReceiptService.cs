using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IGoodsReceiptService : IGenericWithViewDetailService<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, GoodsReceiptDTO, GoodsReceiptPrimitiveDTO, GoodsReceiptDetailDTO>
    {
    }
}


