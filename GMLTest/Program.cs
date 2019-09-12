using System;
using System.Diagnostics;

namespace LaixerGMLTest
{
    internal sealed class Program
    {
        private DirectoryReader directoryReader;
        private string path;
        private ILoader loader;

        private Program Extract(string path)
        {
            // TODO: Step 1

            this.path = path;
            return this;
        }

        private Program Load<TLoader>()
            where TLoader : ILoader, new()
        {
            // TODO: Step 2

            loader = new TLoader(); // TODO: Fow now
            return this;
        }

        private Program Process()
        {
            // TODO: Run
            directoryReader = new DirectoryReader(loader); // TODO: Fow now
            directoryReader.readFolder(path);
            return this;
        }

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"tool.exe [path]");
                return;
            }

            var timer = new Stopwatch();
            timer.Start();

            var processedObjects = new Program()
                .Extract(path: args[0])
                .Load<DatabaseLoader>()
                .Process();

            timer.Stop();
            Console.WriteLine($"Made {processedObjects} objects!");
            Console.WriteLine($"Read within {timer.Elapsed.TotalSeconds} seconds");
            Console.ReadLine();
        }
    }
}
