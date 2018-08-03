using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalDAL.Repositories.Inventories
{
    public class WarehouseAdjustmentRepository : GenericWithDetailRepository<WarehouseAdjustment, WarehouseAdjustmentDetail>, IWarehouseAdjustmentRepository
    {
        public WarehouseAdjustmentRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "WarehouseAdjustmentEditable", "WarehouseAdjustmentApproved", null, "WarehouseAdjustmentVoidable")
        {
        }
    }








    public class WarehouseAdjustmentAPIRepository : GenericAPIRepository, IWarehouseAdjustmentAPIRepository
    {
        public WarehouseAdjustmentAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetWarehouseAdjustmentIndexes")
        {
        }
    }


}
