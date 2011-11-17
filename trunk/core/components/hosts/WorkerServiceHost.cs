using System;
using System.Collections.Generic;
using munimji.core.instrumentation.logging;
using munimji.core.worker;

namespace munimji.core.components.hosts {
    public class WorkerServiceHost : ServicesHostBase {
        private readonly Dictionary<string, WorkerHost> servicesMap = new Dictionary<string, WorkerHost>();

        public WorkerServiceHost(Logger logger) : base(logger) {
            logger.Info("Starting Worker Service Host");
        }

        protected override void OnServiceStart(string[] args) {
            servicesMap.Clear();
            foreach (var serviceType in ServiceResolver.Resolve()) {
                var host = new WorkerHost(serviceType);
                host.Faulted += OnServiceError;
                host.Open();
                logger.Info("Started service '{0}'", serviceType.Name);
                servicesMap.Add(serviceType.Name, host);
            }
        }

        protected override void OnServiceStop() {
            foreach (var service in servicesMap) {
                service.Value.Close();
                logger.Info("Stopped service '{0}'", service.Key);
            }
        }

        protected override void OnServiceError(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                OnServiceStop();
                foreach (var service in servicesMap) {
                    service.Value.Faulted -= OnServiceError;
                }
                servicesMap.Clear();
            }
            base.Dispose(disposing);
        }
    }
}