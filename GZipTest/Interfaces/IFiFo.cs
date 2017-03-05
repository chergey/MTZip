using TestTask.Imp;

namespace TestTask.Interfaces
{
    /// <summary>
    /// FIFO queue (ideally, should be concurrent)
    /// </summary>
    public interface IFiFo
    {
        /// <summary>
        /// Get number of elements in a queue
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Put element to queue
        /// </summary>
        /// <param name="elem">element to queue</param>
        void Put(FiFo.Chunk elem);

        /// <summary>
        /// Get element from queue 
        /// </summary>
        /// <param name="data">element to get</param>
        /// <returns></returns>
        bool Get(out FiFo.Chunk data);
    }
}