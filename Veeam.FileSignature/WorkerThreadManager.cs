using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Veeam.FileSignature
{
    public interface IWorkerThreadManager
    {
        public void EnqueueWork(IWorkItem workItem);
    }

    public class WorkerThreadManager : IWorkerThreadManager
    {
        private readonly ConcurrentQueue<IWorkItem> _workItemsQueue;

        private int _workerThreadsCount;
        private WorkerThread[] _workerThreads;
        private Thread _processThread;

        public WorkerThreadManager()
        {
            _workerThreadsCount = Environment.ProcessorCount;
            _workItemsQueue = new ConcurrentQueue<IWorkItem>();
            _processThread = new Thread(new ThreadStart(Process));
            _processThread.Start();
        }

        public void EnqueueWork(IWorkItem workItem)
        {
            //TODO: we should restrict workItemsQueue, to prevent memory overflow
            if (workItem == null)
            {
                throw new ArgumentNullException("WorkItem");
            }
            _workItemsQueue.Enqueue(workItem);
        }

        private void Process()
        {
            _workerThreads = new WorkerThread[_workerThreadsCount];
            for (int i = 0; i < _workerThreadsCount; i++)
            {
                _workerThreads[i] = new WorkerThread();
            }

            while (true)
            {
                if (_workItemsQueue.TryDequeue(out IWorkItem workItem))
                {
                    WorkerThread workerThread = null;
                    int workerThreadIndex = 0;
                    while (workerThread == null)
                    {
                        if (workerThreadIndex == _workerThreads.Length)
                        {
                            workerThreadIndex = 0;
                        }

                        if (_workerThreads[workerThreadIndex].Status == WorkerThread.WorkerThreadStatus.Free)
                        {
                            _workerThreads[workerThreadIndex].AddWork(workItem);
                        }
                        workerThreadIndex++;
                    }
                }
            }
        }
    }
}
