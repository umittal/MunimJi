using System;
using System.Collections.Generic;
using System.ServiceModel;
using munimji.core.instrumentation.logging;

namespace munimji.core.components.hosts {
    public class WcfServiceHost : ServicesHostBase {
        private readonly Dictionary<string, ServiceHost> servicesMap = new Dictionary<string, ServiceHost>();

        public WcfServiceHost(Logger logger) : base(logger) {
            logger.Info("Starting Wcf Service Host");
        }

        protected override void OnServiceStart(string[] args) {
            servicesMap.Clear();
            foreach (var serviceType in ServiceResolver.Resolve()) {
                var host = new ServiceHost(serviceType);
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
            logger.Info("Error");
            OnServiceStop();
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