using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LaixerGMLTest
{
    internal sealed class Program
    {
        private static DirectoryReader directoryReader;
        private string path;
        private ILoader loader;
        private List<BAGObject> list;
        private List<string> directories;

        private static int fileCount = 0;

        private Program Extract(string path)
        {
            // TODO: Step 1

            // Make a new directory reader
            directoryReader = new DirectoryReader();
            
            // Store the path to this folder
            this.path = path;
            
            // First set the loader
            directoryReader.SetLoader(loader);

            // Then read the filepath
            directoryReader.readFolder(path);
            
            // Adjust the directory to read from the specified directory
            directoryReader.SetDirectoryNumber(2); // read the map : Openbare Ruimtes

            // Get the amount of files in the directory
            fileCount = directoryReader.GetFileCountInDirectory();

            return this;
        }
        private Program Process(int fileNumber)
        {
            // TODO: Step2

            // Get a list of directories from that filepath from above
            directories = directoryReader.GetListOfDirectories();

            var files = directoryReader.GetListOfFilesInDirectory();

            // Generate a list of BAGObjects
            directoryReader.ReadFile(files[fileNumber]);

            // retrieve the list of BagObjects
            list = directoryReader.GetAllObjects();

            return this;
        }

        private Program Load<TLoader>()
            where TLoader : ILoader, new()
        {
            // TODO: Step 3

            loader = new TLoader();
            loader.Load(list);
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

            //var processedObjects = new Program()
            //    .Extract(path: args[0])
            //    .Process()
            //    .Load<DatabaseLoader>();

            // !!! This part does not fully follow the ETL pattern but comes close to it ?.. (>.<)

            // run Extract once for now..
            new Program().Extract(path: args[0]);
            
            for(int x = 0; x < fileCount;x++)
            {
                new Program().Process(x)
                .Load<DatabaseLoader>();
                Console.WriteLine($"Loaded file number {x}");
            }

            timer.Stop();
            Console.WriteLine($"Read within {timer.Elapsed.TotalSeconds} seconds");
            Console.ReadLine();
        }
    }
}
