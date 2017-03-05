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
       /// <returns>0 - fail, 1 - win </returns>
        bool Run();

        /// <summary>
        /// Stop worker
        /// </summary>
        void Stop();

    }
}
