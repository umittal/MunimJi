using System;
using munimji.core.components.entrypoint;

namespace munimji.apps.host.wcf {
    internal class Program {
        private static void Main(string[] args) {
            //var host = new ServiceHost(typeof(InvoiceService));
            //host.Faulted += new EventHandler(host_Faulted);
            //host.Open();
            //Console.ReadLine();
            //host.Close();
            var entryPoint = EntryPointBase.GetEntryPoint();
            entryPoint.Run(args);
        }

        static void host_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Error");
        }
    }
}