
using TestTask.Enums;
using TestTask.Imp;

namespace TestTask.Interfaces
{
    public interface ICoordinator
    {
        int BufSize { get; }
        int MaxItems { get; }
        int NumOfCompressorWorkers { get; }
        
        /// <summary>
        /// Coordinate 
        /// </summary>
        /// <returns>sucess or failure</returns>
        bool Coordinate();
        
        /// <summary>
        /// post process
        /// </summary>
        void PostProcessWork();

        /// <summary>
        /// Process reading and compressing work
        /// </summary>
        void Process();

        /// <summary>
        /// Stop all the processes
        /// </summary>
        void Terminate();
    }
}