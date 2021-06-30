using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Veeam.FileSignature
{
    public class WorkerThread
    {
        public enum WorkerThreadStatus
        {
            Free,
            Busy,
            Aborted
        }

        private readonly Thread _thread;
        private IWorkItem _workItem;
        private bool _inProgress;

        public WorkerThreadStatus Status { get; private set; }
        
        public WorkerThread()
        {
            _workItem = null;
            Status = WorkerThreadStatus.Free;
            _thread = new Thread(new ThreadStart(ThreadProc));
            _thread.Start();
        }

        public bool AddWork(IWorkItem workItem)
        {
            if (Status == WorkerThreadStatus.Free)
            {
                _workItem = workItem;
                Status = WorkerThreadStatus.Busy;
                return true;
            }
            return false;
        }

        public void StopWorkerThread()
        {
            _inProgress = false;
        }
        
        public void Join(int millisecondsTimeout)
        {
            if (!_thread.Join(millisecondsTimeout))
            {
                _thread.Abort();
            };
        }

        private void ThreadProc()
        {
            try
            {
                _inProgress = true;
                while (_inProgress)
                {
                    //dequeue some data
                    if (_workItem != null)
                    {
                        _workItem.ExecuteWorkItem();
                        _workItem = null;
                        Status = WorkerThreadStatus.Free;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                _inProgress = false;
                _workItem = null;
                Status = WorkerThreadStatus.Aborted;
            }
        }
    }
}
