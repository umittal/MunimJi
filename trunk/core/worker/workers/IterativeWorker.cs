using System.Timers;

namespace munimji.core.worker.workers {
    public abstract class IterativeWorker : WorkerBase {
        private readonly Timer timer;

        protected IterativeWorker(int interval) {
            timer = new Timer(interval);
            timer.Elapsed += TimerElapsed;
        }

        public override sealed void Start() {
            timer.Start();
        }

        public override sealed void Stop() {
            timer.Stop();
            base.Stop();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e) {
            base.Start();
        }
    }
}