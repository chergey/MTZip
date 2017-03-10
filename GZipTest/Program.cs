using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using TestTask.Enums;
using TestTask.Imp;
using TestTask.Interfaces;
using TestTask.Properties;

namespace TestTask
{

    class Program
    {
        private static Activity _activity;
        private static ICoordinator _coordinator;

        static void Main(string[] args)
        {

            Console.Title = Resources.MainTitle;
            var tempColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Blue;

            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                Console.WriteLine(e);

            };

            var tempArgs = args;
            while (true)
            {

                if (!ParseArgs(tempArgs))
                {
                    return;
                }

                _coordinator = new Coordinator(tempArgs[1], tempArgs[2], _activity);

                Console.CancelKeyPress += (o, e) =>
                {
                    Console.WriteLine(Resources.CancelledOperation);
                    _coordinator.Terminate();
                };

                var sw = new Stopwatch();
                sw.Start();
                Console.WriteLine("Begin compression ");
                try
                {
                    _coordinator.Coordinate();
                    Console.WriteLine("Operation finished with success ");

                    var fileOrig = new FileInfo(tempArgs[1]);
                    var fileNew = new FileInfo(tempArgs[2]);
                    Console.WriteLine(Resources.CompressionRate +
                                      (decimal)fileOrig.Length / fileNew.Length);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Operation finished with failure: " + e);

                }
                
                Console.WriteLine(Resources.TimeElapsed + sw.Elapsed);


                Console.WriteLine(Resources.PressEnter);

                var key = Console.ReadKey().Key;
                if (key != ConsoleKey.Enter)
                {
                    Console.BackgroundColor = tempColor;
                    return;
                }
                tempArgs = Console.ReadLine().Split(' ');


            }
           
        }

        /// <summary>
        /// Parse arguments 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool ParseArgs(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine(Resources.NotEnoughPar);
                return false;
            }
            switch (args[0].ToLower())
            {
                case "compress":
                    _activity = Activity.Compress;
                    break;

                case "decompress":
                    _activity = Activity.Decompress;
                    break;

                case "help":
                    ShowUsage();
                    break;

                default:
                    Console.WriteLine(Resources.WrongArg + args[0]);
                    return false;

            }

            if (!File.Exists(args[1]))
            {
                Console.WriteLine(Resources.FileMissing, args[1]);
                return false;
            }

            return true;
        }
        /// <summary>
        /// Show usage
        /// </summary>
        private static void ShowUsage()
        {
            Console.WriteLine(Resources.ToCompress);
            Console.WriteLine(Resources.ToDecompress);
        }
    }
}
