using System;

namespace munimji.core.worker {
    public abstract class WorkerBase {
        public event EventHandler Faulted;

        public WorkerStates State { get; protected set; }

        public virtual void Start() {
            try {
                OnStart();
                State = WorkerStates.Started;
            }
            catch (Exception ex) {
                Error();
            }
        }

        public virtual void Stop() {
            try {
                OnStop();
                State = WorkerStates.Stopped;
            }
            catch (Exception ex) {
                Error();
            }
        }

        private void Error() {
            if (Faulted != null) {
                State = WorkerStates.Errored;
                Faulted(this, null);
            }
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
    }

    public enum WorkerStates {
        Unknown,
        Started,
        Stopped,
        Errored,
    }
}