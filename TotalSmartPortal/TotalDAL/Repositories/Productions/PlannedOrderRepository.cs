using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class PlannedOrderRepository : GenericWithDetailRepository<PlannedOrder, PlannedOrderDetail>, IPlannedOrderRepository
    {
        public PlannedOrderRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "PlannedOrderEditable", "PlannedOrderApproved", null, "PlannedOrderVoidable")
        {
        }
    }








    public class PlannedOrderAPIRepository : GenericAPIRepository, IPlannedOrderAPIRepository
    {
        public PlannedOrderAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetPlannedOrderIndexes")
        {
        }
    }


}