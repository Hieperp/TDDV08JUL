using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IWarehouseAdjustmentViewModelSelectListBuilder<TWarehouseAdjustmentViewModel> : IViewModelSelectListBuilder<TWarehouseAdjustmentViewModel>
        where TWarehouseAdjustmentViewModel : IWarehouseAdjustmentViewModel
    {
    }

    public class WarehouseAdjustmentViewModelSelectListBuilder<TWarehouseAdjustmentViewModel> : A01ViewModelSelectListBuilder<TWarehouseAdjustmentViewModel>, IWarehouseAdjustmentViewModelSelectListBuilder<TWarehouseAdjustmentViewModel>
        where TWarehouseAdjustmentViewModel : IWarehouseAdjustmentViewModel
    {
        private readonly IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder;
        private readonly IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository;

        public WarehouseAdjustmentViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.warehouseAdjustmentTypeSelectListBuilder = warehouseAdjustmentTypeSelectListBuilder;
            this.warehouseAdjustmentTypeRepository = warehouseAdjustmentTypeRepository;
        }

        public override void BuildSelectLists(TWarehouseAdjustmentViewModel warehouseAdjustmentViewModel)
        {
            base.BuildSelectLists(warehouseAdjustmentViewModel);
            warehouseAdjustmentViewModel.WarehouseAdjustmentTypeSelectList = this.warehouseAdjustmentTypeSelectListBuilder.BuildSelectListItemsForWarehouseAdjustmentTypes(this.warehouseAdjustmentTypeRepository.GetAllWarehouseAdjustmentTypes());
        }
    }

    public interface IOtherMaterialIssueViewModelSelectListBuilder : IWarehouseAdjustmentViewModelSelectListBuilder<OtherMaterialIssueViewModel>
    {
    }
    public class OtherMaterialIssueViewModelSelectListBuilder : WarehouseAdjustmentViewModelSelectListBuilder<OtherMaterialIssueViewModel>, IOtherMaterialIssueViewModelSelectListBuilder
    {
        public OtherMaterialIssueViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, warehouseAdjustmentTypeSelectListBuilder, warehouseAdjustmentTypeRepository)
        {}
    }
}