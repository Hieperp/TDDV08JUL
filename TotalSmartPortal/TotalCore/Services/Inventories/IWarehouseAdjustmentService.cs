using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IWarehouseAdjustmentService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<WarehouseAdjustment, WarehouseAdjustmentDetail, WarehouseAdjustmentViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
    }

    public interface IOtherMaterialReceiptService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO>
    { }
    public interface IOtherMaterialIssueService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO>
    { }
    public interface IMaterialAdjustmentService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO>
    { }

    public interface IOtherProductReceiptService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO>
    { }
    public interface IOtherProductIssueService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO>
    { }
    public interface IProductAdjustmentService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO>
    { }
}

