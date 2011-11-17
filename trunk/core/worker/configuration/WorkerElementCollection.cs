using System.Configuration;

namespace munimji.core.worker.configuration {
    [ConfigurationCollection(typeof (WorkerElement))]
    public sealed class WorkerElementCollection : ConfigurationElementCollection {
        protected override ConfigurationElement CreateNewElement() {
            return new WorkerElement();
        }

        protected override object GetElementKey(ConfigurationElement element) {
            return ((WorkerElement) element).Name;
        }
    }
}