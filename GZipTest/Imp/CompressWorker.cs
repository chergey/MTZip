using System;
using System.Threading;
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

        public CompressWorker(IFiFo inQueue, IFiFo outQueue,
            Activity workActivity, int sleepTime = 50)
        {
            _inQueue = inQueue;
            _outQueue = outQueue;
            WorkActivity = workActivity;
            SleepTime = sleepTime;
            _stop = false;
            _checkPoint = 0;

        }

        public override bool Run()
        {
            try
            {
                while (!_stop)
                {

                    FiFo.Chunk chunk;

                    if (!_inQueue.Get(out chunk))
                    {
                        Thread.Sleep(SleepTime);
                        continue;
                    }

                    // end of queue reached
                    if (chunk.CheckPoint == -1)
                    {
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
                            break;
                        }

                        Thread.Sleep(SleepTime);
                    }
                }
            }
            catch (Exception e)
            {
                e.DumpException();
                Stop();
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
