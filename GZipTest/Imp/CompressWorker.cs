using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TestTask.Enums;
using TestTask.Interfaces;

namespace TestTask.Imp
{
    /// <summary>
    /// Compressor worker : main agent 
    /// </summary>
    public class CompressWorker : BaseWorker
    {
        /// <summary>
        /// Static fields as they are shared among all the compress workers
        /// </summary>
        private static bool _stop;
        private static int _checkPoint;

        private readonly IFiFo _inQueue, _outQueue;

        /// <summary>
        /// Compressor instance 
        /// </summary>
        private ICompressor _compressor;

       // private static EventWaitHandle HandleDone=new ManualResetEvent(false);


        public CompressWorker(IFiFo inQueue, IFiFo outQueue,
            Activity workActivity, int sleepTime = 50)
        {
            _inQueue = inQueue;
            _outQueue = outQueue;
            WorkActivity = workActivity;
            _stop = false;
            _checkPoint = 0;

        }

        public override void Run()
        {

            while (!_stop)
            {

                FiFo.Chunk chunk;

                if (!_inQueue.Get(out chunk))
                {
                    ReadHandle.WaitOne();
                    continue;
                }

                // end of queue reached
                if (chunk.CheckPoint == -1)
                {
                    // pause this worker
                 //   HandleDone.WaitOne();
                 //    _outQueue.Put(FiFo.Chunk.Empty);
                    Stop();
                    break;
                }
                _compressor = new Compressor(chunk, WorkActivity);
                var data = _compressor.Do();


                while (true)
                {
                    if (_checkPoint == chunk.CheckPoint)
                    {

                        _outQueue.Put(new FiFo.Chunk { CheckPoint = _checkPoint, Data = data });
                        Interlocked.Increment(ref _checkPoint);
                        WriteHandle.Set();
                       // HandleDone.Set();
                        if (Debugger.IsAttached)
                        {
                            Console.WriteLine("CMP: " + (_checkPoint - 1));
                        }
                        break;
                    }
                    // cw.WaitOne();
                    //  Thread.Sleep(SleepTime);
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
