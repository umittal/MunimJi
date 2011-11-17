using System;
using System.Threading;

namespace munimji.core.worker {
    public class WorkerHost {
        public event EventHandler Faulted;
        private readonly WorkerBase instance;
        private readonly Thread workerThread;

        public WorkerHost(Type workerType) {
            instance = (WorkerBase) Activator.CreateInstance(workerType);
            instance.Faulted += InstanceFaulted;
            workerThread = new Thread(instance.Start) {IsBackground = true};
        }

        private void InstanceFaulted(object sender, EventArgs e) {
            if (Faulted != null) {
                Faulted(sender, e);
            }
        }

        public void Open() {
            if (instance.State != WorkerStates.Started) {
                workerThread.Start();
            }
        }

        public void Close() {
            if (instance.State != WorkerStates.Stopped) {
                instance.Stop();
            }
        }
    }
}