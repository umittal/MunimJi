using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using munimji.core.lib.extentions;

namespace munimji.core.persistance.conventions {
    internal sealed class ColumnNameConvention : IPropertyConvention,
                                                 IIdConvention,
                                                 IVersionConvention,
                                                 ICompositeIdentityConvention {
        public void Apply(IPropertyInstance target) {
            target.Column(String.Format("[{0}]", target.Property.Name.ToCamelCase()));
        }

        public void Apply(IIdentityInstance instance) {
            instance.Column(String.Format("[{0}]", instance.Property.Name.ToCamelCase()));
        }

        public void Apply(IVersionInstance instance) {
            instance.Column(String.Format("[{0}]", instance.Name.ToCamelCase()));
        }

        public void Apply(ICompositeIdentityInstance instance) {
            foreach (var keyProperty in instance.KeyProperties) {
                var columninspector = (ColumnInspector) keyProperty.Columns.First();
                columninspector.Set(x => x.Name, Layer.Defaults, String.Format("[{0}]", columninspector.Name.ToCamelCase()));
            }
        }
                                                 }
}