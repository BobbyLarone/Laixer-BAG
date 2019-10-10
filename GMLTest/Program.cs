using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LaixerGMLTest
{
    internal sealed class Program
    {
        private static int batchSize = 10;
        private static DirectoryReader directoryReader;
        private string path;
        private ILoader loader;
        private List<BAGObject> list;
        private static int amountOfDirectories;
        private static int dirToRead = 7;
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

            // Get a list of directories from that filepath from above
            var directories = directoryReader.GetListOfDirectories();

            // Store the amount of directories in this variable
            amountOfDirectories = directories.Count;

            // Adjust the directory to read from the first directory found
            directoryReader.SetDirectoryNumber(0);

            // Get the amount of files in the first directory
            fileCount = directoryReader.GetFileCountInDirectory();

            return this;
        }
        private Program Process(int fileNumber)
        {
            // TODO: Step2

            // Get a list of files from the directory
            var files = directoryReader.GetListOfFilesInDirectory();

            // Generate a list of BAGObjects form the list of files 
            // by reading the file in the position of the filenumber
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

            // run Extract once to get information about the root folder
            new Program().Extract(path: args[0]);

            // Loop through the folders in the root map
            for (var x = 0; x < amountOfDirectories; ++x)
            {
                // Adjust the directory to read from the directory
                directoryReader.SetDirectoryNumber(x);

                // Get the amount of files in the directory
                fileCount = directoryReader.GetFileCountInDirectory();

                // calculate how many batches it needs to run based on the batchSize
                int whole = fileCount / batchSize;

                // calculates howmany files there are left
                int rest = fileCount % batchSize; 

                // counter to increment to the next file in a list
                int numberOfFile = 0;

                // loop through the files
                if (whole > 0)
                {
                    // Holds the amount of tasks
                    Task[] taskList = new Task[batchSize];
                    
                    // loop through all the files
                    for (var y = 0; y < whole; ++y)
                    {
                        for (var i = 0; i < batchSize; ++i)
                        {
                            taskList[i] = new Program()
                                    .Process(numberOfFile)
                                    .Load<DatabaseLoader>();
                            numberOfFile++;
                        }
                        await Task.WhenAll(taskList).ConfigureAwait(false);
                    }
                }

                if (rest > 0)
                {
                    // Create a list of task based on how many files there are left
                    Task[] taskList2 = new Task[rest];
                    // Loop through the remaining files
                    for (int z = 0; z < rest; ++z)
                    {
                        taskList2[z] = new Program()
                                .Process(numberOfFile)
                                .Load<DatabaseLoader>();
                        numberOfFile++;
                    }
                    await Task.WhenAll(taskList2).ConfigureAwait(false);
                }
            }

            Console.WriteLine("Push complete!");
            Console.ReadLine();
        }
    }
}
