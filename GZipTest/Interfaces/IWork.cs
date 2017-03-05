namespace TestTask.Interfaces
{

    /// <summary>
    /// Work interface 
    /// </summary>
    public interface IWork
    {
        /// <summary>
        /// Name of work
        /// </summary>
        string Name { get; }

        /// <summary>
        /// work result (0 - fail, 1 - win)
        /// </summary>
         bool Result { get; }

        /// <summary>
        ///  Start work
        /// </summary>
        void Start();

        /// <summary>
        /// Wait for work to complete
        /// </summary>
        void WaitFor();

        /// <summary>
        /// Abort work
        /// </summary>
        void Abort();

    }
}
