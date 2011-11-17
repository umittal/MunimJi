using System;

namespace munimji.core.components.entrypoint
{
    public sealed class ExeEntryPoint :  EntryPointBase
    {
        public ExeEntryPoint() {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainProcessExit;
        }

        public override void Run(string[] args)
        {
            servicesHost.RunService(args);
            Console.WriteLine("Press ENTER to end.");
            Console.ReadLine();
            Stop();
        }

        public override void Stop()
        {
            servicesHost.Stop();
        }

        private void CurrentDomainProcessExit(object sender, EventArgs e)
        {
            //logger.Info("Closing host for {0} event");
            //StopHost();
            servicesHost.Stop();
        }
    }
}
