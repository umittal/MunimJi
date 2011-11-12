using System;

namespace munimji.core.persistance.annoations {
    public sealed class ContextDefinationAttribute : Attribute {
        public string ConnectionStringName { get; set; }
    }
}