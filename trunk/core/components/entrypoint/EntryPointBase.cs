using System;
using munimji.core.components.hosts;

namespace munimji.core.components.entrypoint
{
    public abstract class EntryPointBase {
        protected ServicesHostBase servicesHost;
        
        protected EntryPointBase() {
            servicesHost = ServicesHostBase.GetHost();
        }

        public static EntryPointBase GetEntryPoint() {
            if(Environment.UserInteractive) {
                return new ExeEntryPoint();
            }
            return new WindowsServiceEntryPoint();
        }

        public abstract void Run(string[] args);

        public abstract void Stop();
    }
}
