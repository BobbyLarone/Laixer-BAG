using System;
using System.Diagnostics;

namespace LaixerGMLTest
{
    class Program
    {
        static string filePath; // = @"C:\Users\Workstation\Documents\MyProjects\Documents\XML";
        // folder to the files
        //static string filePath2 = @"C:\Users\Workstation\Documents\Fundermaps\Documents\BAG data\inspireadressen";

        static void Main(string[] args)
        {
            var reader = new DirectoryReader();

            var timer = new Stopwatch();
            timer.Start();

            if (args.Length == 0)
            {
                Console.WriteLine($"tool.exe [path]");
                return;
            }

            filePath = args[0];

            //reader.readFolder(filePath);
            reader.readFolder(filePath);

            timer.Stop();
            Console.WriteLine($"Made {reader.GetTotalObjects()} objects!");
            Console.WriteLine($"Read within {timer.Elapsed.TotalSeconds} seconds");
            Console.ReadLine();
        }
    }
}
