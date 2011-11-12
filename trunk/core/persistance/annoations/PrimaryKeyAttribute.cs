using System;

namespace munimji.core.persistance.annoations {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class PrimaryKeyAttribute : Attribute {}
}