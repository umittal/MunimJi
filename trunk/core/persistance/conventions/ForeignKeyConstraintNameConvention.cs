using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using munimji.core.lib.extentions;

namespace munimji.core.persistance.conventions {
    /// <summary>
    ///   Foreign Key Name convention
    /// </summary>
    internal sealed class ForeignKeyConstraintNameConvention : IHasManyConvention, IHasOneConvention,
                                                               IReferenceConvention,
                                                               IJoinedSubclassConvention {
        public void Apply(IOneToManyCollectionInstance instance) {
            instance.Key.ForeignKey("none");

            //instance.Key.ForeignKey(String.Format("FK_{0}_{1}_id", instance.EntityType.Name.ToTitleCase(),
            //                                      instance.Member.Name.ToCamelCase()).Replace("`", ""));
        }

        public void Apply(IOneToOneInstance instance) {
            instance.ForeignKey("none");

            //instance.ForeignKey(String.Format("FK_{0}_{1}_id", instance.EntityType.Name.ToTitleCase(),
            //                                  instance.Name.ToCamelCase()).Replace("`", ""));
        }

        public void Apply(IManyToOneInstance instance) {
            instance.ForeignKey("none");
            //instance.ForeignKey(
            //    string.Format("FK_{0}_{1}_id", instance.EntityType.Name.ToTitleCase(),
            //                  instance.Property.Name.ToCamelCase()).Replace("`", ""));
        }

        public void Apply(IJoinedSubclassInstance instance) {
            instance.Key.ForeignKey(string.Format("FK_{0}", instance.EntityType.Name.ToTitleCase()));
        }
                                                               }
}