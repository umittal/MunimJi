using System;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using munimji.core.lib.coding;
using munimji.core.lib.extentions;
using munimji.core.persistance.annoations;

namespace munimji.core.persistance.mapping {
    internal sealed class AutomappingConfiguration : DefaultAutomappingConfiguration {

        public override bool ShouldMap(Type type) {
            return type.InheritsFrom<EntityBase>();
        }

        public override bool IsVersion(Member member) {
            return member.MemberInfo.IsDefined(typeof (VersionAttribute), true);
        }

        public override bool AbstractClassIsLayerSupertype(Type type) {
            return type == typeof (EntityBase);
        }

        public override bool IsId(Member member) {
            return member.MemberInfo.IsDefined(typeof (PrimaryKeyAttribute), true);
        }

        public override bool IsComponent(Type type) {
            return type.IsDefined(typeof (ComplexTypeAttribute), false);
        }

        public override string GetComponentColumnPrefix(Member member) {
            return string.Format("{0}_", member.Name.ToCamelCase());
        }

    }
}