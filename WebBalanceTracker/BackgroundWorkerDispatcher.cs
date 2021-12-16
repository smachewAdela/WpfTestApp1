using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebBalanceTracker
{
    public class BackgroundWorkerDispatcher
    {
        List<BackgroundWorkerBase> _workers = new List<BackgroundWorkerBase>();
        Thread workerThread;
        Dictionary<BackgroundWorkerBase, DateTime> _lastWorkDateTime = new Dictionary<BackgroundWorkerBase, DateTime>();

        const int Interval = 30* 1000; // 30 seconds

        public List<BackgroundWorkerBase> Workers
        {
            get { return _workers; }
            set { _workers = value; }
        }

        protected virtual void DoWork()
        {
            while (workerThread.IsAlive)
            {
                foreach (BackgroundWorkerBase w in Workers)
                    if (!_lastWorkDateTime.ContainsKey(w))
                        RunWork(w);
                    else
                    {
                        DateTime lastWork = _lastWorkDateTime[w];
                        TimeSpan t = DateTime.Now - lastWork;
                        if (t.TotalMilliseconds >= w.RepeatEvery)
                            RunWork(w);
                    }
                Thread.Sleep(Interval);
            }
        }

        protected virtual void BackgroundWorkerStarted(BackgroundWorkerBase w)
        {
        }

        private void RunWork(BackgroundWorkerBase w)
        {
            _lastWorkDateTime[w] = DateTime.Now;
            Thread t = new Thread(new ThreadStart(w.DoWork));
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
            BackgroundWorkerStarted(w);
        }

        public void Start()
        {
            if (workerThread != null)
                Stop();
            workerThread = new Thread(new ThreadStart(DoWork));
            workerThread.Start();
        }

        public void Stop()
        {
            workerThread.Abort();
            workerThread = null;
        }
    }

    public abstract class BackgroundWorkerBase
    {
        /// <summary>
        /// do cycle in every supplied miliseconds
        /// </summary>
        public abstract int RepeatEvery { get; }
        public abstract void DoWork();
    }
}