using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IMaterialIssueViewModelSelectListBuilder : IViewModelSelectListBuilder<MaterialIssueViewModel>
    {
    }

    public class MaterialIssueViewModelSelectListBuilder : A01ViewModelSelectListBuilder<MaterialIssueViewModel>, IMaterialIssueViewModelSelectListBuilder
    {
        public MaterialIssueViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }

}