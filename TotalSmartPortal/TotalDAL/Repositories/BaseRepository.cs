using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Helpers;
using TotalCore.Repositories;


namespace TotalDAL.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public BaseRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;

            this.RepositoryBag = new Dictionary<string, object>();
        }

        private ObjectContext TotalSmartPortalObjectContext
        {
            get { return ((IObjectContextAdapter)this.totalSmartPortalEntities).ObjectContext; }
        }

        protected TotalSmartPortalEntities TotalSmartPortalEntities { get { return this.totalSmartPortalEntities; } }


        public int GetModuleID(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            var moduleDetail = this.totalSmartPortalEntities.ModuleDetails.Where(w => w.TaskID == (int)nmvnTaskID).FirstOrDefault();
            return moduleDetail != null ? moduleDetail.ModuleID : 0;
        }

        /// <summary>
        ///     Detect whether the context is dirty (i.e., there are changes in entities in memory that have
        ///     not yet been saved to the database).
        /// </summary>
        /// <param name="context">The database context to check.</param>
        /// <returns>True if dirty (unsaved changes); false otherwise.</returns>
        public bool IsDirty()
        {
            //Contract.Requires<ArgumentNullException>(context != null);

            // Query the change tracker entries for any adds, modifications, or deletes.
            IEnumerable<DbEntityEntry> res = from e in this.totalSmartPortalEntities.ChangeTracker.Entries()
                                             where e.State.HasFlag(EntityState.Added) ||
                                                 e.State.HasFlag(EntityState.Modified) ||
                                                 e.State.HasFlag(EntityState.Deleted)
                                             select e;

            var myTestOnly = res.ToList();

            if (res.Any())
                return true;

            return false;
        }

        public Dictionary<string, object> RepositoryBag { get; set; }

        public virtual ICollection<TElement> ExecuteFunction<TElement>(string functionName, params ObjectParameter[] parameters)
        {
            this.TotalSmartPortalObjectContext.CommandTimeout = 300;
            var objectResult = this.TotalSmartPortalObjectContext.ExecuteFunction<TElement>(functionName, parameters);

            return objectResult.ToList<TElement>();
        }

        public virtual int ExecuteFunction(string functionName, params ObjectParameter[] parameters)
        {
            this.TotalSmartPortalObjectContext.CommandTimeout = 300;
            return this.TotalSmartPortalObjectContext.ExecuteFunction(functionName, parameters);
        }

        public virtual int ExecuteStoreCommand(string commandText, params Object[] parameters)
        {
            this.TotalSmartPortalObjectContext.CommandTimeout = 300;
            return this.TotalSmartPortalObjectContext.ExecuteStoreCommand(commandText, parameters);
        }




        public T GetEntity<T>(bool proxyCreationEnabled, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (!proxyCreationEnabled) this.totalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;


            IQueryable<T> result = this.totalSmartPortalEntities.Set<T>();

            if (includes != null && includes.Any())
                result = includes.Aggregate(result, (current, include) => current.Include(include));


            T entity = null;

            if (predicate != null)
                entity = result.FirstOrDefault(predicate);
            else
                entity = result.FirstOrDefault();


            if (!proxyCreationEnabled) this.totalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;


            return entity;
        }


        public T GetEntity<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntity<T>(true, predicate, includes);
        }

        public T GetEntity<T>(bool proxyCreationEnabled, params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntity<T>(proxyCreationEnabled, null, includes);
        }

        public T GetEntity<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntity<T>(null, includes);
        }






        public ICollection<T> GetEntities<T>(bool proxyCreationEnabled, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (!proxyCreationEnabled) this.totalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;


            IQueryable<T> result = this.totalSmartPortalEntities.Set<T>();

            if (includes != null && includes.Any())
                result = includes.Aggregate(result, (current, include) => current.Include(include));

            ICollection<T> entities = null;

            if (predicate != null)
                entities = result.Where(predicate).ToList();
            else
                entities = result.ToList();



            if (!proxyCreationEnabled) this.totalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return entities;

        }

        public ICollection<T> GetEntities<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntities<T>(true, predicate, includes);
        }

        public ICollection<T> GetEntities<T>(bool proxyCreationEnabled, params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntities<T>(proxyCreationEnabled, null, includes);
        }

        public ICollection<T> GetEntities<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntities<T>(null, includes);
        }


        public String GetSystemInfos()
        {
            return this.GetSystemInfos(false);
        }

        public String GetSystemInfos(bool secureEncoding)
        {
            return SystemInfos.GetSystemInfos(secureEncoding);
        }

        public bool SystemInfoValidate()
        {
            return SystemInfos.Validate();
        }

    }
}
