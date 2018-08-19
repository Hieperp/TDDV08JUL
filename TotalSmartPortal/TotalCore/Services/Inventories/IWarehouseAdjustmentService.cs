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

    public interface IOtherMaterialIssueService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO>
    { }
}

