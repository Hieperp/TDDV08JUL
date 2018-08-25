using System.Web.Mvc;
using System.Collections.Generic;

using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.ViewModels.Helpers;


namespace TotalPortal.Areas.Commons.Builders
{
    public interface IA0XSimpleViewModel : ISimpleViewModel
    {
        IEnumerable<SelectListItem> CommodityBrandSelectList { get; set; }
        IEnumerable<SelectListItem> CommodityCategorySelectList { get; set; }
        IEnumerable<SelectListItem> CommodityTypeSelectList { get; set; }
    }

    public interface ICommoditySelectListBuilder<TCommodityViewModel> : IViewModelSelectListBuilder<TCommodityViewModel>
        where TCommodityViewModel : IA0XSimpleViewModel
    {
    }

    public class CommoditySelectListBuilder<TCommodityViewModel> : ICommoditySelectListBuilder<TCommodityViewModel>
        where TCommodityViewModel : IA0XSimpleViewModel
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

        public virtual void BuildSelectLists(TCommodityViewModel commodityViewModel)
        {
            commodityViewModel.CommodityBrandSelectList = this.commodityBrandSelectListBuilder.BuildSelectListItemsForCommodityBrands(this.commodityBrandRepository.GetAllCommodityBrands());
            commodityViewModel.CommodityCategorySelectList = this.commodityCategorySelectListBuilder.BuildSelectListItemsForCommodityCategorys(this.commodityCategoryRepository.GetAllCommodityCategories());
            commodityViewModel.CommodityTypeSelectList = this.commodityTypeSelectListBuilder.BuildSelectListItemsForCommodityTypes(this.commodityTypeRepository.GetAllCommodityTypes());
        }
    }


    public interface IMaterialSelectListBuilder : ICommoditySelectListBuilder<MaterialViewModel> { }
    public class MaterialSelectListBuilder : CommoditySelectListBuilder<MaterialViewModel>, IMaterialSelectListBuilder
    {
        public MaterialSelectListBuilder(ICommodityBrandSelectListBuilder commodityBrandSelectListBuilder, ICommodityBrandRepository commodityBrandRepository, ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder, ICommodityCategoryRepository commodityCategoryRepository, ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder, ICommodityTypeRepository commodityTypeRepository)
            : base(commodityBrandSelectListBuilder, commodityBrandRepository, commodityCategorySelectListBuilder, commodityCategoryRepository, commodityTypeSelectListBuilder, commodityTypeRepository)
        { }
    }

    public interface IItemSelectListBuilder : ICommoditySelectListBuilder<ItemViewModel> { }
    public class ItemSelectListBuilder : CommoditySelectListBuilder<ItemViewModel>, IItemSelectListBuilder
    {
        public ItemSelectListBuilder(ICommodityBrandSelectListBuilder commodityBrandSelectListBuilder, ICommodityBrandRepository commodityBrandRepository, ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder, ICommodityCategoryRepository commodityCategoryRepository, ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder, ICommodityTypeRepository commodityTypeRepository)
            : base(commodityBrandSelectListBuilder, commodityBrandRepository, commodityCategorySelectListBuilder, commodityCategoryRepository, commodityTypeSelectListBuilder, commodityTypeRepository)
        { }
    }

    public interface IProductSelectListBuilder : ICommoditySelectListBuilder<ProductViewModel> { }
    public class ProductSelectListBuilder : CommoditySelectListBuilder<ProductViewModel>, IProductSelectListBuilder
    {
        public ProductSelectListBuilder(ICommodityBrandSelectListBuilder commodityBrandSelectListBuilder, ICommodityBrandRepository commodityBrandRepository, ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder, ICommodityCategoryRepository commodityCategoryRepository, ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder, ICommodityTypeRepository commodityTypeRepository)
            : base(commodityBrandSelectListBuilder, commodityBrandRepository, commodityCategorySelectListBuilder, commodityCategoryRepository, commodityTypeSelectListBuilder, commodityTypeRepository)
        { }
    }
}