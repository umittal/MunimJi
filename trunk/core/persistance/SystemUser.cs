using NHibernate.Envers.Configuration.Attributes;

namespace munimji.core.persistance {
    [Audited]
    public class SystemUser : EntityBase {
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Domain { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
    }
}