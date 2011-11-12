using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using munimji.core.persistance.annoations;

namespace munimji.core.persistance.conventions {
    internal sealed class MaxSizeConvetion : AttributePropertyConvention<MaxSizeAttribute> {
        protected override void Apply(MaxSizeAttribute attribute, IPropertyInstance instance) {
            if (instance.Property.PropertyType == typeof (string)) {
                instance.Length(attribute.Length);
            }
        }
    }
}