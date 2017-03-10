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


        public string Name => _instance.Name;


   
        public Work(T worker)
        {
            _instance = new Thread(() => worker.Run())
            {
                Name = worker.ToString()
            };
        }

        public void Start()
        {
            _instance.Start();
            
        }

        public void WaitFor()
        {
            _instance.Join();
        }

        public void Abort()
        {
            _instance.Abort();
        }


    }
}