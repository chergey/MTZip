using TestTask.Enums;
using TestTask.Interfaces;

namespace TestTask.Imp
{

    /// <summary>
    /// Worker skeleton  : all workers inherit from it
    /// </summary>
    public abstract class BaseWorker : IWorker
    {
        public abstract bool Run();
        public abstract void Stop();

        /// <summary>
        /// Check chunk for correct data
        /// </summary>
        /// <param name="chunk"></param>
        public abstract void Check(FiFo.Chunk chunk);

        /// <summary>
        /// time for worker to sleep in case of no data present or another worker operating
        /// </summary>
        protected int SleepTime;

        /// <summary>
        /// type of acitivity assigned to worker
        /// </summary>
        protected Activity WorkActivity;



    }
}
