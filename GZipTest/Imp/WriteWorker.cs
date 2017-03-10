using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using TestTask.Interfaces;

namespace TestTask.Imp
{
    /// <summary>
    /// Writing worker : that writes to file
    /// </summary>
    public class WriteWorker : BaseWorker
    {
        private bool _stop;

        private readonly string _fileName;
        private IFiFo _queue;

        public WriteWorker(string fileName, IFiFo queue, int sleepTime = 50)
        {

            _fileName = fileName;
            _queue = queue;
            //SleepTime = sleepTime;

        }

        public override void Run()
        {
       
                using (var outStream = new FileStream(_fileName, FileMode.Create))
                {
                    while (!_stop)
                    {

                        FiFo.Chunk chunk;

                        if (!_queue.Get(out chunk))
                        {
                            WriteHandle.WaitOne();
                           //  Thread.Sleep(SleepTime);
                               continue;
                        }

                        if (chunk.CheckPoint == -1)
                        {
                            Stop();
                            break;
                        }
                        outStream.Write(chunk.Data, 0, chunk.Data.Length);

                        if (Debugger.IsAttached)
                        {
                            Console.WriteLine("WRT: " + chunk.CheckPoint);
                        }

                    }
                }
            

        }

        public override void Stop()
        {
            _stop = true;
        }

        public override void Check(FiFo.Chunk chunk)
        {
            throw new NotImplementedException();
        }
    }
}
