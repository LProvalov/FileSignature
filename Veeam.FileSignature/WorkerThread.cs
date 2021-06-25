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
            Busy
        }

        private Thread _thread;
        private IWorkItem _workItem;

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

        private void ThreadProc()
        {
            while (true)
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
    }
}
