using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using munimji.core.lib.extentions;

namespace munimji.core.persistance.conventions {
    internal class PrimaryKeyConstraintNameConvention : IClassConvention, IJoinedSubclassConvention {
        public void Apply(IClassInstance instance) {
            instance.PrimaryKey(string.Format("PK_{0}", instance.EntityType.Name.ToLowerCase(true)));
        }

        public void Apply(IJoinedSubclassInstance instance) {
            instance.PrimaryKey(string.Format("PK_{0}", instance.EntityType.Name.ToLowerCase(true)));
        }
    }
}