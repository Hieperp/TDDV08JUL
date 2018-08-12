using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels;

namespace TotalPortal.Areas.Commons.Builders
{

    public interface ICommoditySelectListBuilder : IViewModelSelectListBuilder<CommodityViewModel>
    {
    }

    public class CommoditySelectListBuilder : ICommoditySelectListBuilder
    {
        private readonly ICommodityBrandSelectListBuilder commodityBrandSelectListBuilder;
        private readonly ICommodityBrandRepository commodityBrandRepository;

        private readonly ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder;
        private readonly ICommodityCategoryRepository commodityCategoryRepository;

        private readonly ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder;
        private readonly ICommodityTypeRepository commodityTypeRepository;

        public CommoditySelectListBuilder(ICommodityBrandSelectListBuilder commodityBrandSelectListBuilder, ICommodityBrandRepository commodityBrandRepository, ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder, ICommodityCategoryRepository commodityCategoryRepository, ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder, ICommodityTypeRepository commodityTypeRepository)
        {
            this.commodityBrandSelectListBuilder = commodityBrandSelectListBuilder;
            this.commodityBrandRepository = commodityBrandRepository;

            this.commodityCategorySelectListBuilder = commodityCategorySelectListBuilder;
            this.commodityCategoryRepository = commodityCategoryRepository;

            this.commodityTypeSelectListBuilder = commodityTypeSelectListBuilder;
            this.commodityTypeRepository = commodityTypeRepository;
        }

        public virtual void BuildSelectLists(CommodityViewModel commodityViewModel)
        {
            commodityViewModel.CommodityBrandSelectList = this.commodityBrandSelectListBuilder.BuildSelectListItemsForCommodityBrands(this.commodityBrandRepository.GetAllCommodityBrands());
            commodityViewModel.CommodityCategorySelectList = this.commodityCategorySelectListBuilder.BuildSelectListItemsForCommodityCategorys(this.commodityCategoryRepository.GetAllCommodityCategories());
            commodityViewModel.CommodityTypeSelectList = this.commodityTypeSelectListBuilder.BuildSelectListItemsForCommodityTypes(this.commodityTypeRepository.GetAllCommodityTypes());
        }
    }
}