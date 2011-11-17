using System;
using System.ComponentModel;
using System.Reflection;
using System.ServiceProcess;
using munimji.core.instrumentation.logging;

namespace munimji.core.components.hosts {
    public abstract class ServicesHostBase : ServiceBase {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components;

        protected readonly Logger logger;


        protected ServicesHostBase(Logger logger) {
            InitializeComponent();
            this.logger = logger;
        }

        /// <summary>
        ///   Required method for Designer support - do not modify 
        ///   the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            components = new Container();
            ServiceName = Assembly.GetEntryAssembly().FullName;
        }


        public static ServicesHostBase GetHost() {
            var logger = Logger.GetInstance();
            switch (ServiceResolver.GetConfiguredServiceType()) {
                case ServiceTypes.Wcf:
                    return new WcfServiceHost(logger);
                case ServiceTypes.Worker:
                    return new WorkerServiceHost(logger);
                default:
                    throw new NotSupportedException();
            }
        }

        public void RunService(string[] args) {
            OnStart(args);
        }

        protected override sealed void OnStart(string[] args) {
            OnServiceStart(args);
        }

        protected override sealed void OnStop() {
            OnServiceStop();
        }

        protected override sealed void OnShutdown() {
            OnServiceStop();
        }

        protected abstract void OnServiceStart(string[] args);
        protected abstract void OnServiceStop();
        protected abstract void OnServiceError(object sender, EventArgs e);

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
                logger.Info("Closing host");
                Stop();
            }
            base.Dispose(disposing);
        }
    }
}