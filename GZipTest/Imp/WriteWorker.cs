using System;
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

        public WriteWorker(string fileName, IFiFo queue, int sleepTime = 10)
        {

            _fileName = fileName;
            _queue = queue;
            SleepTime = sleepTime;

        }

        public override bool Run()
        {
            try
            {
                using (var outStream = new FileStream(_fileName, FileMode.Create))
                {
                    while (!_stop)
                    {

                        FiFo.Chunk chunk;

                        if (!_queue.Get(out chunk))
                        {
                            Thread.Sleep(SleepTime);
                            continue;
                        }

                        if (chunk.CheckPoint == -1)
                        {
                            Stop();
                            break;
                        }

                        outStream.Write(chunk.Data, 0, chunk.Data.Length);

                    }
                }
            }
            catch (Exception e)
            {
                e.DumpException(_fileName);
            }
            return true;
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
