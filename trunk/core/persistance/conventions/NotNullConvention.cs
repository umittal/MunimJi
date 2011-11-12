using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace munimji.core.persistance.conventions {
    internal sealed class NotNullConvention : IPropertyConvention, IPropertyConventionAcceptance {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria) {
            criteria.Expect(x => x.Nullable, Is.Not.Set);
        }

        public void Apply(IPropertyInstance instance) {
            instance.Not.Nullable();
        }
    }
}