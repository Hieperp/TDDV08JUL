using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IWarehouseAdjustmentService : IGenericWithViewDetailService<WarehouseAdjustment, WarehouseAdjustmentDetail, WarehouseAdjustmentViewDetail, WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO>
    {
    }
}

