using System.Configuration;

namespace munimji.core.worker.configuration {
    /// <summary>
    ///   http://net.tutsplus.com/tutorials/asp-net/how-to-add-custom-configuration-settings-for-your-asp-net-application/
    /// </summary>
    public sealed class WorkerModelSection : ConfigurationSection {
        [ConfigurationProperty("workers", IsDefaultCollection = true)]
        public WorkerElementCollection Workers {
            get { return (WorkerElementCollection) this["workers"]; }
            set { this["workers"] = value; }
        }

        public static WorkerModelSection GetSection(Configuration configuration) {
            return configuration.GetSection("munimji.workerModel") as WorkerModelSection;
        }
    }
}