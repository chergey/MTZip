using System.Threading;
using TestTask.Interfaces;

namespace TestTask.Imp
{

    /// <summary>
    /// Class implemening work
    /// </summary>
    /// <typeparam name="T">type implementing IWorker</typeparam>
    public class Work<T> : IWork  where T : IWorker
    {
        private readonly Thread _instance;
        private bool _result;


        public string Name => _instance.Name;

        public bool Result => _result;

   
        public Work(T worker)
        {
            _instance = new Thread(() => _result=worker.Run())
            {
                Name = worker.ToString()
            };
        }
        /// <summary>
        /// Start work assigned to worker
        /// </summary>

        public void Start()
        {
            _instance.Start();
            
        }
        /// <summary>
        /// Wait for work to be finished
        /// </summary>

        public void WaitFor()
        {
            _instance.Join();
        }
        /// <summary>
        /// Abort work
        /// </summary>

        public void Abort()
        {
            _instance.Abort();
        }


    }
}