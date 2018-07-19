using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class ProductionOrderRepository : GenericWithDetailRepository<ProductionOrder, ProductionOrderDetail>, IProductionOrderRepository
    {
        public ProductionOrderRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "ProductionOrderEditable", "ProductionOrderApproved", null, "ProductionOrderVoidable")
        {
        }
    }








    public class ProductionOrderAPIRepository : GenericAPIRepository, IProductionOrderAPIRepository
    {
        public ProductionOrderAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetProductionOrderIndexes")
        {
        }
    }


}