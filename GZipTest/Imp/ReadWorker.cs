using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using TestTask.Enums;
using TestTask.Interfaces;

namespace TestTask.Imp
{
    /// <summary>
    /// Reading worker : that reads file
    /// </summary>
    public class ReadWorker : BaseWorker
    {
        private bool _stop;

        private readonly int _quota;
        private readonly int _bufSize;
        private readonly string _fileName;
        private readonly IFiFo _fifo;

        protected const int HeaderSize = 20;


        public ReadWorker(int quota, int bufSize, string fileName, IFiFo fifo,
            Activity workActivity, int sleepTime = 100)
        {
            _quota = quota;
            _bufSize = bufSize;
            _fileName = fileName;
            _fifo = fifo;
            WorkActivity = workActivity;
            SleepTime = sleepTime;

        }

        public override void Run()
        {
            int currentCheckPoint = 0;
            var random = new Random();
            using (var inStream = new FileStream(_fileName, FileMode.Open))
            {
                while (!_stop)
                {

                    if (_fifo.Count >= _quota)
                    {

                        Thread.Sleep(SleepTime);
                        //trigger GC (otherwise memory would be hard to get back) 
                        
                        if (random.Next(1, 60) == 1)
                        {
                            GC.Collect();

                            GC.WaitForPendingFinalizers();
                        }
                        continue;
                    }

                    if (WorkActivity == Activity.Compress)
                    {

                        byte[] data;
                        if (inStream.Length - inStream.Position > _bufSize)
                        {
                            data = new byte[_bufSize];
                        }
                        else
                        {
                            data = new byte[inStream.Length - inStream.Position];
                        }
                        if (inStream.Read(data, 0, data.Length) > 0)
                        {
                            _fifo.Put(new FiFo.Chunk { CheckPoint = currentCheckPoint, Data = data });
                            if (Debugger.IsAttached)
                            {
                                Console.WriteLine("RD: " + currentCheckPoint);
                            }
                        }
                        else
                        {
                            _fifo.Put(FiFo.Chunk.Empty);
                            Stop();
                            break;
                        }


                    }
                    else if (WorkActivity == Activity.Decompress)
                    {

                        byte[] data = new byte[HeaderSize];
                        if (inStream.Read(data, 0, data.Length) == 0)
                        {
                            _fifo.Put(FiFo.Chunk.Empty);
                            Stop();
                            break;
                        }

                        int size = new GZipHeader(data).GetSegmentSize();
                        if (size == 0)
                        {
                            data = new byte[0];

                        }
                        Array.Resize(ref data, size);
                        inStream.Read(data, HeaderSize, size - HeaderSize);

                        _fifo.Put(new FiFo.Chunk { CheckPoint = currentCheckPoint, Data = data });

                        if (Debugger.IsAttached)
                        {
                            Console.WriteLine("RDEC: " + currentCheckPoint);
                        }

                    }
                    currentCheckPoint++;
                    ReadHandle.Set();
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
