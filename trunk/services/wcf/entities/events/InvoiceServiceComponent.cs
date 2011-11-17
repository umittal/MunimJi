using System.ServiceModel;
using wvc.core.instrumentation.logging;
using wvc.core.services;

namespace wvc.services.wcf.entities.events {
    public class InvoiceServiceComponentBase : ServiceComponentBase {
        private readonly ServiceHost host;
        public InvoiceServiceComponentBase(Logger logger) : base(logger) {
            var type = typeof(InvoiceService);
            Logger.Info("Intializing {0} service..", type.FullName);
            host = new ServiceHost(typeof(InvoiceService));
        }

        public override void Start(string[] args) {
            var type = typeof (InvoiceService);
            Logger.Info("Intializing {0} service..", type.FullName);
            host.Open();
        }

        public override void Stop() {
            host.Close();
        }
    }
}