using System;
using System.Collections.Generic;
using munimji.core.lib.coding;
using NHibernate.Event;

namespace munimji.core.persistance.listners {
    /// <summary>
    ///   http://darrell.mozingo.net/2009/08/31/auditing-with-nhibernate-listeners/
    /// </summary>
    internal sealed class AuditListener : IPreInsertEventListener, IPreUpdateEventListener {
        private static readonly string Timestamp = ReflectUtils.Meta<EntityBase, DateTime>(x => x.Timestamp).Name;
        private static readonly string Creator = ReflectUtils.Meta<EntityBase, SystemUser>(x => x.Creator).Name;

        public bool OnPreInsert(PreInsertEvent e) {
            var entity = (EntityBase) e.Entity;
            PopulateAuditFields(e.State, e.Persister.PropertyNames, entity);
            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent e) {
            var entity = (EntityBase) e.Entity;
            PopulateAuditFields(e.State, e.Persister.PropertyNames, entity);
            return false;
        }

        public bool OnPreDelete(PreDeleteEvent e) {
            var entity = (EntityBase) e.Entity;
            PopulateAuditFields(e.DeletedState, e.Persister.PropertyNames, entity);
            return false;
        }

        private static void PopulateAuditFields(IList<object> state, string[] names, EntityBase entityBase) {
#if false
            var creator = string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name)
                      ? "Unknown"
                      : Thread.CurrentPrincipal.Identity.Name;
            idx = Array.FindIndex(names, n => n == Creator);
            state[idx] = creator;
            entityBase.Creator = state[idx].ToString();
#endif
        }
    }
}