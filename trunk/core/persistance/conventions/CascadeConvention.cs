using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace munimji.core.persistance.conventions {
    internal sealed class CascadeConvention : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention {
        public void Apply(IManyToOneInstance instance) {
            instance.Cascade.All();
        }

        public void Apply(IOneToManyCollectionInstance instance) {
            instance.Cascade.All();
        }

        public void Apply(IManyToManyCollectionInstance instance) {
            instance.Cascade.All();
        }
    }
}