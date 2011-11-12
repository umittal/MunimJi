using System;

namespace munimji.core.persistance.annoations {
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreAttribute : Attribute {}
}