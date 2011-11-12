using System;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Automapping.Steps {
    public class IdentityStep : IAutomappingStep {
        private readonly IAutomappingConfiguration cfg;

        public IdentityStep(IAutomappingConfiguration cfg) {
            this.cfg = cfg;
        }

        public bool ShouldMap(Member member) {
            return cfg.IsId(member);
        }

        public void Map(ClassMappingBase classMap, Member member) {
            if (!(classMap is ClassMapping)) {
                return;
            }

            var identity = ((ClassMapping) classMap).Id;
            if (identity == null) {
                var idMapping = new IdMapping {ContainingEntityType = classMap.Type};
                var columnMapping = new ColumnMapping();
                columnMapping.Set(x => x.Name, Layer.Defaults, member.Name);
                idMapping.AddColumn(Layer.Defaults, columnMapping);
                idMapping.Set(x => x.Name, Layer.Defaults, member.Name);
                idMapping.Set(x => x.Type, Layer.Defaults, new TypeReference(member.PropertyType));
                idMapping.Member = member;
                idMapping.Set(x => x.Generator, Layer.Defaults, GetDefaultGenerator(member));

                SetDefaultAccess(member, idMapping);

                identity = idMapping;
            }
            else {
                if (identity is IdMapping) {
                    var idMapping = identity as IdMapping;
                    var compositeId = new CompositeIdMapping {ContainingEntityType = classMap.Type};

                    var idKeyPropertyMapping = GetKeyPropertyMapping(classMap.Type, idMapping.Name,
                                                                     idMapping.Type.GetUnderlyingSystemType());

                    var keyPropertyMapping = GetKeyPropertyMapping(classMap.Type, member.Name,
                                                                   member.PropertyType);

                    compositeId.AddKey(idKeyPropertyMapping);
                    compositeId.AddKey(keyPropertyMapping);

                    identity = compositeId;
                }
                else if (identity is CompositeIdMapping) {
                    var compositeId = identity as CompositeIdMapping;

                    var keyPropertyMapping = GetKeyPropertyMapping(classMap.Type, member.Name,
                                                                   member.PropertyType);

                    compositeId.AddKey(keyPropertyMapping);

                    identity = compositeId;
                }
                else {
                    throw new NotImplementedException(
                        string.Format("Mayank: Fluent nhibernate not exended to support type '{0}'",
                                      identity.GetType().Name));
                }
            }

            ((ClassMapping) classMap).Set(x => x.Id, Layer.Defaults, identity);
        }

        private static KeyPropertyMapping GetKeyPropertyMapping(Type entityType, string name, Type type) {
            var keyPropertyMapping = new KeyPropertyMapping {ContainingEntityType = entityType};
            keyPropertyMapping.Set(x => x.Name, Layer.Defaults, name);
            keyPropertyMapping.Set(x => x.Type, Layer.Defaults, new TypeReference(type));
            var columnMapping = new ColumnMapping();
            columnMapping.Set(x => x.Name, Layer.Defaults, name);
            keyPropertyMapping.AddColumn(columnMapping);
            return keyPropertyMapping;
        }

        private void SetDefaultAccess(Member member, IdMapping mapping) {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess != Access.Property && resolvedAccess != Access.Unset) {
                // if it's a property or unset then we'll just let NH deal with it, otherwise
                // set the access to be whatever we determined it might be
                mapping.Set(x => x.Access, Layer.Defaults, resolvedAccess.ToString());
            }

            if (member.IsProperty && !member.CanWrite) {
                mapping.Set(x => x.Access, Layer.Defaults, cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
            }
        }

        private static GeneratorMapping GetDefaultGenerator(Member property) {
            var generatorMapping = new GeneratorMapping();
            var defaultGenerator = new GeneratorBuilder(generatorMapping, property.PropertyType, Layer.Defaults);

            if (property.PropertyType == typeof (Guid)) {
                defaultGenerator.GuidComb();
            }
            else if (property.PropertyType == typeof (int) || property.PropertyType == typeof (long)) {
                defaultGenerator.Identity();
            }
            else {
                defaultGenerator.Assigned();
            }

            return generatorMapping;
        }
    }
}