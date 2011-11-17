using System.ServiceProcess;

namespace munimji.core.components.entrypoint {
    public sealed class WindowsServiceEntryPoint : EntryPointBase {
        public override void Run(string[] args) {
            var servicesToRun = new ServiceBase[]
                                    {
                                        servicesHost
                                    };
            ServiceBase.Run(servicesToRun);
        }

        public override void Stop() {
            servicesHost.Stop();
        }
    }
}