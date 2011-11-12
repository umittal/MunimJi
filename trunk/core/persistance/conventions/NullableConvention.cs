using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using munimji.core.persistance.annoations;

namespace munimji.core.persistance.conventions {
    internal sealed class NullableConvention : AttributePropertyConvention<NullableAttribute> {
        protected override void Apply(NullableAttribute attribute, IPropertyInstance instance) {
            instance.Nullable();
        }
    }
}