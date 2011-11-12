using System;
using munimji.core.persistance.annoations;
using NHibernate.Envers.Configuration.Attributes;

namespace munimji.core.persistance {
    [Audited]
    public abstract class EntityBase {
        [PrimaryKey]
        public virtual long Id { get; protected internal set; }

        [Nullable]
        public virtual SystemUser Creator { get; protected internal set; }

        [Version]
        public virtual DateTime Timestamp { get; protected internal set; }

        protected EntityBase() {
            Creator = null;
        }
    }
}