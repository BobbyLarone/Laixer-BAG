using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GMLTest
{
    class Program
    {
        static string filePath = @"C:\Users\Workstation\Documents\MyProjects\Documents\XML";
        static string filePath2 = @"C:\Users\Workstation\Documents\Fundermaps\Documents\BAG data\inspireadressen";

        static void Main(string[] args)
        {
            var justRead = new LaixerBagReader();
            var reader = new DirectoryReader();

            var timer = new Stopwatch();
            timer.Start();

            //justRead.TheFkingFile();
            //justRead.TheScopes();
            //justRead.TheNameSpaces();
            //justRead.withXML();

            //reader.readFolder(filePath);
            reader.readFolder(filePath2);

            timer.Stop();

            Console.WriteLine($"Read within {timer.Elapsed.TotalSeconds} seconds");
            Console.ReadLine();
        }
    }
}
