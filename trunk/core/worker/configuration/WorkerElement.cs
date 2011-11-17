using System.Configuration;

namespace munimji.core.worker.configuration {
    public sealed class WorkerElement : ConfigurationSection {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("implementation", IsRequired = true)]
        public string Implementation {
            get { return (string) this["implementation"]; }
            set { this["implementation"] = value; }
        }
    }
}