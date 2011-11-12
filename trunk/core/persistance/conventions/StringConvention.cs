using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace munimji.core.persistance.conventions {
    internal sealed class StringConvention : IPropertyConvention, IPropertyConventionAcceptance {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria) {
            criteria.Expect(x => x.Property.PropertyType == typeof (string)).Expect(x => x.Length == 0);
        }

        public void Apply(IPropertyInstance target) {
            target.Length(20);
        }
    }
}