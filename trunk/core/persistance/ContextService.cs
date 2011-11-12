using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using munimji.core.lib.coding;
using munimji.core.lib.extentions;
using munimji.core.persistance.annoations;

namespace munimji.core.persistance {
    public static class ContextService {
        private static readonly ConcurrentDictionary<Type, Model> Cache = new ConcurrentDictionary<Type, Model>();
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;

        public static TEntityContext GetContext<TEntityContext>() where TEntityContext : EntityContextBase {
            var entityContextType = typeof (TEntityContext);
            return GetContext(entityContextType) as TEntityContext;
        }

        public static EntityContextBase GetContext(Type entityContextType)
        {
            Model model;
            if (!Cache.TryGetValue(entityContextType, out model))
            {
                model = DiscoverModelFor(entityContextType);
            }

            if (model != null)
            {
                var sesssion = model.CreateSession();
                return (EntityContextBase) Activator.CreateInstance(entityContextType, new Object[] { sesssion });
            }

            throw new NotImplementedException(
                String.Format("Error creating instance for '{0}'. Implementation not found.", entityContextType.FullName));
        }

        internal static Model DiscoverModelFor(Type entityContextType) {
            var attribute = (ContextDefinationAttribute)entityContextType.GetCustomAttributes(typeof (ContextDefinationAttribute), false).FirstOrDefault();
            if(attribute== null || !attribute.ConnectionStringName.IsNotEmptyOrWhiteSpace()) {
                throw new Exception(string.Format("No {0} definned for {1}.", typeof(ContextDefinationAttribute).Name, entityContextType.Name));
            }
            var properties = entityContextType.GetProperties(Flags)
                .Where(x => x.PropertyType.IsGenericType
                    && x.PropertyType.GetGenericArguments()[0].InheritsFrom<EntityBase>()        
                    && x.PropertyType == typeof(IQueryable<>).MakeGenericType(x.PropertyType.GetGenericArguments()[0]));
            var discoveredEntities = properties.Select(x => x.PropertyType.GetGenericArguments()[0]);
            var discoveredAssemblies = discoveredEntities.ToHashSet(x => x.Assembly).ToArray();

            var model = ModelBuilder.Build(attribute.ConnectionStringName, discoveredAssemblies);
            if (!Cache.TryAdd(entityContextType, model)) {
                throw new ApplicationException("Error adding to ContextService cache.");
            }
            return model;
        }
    }
}