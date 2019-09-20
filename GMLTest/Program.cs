using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LaixerGMLTest
{
    internal sealed class Program
    {
        private static int batchSize = 5;
        private static DirectoryReader directoryReader;
        private string path;
        private ILoader loader;
        private List<BAGObject> list;
        private List<string> directories;
        private static int dirToRead = 4;

        private static int fileCount;

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
            directoryReader.SetDirectoryNumber(dirToRead);

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

        private async Task<Program> Load<TLoader>()
            where TLoader : ILoader, new()
        {
            // TODO: Step 3
            await Task.Yield();

            loader = new TLoader();
            await loader.LoadAsync(list);
            return this;
        }

        private static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"tool.exe [path]");
                return;
            }

            var timer = new Stopwatch();
            timer.Start();

            // run Extract once
            new Program().Extract(path: args[0]);

            int whole = fileCount / batchSize;
            int rest = fileCount % batchSize;
            int total = whole + rest;
            int numberOfFile = 0;

            Task[] taskList = new Task[batchSize];

            // loop through the files
            for(int x = 0; x< whole;x++)
            {
                for( int i =0; i< batchSize; i++)
                {
                    taskList[i] = new Program()
                            .Process(numberOfFile)
                            .Load<DatabaseLoader>();
                    numberOfFile++;
                }
                await Task.WhenAll(taskList);
            }

            if(rest>0)
            {
                // for the remaining files
                for (int y = 0; y<rest;y++)
                {
                    taskList[y] = new Program()
                            .Process(numberOfFile)
                            .Load<DatabaseLoader>();
                    numberOfFile++;
                }
                await Task.WhenAll(taskList);
            }

            timer.Stop();
            Console.WriteLine("Push complete!");
            Console.WriteLine($"Read within {timer.Elapsed.TotalSeconds} seconds");
            Console.ReadLine();
        }
    }
}
