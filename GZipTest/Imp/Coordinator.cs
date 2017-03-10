using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TestTask.Enums;
using TestTask.Interfaces;

namespace TestTask.Imp
{
    /// <summary>
    /// Main class responsible for coordination between workers
    /// </summary>
    public class Coordinator : ICoordinator
    {
        #region Properties

        public int BufSize => 1024 * 1024;

        public int MaxItems =>
            (int)(new PerformanceCounter("Memory", "Available Bytes").RawValue / BufSize/2);

        public int NumOfCompressorWorkers => Environment.ProcessorCount > 2
            ? Environment.ProcessorCount +2
            : Environment.ProcessorCount == 1 ? 1 : 2;

        #endregion Properties

        #region injection of work

        private readonly IWork _readWork;
        private readonly IWork _writeWork;
        private readonly List<IWork> _compressWork;

        #endregion injection



        readonly IFiFo _outQueue;


        public void Coordinate()
        {
            try
            {
                Process();
            }
            catch (Exception)
            {
                Terminate();
                throw;
            }

        }

    


        public void Process()
        {
            Console.WriteLine("Begin reading file");
            _readWork.Start();

            Console.WriteLine("Begin compress/decompressing data");
            _compressWork.ForEach(cw => cw.Start());

            Console.WriteLine("Begin writing to a new file");
            _writeWork.Start();


           Console.WriteLine("Wait for compress/decompress to be done");
            _compressWork.ForEach(cw =>
            {
                cw.WaitFor();
                Console.WriteLine("Compress/decompress work {0} finished", cw.Name);
            });
          //  _outQueue.Put(FiFo.Chunk.Empty);
            Console.WriteLine("Wait for write to be done");
            _writeWork.WaitFor();

            Console.WriteLine("Write finished");
        }

        public Coordinator(string inFile, string outFile, Activity activity)
        {
            IFiFo inQueue = new FiFo();
            _outQueue = new FiFo();

            _readWork = new Work<BaseWorker>(new ReadWorker(MaxItems, BufSize,
                inFile, inQueue, activity));
            _writeWork = new Work<BaseWorker>(new WriteWorker(outFile, _outQueue));

            _compressWork = Enumerable.Range(0, NumOfCompressorWorkers).Select(w => new Work<BaseWorker>(
                new CompressWorker(inQueue, _outQueue, activity,w))).Cast<IWork>().ToList();
        }

        public void Terminate()
        {
            _readWork.Abort();
            _compressWork.ForEach(cw => cw.Abort());
            _writeWork.Abort();
        }
    }
}
