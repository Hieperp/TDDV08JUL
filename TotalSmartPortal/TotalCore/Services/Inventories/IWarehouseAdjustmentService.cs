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

    public interface IOtherMaterialReceiptService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMTLRCT>, WarehouseAdjustmentPrimitiveDTO<WAOptionMTLRCT>, WarehouseAdjustmentDetailDTO>
    { }
    public interface IOtherMaterialIssueService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMTLISS>, WarehouseAdjustmentPrimitiveDTO<WAOptionMTLISS>, WarehouseAdjustmentDetailDTO>
    { }
    public interface IMaterialAdjustmentService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMTLISS>, WarehouseAdjustmentPrimitiveDTO<WAOptionMTLISS>, WarehouseAdjustmentDetailDTO>
    { }

    public interface IOtherProductReceiptService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMTLISS>, WarehouseAdjustmentPrimitiveDTO<WAOptionMTLISS>, WarehouseAdjustmentDetailDTO>
    { }
    public interface IOtherProductIssueService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMTLISS>, WarehouseAdjustmentPrimitiveDTO<WAOptionMTLISS>, WarehouseAdjustmentDetailDTO>
    { }
    public interface IProductAdjustmentService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMTLISS>, WarehouseAdjustmentPrimitiveDTO<WAOptionMTLISS>, WarehouseAdjustmentDetailDTO>
    { }
}

