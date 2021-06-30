using System;

namespace Veeam.FileSignature
{
    public class WorkItemCreator
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
            var block = _block;
            return new WorkItem((state) =>
            {
                block.Calculate();
                Console.WriteLine(block.ToString());
            }, null);
        }
    }
}
