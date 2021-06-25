using System;
using System.Threading;

namespace Veeam.FileSignature
{
    internal class WorkItem : IWorkItem
    {
        private WaitCallback _callback;
        private Object _state;
        internal WorkItem(WaitCallback waitCallback, Object state)
        {
            _callback = waitCallback;
            _state = state;
        }
        public void ExecuteWorkItem()
        {
            WaitCallback cb = _callback;
            _callback = null;
            cb(_state);
        }
    }
}
