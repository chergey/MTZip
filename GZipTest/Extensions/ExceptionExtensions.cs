using System;
using TestTask.Properties;

namespace TestTask
{
    /// <summary>
    /// No comments
    /// </summary>
    static class ExceptionExtensions
    {

        public static void DumpException(this Exception e, string fileName)
        {
            switch (e.GetType().Name)
            {

                case "OutOfMemoryException":
                    Console.WriteLine(Resources.NotEnoughMem + e.Message);
                    break;

                case "ArgumentException":
                    Console.WriteLine(Resources.WrongArg + e.Message);
                    break;

                case "IOException":
                    Console.WriteLine(Resources.CantAccessFile, fileName);
                    Console.WriteLine(e.Message);
                    break;

                case "UnauthorizedAccessException":

                    Console.WriteLine(Resources.DontHavePerm, fileName);
                    Console.WriteLine(e.Message);
                    break;

                default:
                    Console.WriteLine(e.Message);
                    break;

            }
        }

        public static void DumpException(this Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}