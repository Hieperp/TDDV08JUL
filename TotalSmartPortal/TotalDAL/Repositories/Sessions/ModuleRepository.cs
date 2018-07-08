using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

using TotalModel.Models;
using TotalCore.Repositories.Sessions;

namespace TotalDAL.Repositories.Sessions
{   
    public class ModuleRepository : IModuleRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public ModuleRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
            this.totalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
        }

        public IQueryable<Module> GetAllModules()
        {
            return this.totalSmartPortalEntities.Modules.Where(w => w.InActive == 0);
        }

        public Module GetModuleByID(int moduleID)
        {
            var module = this.totalSmartPortalEntities.Modules.SingleOrDefault(x => x.ModuleID == moduleID);
            return module;
        }

        //Cai nay su dung tam thoi, cho cai menu ma thoi. Cach lam nay amatuer qua!!!!
        public string GetLocationName(int userID)
        {
            var organizationalUnitUser = this.totalSmartPortalEntities.OrganizationalUnitUsers.Where(w => w.UserID == userID && !w.InActive).Include(i => i.OrganizationalUnit.Location).First();
            return organizationalUnitUser.OrganizationalUnit.Location.OfficialName;
        }
        public int GetLocationID(int userID)
        {
            var organizationalUnitUser = this.totalSmartPortalEntities.OrganizationalUnitUsers.Where(w => w.UserID == userID && !w.InActive).Include(i => i.OrganizationalUnit.Location).First();
            return organizationalUnitUser.OrganizationalUnit.Location.LocationID;
        }






        public void SaveChanges()
        {
            this.totalSmartPortalEntities.SaveChanges();
        }

        public void Add(Module module)
        {
            this.totalSmartPortalEntities.Modules.Add(module);
        }

        public void Delete(Module module)
        {
            this.totalSmartPortalEntities.Modules.Remove(module);
        }
    }
}
