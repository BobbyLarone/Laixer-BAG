using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GMLTest
{
    class Program
    {
        static LaixerBagReader justRead;

        static void Main(string[] args)
        {
            justRead = new LaixerBagReader();

            var timer = new Stopwatch();
            timer.Start();

            //justRead.TheFkingFile();
            //justRead.TheScopes();
            //justRead.TheNameSpaces();
            justRead.withXML();

            timer.Stop();

            Console.WriteLine($"Read within {timer.Elapsed.TotalSeconds} seconds");
            Console.ReadLine();
        }
    }
}
