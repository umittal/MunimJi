using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using munimji.core.lib.extentions;

namespace munimji.core.persistance.conventions {
    internal sealed class TableNameConvention : IClassConvention, IJoinedSubclassConvention {
        private readonly Dictionary<Assembly, IDatabaseConvention> cache =
            new Dictionary<Assembly, IDatabaseConvention>();

        public void Apply(IClassInstance instance) {
            var tableName = instance.EntityType.Name.ToLowerCase(true);
            IDatabaseConvention dbConvention;
            if (!TryGetConvention(instance.EntityType, out dbConvention)) {
                dbConvention = EntityFrameworkGlobals.DefaultConvention;
            }
            tableName = ApplyConvention(dbConvention, tableName);
            instance.Table(tableName);
            instance.Schema(dbConvention.Schema);
            instance.Catalog(dbConvention.Catalog);

        }

        public void Apply(IJoinedSubclassInstance instance) {
            var tableName = instance.EntityType.Name.ToLowerCase(true);
            IDatabaseConvention dbConvention;
            if (!TryGetConvention(instance.EntityType, out dbConvention)) {
                dbConvention = EntityFrameworkGlobals.DefaultConvention;
            }
            tableName = ApplyConvention(dbConvention, tableName);
            instance.Table(tableName);
            instance.Schema(dbConvention.Schema);
            instance.Catalog(dbConvention.Catalog);
        }

        private bool TryGetConvention(Type type, out IDatabaseConvention value) {
            var assembly = type.Assembly;

            if (cache.TryGetValue(assembly, out value)) {
                return true;
            }
            var query = assembly.GetExportedTypes()
                .Where(assemblyType => assemblyType != typeof (IDatabaseConvention))
                .Where(assemblyType => typeof (IDatabaseConvention).IsAssignableFrom(assemblyType));
            if (!EnumbrableExtender.IsEmpty(query)) {
                var dbConventionImplementation = query.First();
                cache[assembly] = (IDatabaseConvention) Activator.CreateInstance(dbConventionImplementation);
            }

            return cache.TryGetValue(assembly, out value);
        }

        private static string ApplyConvention(IDatabaseConvention convention, string defaultName) {
            return string.Format("{0}{1}{2}",
                                 convention.TablePrefix.IsNotEmptyOrWhiteSpace() ? convention.TablePrefix : "",
                                 defaultName,
                                 convention.TablePostfix.IsNotEmptyOrWhiteSpace() ? convention.TablePostfix : "");
        }
    }
}