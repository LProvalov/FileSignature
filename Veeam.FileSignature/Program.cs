using System;
using System.Globalization;
using System.IO;

namespace Veeam.FileSignature
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FileInfo fileInfo;
            int blockByteSize = 0;

            if (!string.IsNullOrEmpty(args[0]) && File.Exists(args[0]))
            {
                fileInfo = new FileInfo(args[0]);
            }
            else
            {
                throw new ArgumentException("Input args are incorrect. ");
            }
            if (!int.TryParse(args[1], NumberStyles.Integer, new NumberFormatInfo(), out blockByteSize))
            {
                throw new ArgumentException("Input args are incorrect. Can't parse block byte size value");
            }

            var workerThreadManager = new WorkerThreadManager();
            var workItemCreator = new WorkItemCreator();

            using (var blockBuilder = new BlockBuilder(fileInfo, blockByteSize))
            {
                while (!blockBuilder.EoF)
                {
                    var block = blockBuilder.BuildNextBlock();
                    if (block != null)
                    {
                        workItemCreator.SetDataBlock(block);
                        var workItem = workItemCreator.CreateWorkItem();
                        workerThreadManager.EnqueueWork(workItem);
                    }
                }
            }
        }
    }
}
