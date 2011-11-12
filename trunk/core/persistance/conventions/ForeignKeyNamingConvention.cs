using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using munimji.core.lib.extentions;

namespace munimji.core.persistance.conventions {
    /// <summary>
    ///   For foreign key column
    /// </summary>
    internal sealed class ForeignKeyColumnNameConvention : IHasManyConvention, IJoinedSubclassConvention {
        public void Apply(IOneToManyCollectionInstance instance) {
            //foreach (var column in  instance.Key.Columns.OfType<ColumnInstance>()) {
            //    column.Set(x => x.Name, Layer.Defaults, column.Name.ToCamelCase());
            //}
            instance.Key.Column(string.Format("[{0}_id]", instance.Member.Name.ToCamelCase()));
            //instance.Key.Column(string.Format("[{0}_revision]", instance.Member.Name.ToCamelCase()));
        }

        public void Apply(IJoinedSubclassInstance instance) {
            //instance.Key.Column(string.Format("{0}_id", instance.EntityType.Name.ToCamelCase()));
            //foreach (var column in instance.Key.Columns.OfType<ColumnInstance>()) {
            //    column.Set(x => x.Name, Layer.Defaults, column.Name.ToCamelCase());
            //}
            instance.Key.Column("[id]");
            //instance.Key.Column("[revision]");
        }
    }
}