using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using munimji.core.lib.extentions;

namespace munimji.core.persistance.conventions {
    internal sealed class ReferenceConvention : IReferenceConvention {
        public void Apply(IManyToOneInstance instance) {
            instance.Column(string.Format("[{0}_id]", instance.Property.Name.ToCamelCase()));
            //instance.Column(string.Format("[{0}_revision]", instance.Property.Name.ToCamelCase()));
        }
    }
}