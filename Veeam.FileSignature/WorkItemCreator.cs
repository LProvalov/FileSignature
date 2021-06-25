using System;
using System.Collections.Generic;
using System.Text;

namespace Veeam.FileSignature
{
    public class WorkItemCreator : IWorkItemCreator
    {
        private IBlock _block;

        public WorkItemCreator()
        {
            _block = null;
        }
        public void SetDataBlock(IBlock block)
        {
            if (block != null)
            {
                _block = block;
            }
            else
            {
                throw new ArgumentNullException("Block");
            }
        }

        public IWorkItem CreateWorkItem()
        {
            if (_block == null)
            {
                return null;
            } 
            return new WorkItem((state) => { _block.Calculate(); }, null);
        }
    }
}
