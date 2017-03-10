using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TestTask.Interfaces;

namespace TestTask.Imp
{
  
    public class FiFo : IFiFo
    {
        /// <summary>
        /// Unit of data transferrable among threads
        /// </summary>
        public class Chunk
        {

            public static Chunk Empty => new Chunk {CheckPoint = -1};
            public int CheckPoint;
            public byte[] Data;

        }

        private readonly Queue<Chunk> _queue;
        private readonly object _queueLock;

        public FiFo()
        {
            _queue = new Queue<Chunk>();
            _queueLock = new object();
        }
        /// <summary>
        /// Get number of elements in a queue
        /// </summary>
        public int Count
        {
            get
            {
                lock (_queueLock)
                {
                    return _queue.Count;
                }
            }
        }
        /// <summary>
        /// Put element to queue
        /// </summary>
        /// <param name="elem">element to queue</param>
        public void Put(Chunk chunk)
        {
            lock (_queueLock)
            {
            //   if (!_queue.Contains(chunk))
                {
                    _queue.Enqueue(chunk);
                }

            }
        }
        /// <summary>
        /// Get element from queue 
        /// </summary>
        /// <param name="data">element to get</param>
        /// <returns></returns>
        public bool Get(out Chunk data)
        {
            data = default(Chunk);
            lock (_queueLock)
            {
                if (_queue.Count > 0)
                {
                    data = _queue.Dequeue();
                    return true;
                }
            }
            return false;
        }
    }
}