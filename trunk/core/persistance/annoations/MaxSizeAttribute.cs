using System;

namespace munimji.core.persistance.annoations {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class MaxSizeAttribute : Attribute {
        public int Length { get; set; }

        public MaxSizeAttribute() {
            Length = 20;
        }

        public MaxSizeAttribute(int length) {
            Length = length;
        }
    }
}