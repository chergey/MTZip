namespace TestTask.Interfaces
{
    /// <summary>
    /// Worker interface
    /// </summary>
    public interface IWorker
    {
       /// <summary>
       /// Run worker
       /// </summary>
        void Run();

        /// <summary>
        /// Stop worker
        /// </summary>
        void Stop();

    }
}
