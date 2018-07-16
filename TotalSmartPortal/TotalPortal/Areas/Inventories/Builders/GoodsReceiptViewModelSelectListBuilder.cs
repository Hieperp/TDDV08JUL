using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IGoodsReceiptViewModelSelectListBuilder : IViewModelSelectListBuilder<GoodsReceiptViewModel>
    {
    }

    public class GoodsReceiptViewModelSelectListBuilder : A01ViewModelSelectListBuilder<GoodsReceiptViewModel>, IGoodsReceiptViewModelSelectListBuilder
    {
        public GoodsReceiptViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }

}