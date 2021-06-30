using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Veeam.FileSignature
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            WorkerThreadManager workerThreadManager = null;
            try
            {
                FileInfo fileInfo;
                if (!string.IsNullOrEmpty(args[0]) && File.Exists(args[0]))
                {
                    fileInfo = new FileInfo(args[0]);
                }
                else
                {
                    throw new ArgumentException("Input args are incorrect. File should exists.");
                }
                int blockByteSize;
                if (!int.TryParse(args[1], NumberStyles.Integer, new NumberFormatInfo(), out blockByteSize))
                {
                    throw new ArgumentException("Input args are incorrect. Can't parse block byte size value");
                }

                if (args.Length == 3 &&
                    int.TryParse(args[2], NumberStyles.Integer, new NumberFormatInfo(), out int threadCount))
                {
                    workerThreadManager = new WorkerThreadManager(threadCount);
                }
                else
                {
                    workerThreadManager = new WorkerThreadManager();
                }

                var workItemCreator = new WorkItemCreator();

                stopWatch.Start();
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
                    workerThreadManager.FinishAndStop();
                }
            }
            catch (Exception ex)
            {
                workerThreadManager?.StopWorking();
                workerThreadManager?.Join();
                Utility.OutputException(ex);
            }
            finally
            {
                workerThreadManager?.Join();
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("Working time: {0:00}:{1:00}:{2:00}.{3:00}",
                                                   ts.Hours, ts.Minutes, ts.Seconds,
                                                   ts.Milliseconds / 10);
                Console.WriteLine(elapsedTime);
            }
        }
    }
}
