using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Veeam.FileSignature
{
   public class WorkerThreadManager
    {
        private readonly ConcurrentQueue<IWorkItem> _workItemsQueue;
        private readonly int _workerThreadsCount;
        private readonly Thread _processThread;
        private WorkerThread[] _workerThreads;
        private bool _needToStop;
        private bool _inProgress;
        
        public WorkerThreadManager()
        {
            _workerThreadsCount = Environment.ProcessorCount;
            _workItemsQueue = new ConcurrentQueue<IWorkItem>();
            _processThread = new Thread(new ThreadStart(Process));
            _processThread.Start();
        }

        public WorkerThreadManager(int threadsCount)
        {
            if (threadsCount <= 0)
            {
                throw new ArgumentException("ThreadCount");
            }
            _workerThreadsCount = threadsCount;
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
            _inProgress = true;
            _workerThreads = new WorkerThread[_workerThreadsCount];
            for (int i = 0; i < _workerThreadsCount; i++)
            {
                _workerThreads[i] = new WorkerThread();
            }

            while (_inProgress)
            {
                if (_workItemsQueue.TryDequeue(out IWorkItem workItem))
                {
                    WorkerThread workerThread = null;
                    int workerThreadIndex = 0;
                    while (workerThread == null && !_needToStop)
                    {
                        if (workerThreadIndex == _workerThreads.Length)
                        {
                            workerThreadIndex = 0;
                        }

                        if (_workerThreads[workerThreadIndex].Status == WorkerThread.WorkerThreadStatus.Free)
                        {
                            workerThread = _workerThreads[workerThreadIndex];
                            workerThread.AddWork(workItem);
                        }
                        workerThreadIndex++;
                    }
                }
                else
                {
                    if (_needToStop)
                    {
                        StopWorking();
                    }
                }
            }
        }

        public void StopWorking()
        {
            foreach (var workerThread in _workerThreads)
            {
                workerThread.StopWorkerThread();
            }
            foreach (var workerThread in _workerThreads)
            {
                workerThread.Join(500);
            }
            _needToStop = true;
            _inProgress = false;
        }

        public void FinishAndStop()
        {
            _needToStop = true;
        }
    }
}
