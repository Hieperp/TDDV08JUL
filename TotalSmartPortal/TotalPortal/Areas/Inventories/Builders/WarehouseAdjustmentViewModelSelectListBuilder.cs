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
        public WarehouseAdjustmentViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }

}