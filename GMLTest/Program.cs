using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LaixerGMLTest
{
    internal sealed class Program
    {
        private static DirectoryReader directoryReader;
        private string path;
        private ILoader loader;
        private List<BAGObject> list;
        private List<string> directories;
        private static int batchSize = 5;
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

        //private Program Load<TLoader>()
        //    where TLoader : ILoader, new()
        //{
        //    // TODO: Step 3

        //    // await Task.Yield();

        //    loader = new TLoader();
        //    loader.Load(list);
        //    return this;
        //}

        //private static async Task Main(string[] args)
        private static async Task Main(string[] args)
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

            // run Extract once
            new Program().Extract(path: args[0]);

            int whole = fileCount / batchSize;
            int rest = fileCount % batchSize;
            int total = whole + rest;
            int numberOfFile = 0;
                //.Process(0)
                //.Load<DatabaseLoader>();

            Task[] taskList = new Task[batchSize];

            //taskList[0] = new Program()
            //        .Process(0)
            //        .Load<DatabaseLoader>();

            //taskList[1] = new Program()
            //        .Process(1)
            //        .Load<DatabaseLoader>();

            //taskList[2] = new Program()
            //        .Process(2)
            //        .Load<DatabaseLoader>();

            //await Task.WhenAll(taskList);

            // loop through the files 10 at a time
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



            //for (int x = 0; x < fileCount; x++)
            //{
            //    new Program()
            //        .Process(x)
            //        .Load<DatabaseLoader>();

            //    Console.WriteLine($"Loaded file number {x}");
            //}

            timer.Stop();
            Console.WriteLine("Push complete!");
            Console.WriteLine($"Read within {timer.Elapsed.TotalSeconds} seconds");
            Console.ReadLine();
        }
    }
}
