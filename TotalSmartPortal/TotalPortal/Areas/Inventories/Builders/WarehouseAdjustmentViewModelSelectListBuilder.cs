using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IWarehouseAdjustmentViewModelSelectListBuilder : IViewModelSelectListBuilder<WarehouseAdjustmentViewModel>
    {
    }

    public class WarehouseAdjustmentViewModelSelectListBuilder : A01ViewModelSelectListBuilder<WarehouseAdjustmentViewModel>, IWarehouseAdjustmentViewModelSelectListBuilder
    {
        private readonly IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder;
        private readonly IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository;

        public WarehouseAdjustmentViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.warehouseAdjustmentTypeSelectListBuilder = warehouseAdjustmentTypeSelectListBuilder;
            this.warehouseAdjustmentTypeRepository = warehouseAdjustmentTypeRepository;
        }

        public virtual void BuildSelectLists(WarehouseAdjustmentViewModel warehouseAdjustmentViewModel)
        {
            base.BuildSelectLists(warehouseAdjustmentViewModel);
            warehouseAdjustmentViewModel.WarehouseAdjustmentTypeSelectList = this.warehouseAdjustmentTypeSelectListBuilder.BuildSelectListItemsForWarehouseAdjustmentTypes(this.warehouseAdjustmentTypeRepository.GetAllWarehouseAdjustmentTypes());
        }
    }

}