using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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
        private static int fileCount;

        private Program Extract(string path, out bool succes)
        {
            // TODO: Step 1

            // Store the path to this folder
            this.path = path;

            // Make a new directory reader
            directoryReader = new DirectoryReader();

            // Then read the filepath
            directoryReader.readFolder(path,out bool exists);

            if(exists)
            {
                // First set the loader
                directoryReader.SetLoader(loader);

                // Store the amount of directories in this variable
                amountOfDirectories = directoryReader.GetListOfDirectories().Count;

                // Adjust the directory to read from the first directory found
                directoryReader.SetDirectoryNumber(0);

                // Get the amount of files in the first directory
                fileCount = directoryReader.GetFileCountInDirectory();

                succes = true;
                return this;
            }
            else
            {
                // return null if the directory doesn't exist
                succes = false;
                return null;
            }

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

        // TODO: Rewrite to not use Async as this will create race conditions for parallel threading.
        private async Task<Program> Load<TLoader>()
            where TLoader : ILoader, new()
        {
            // TODO: Step 3
            await Task.Yield();

            loader = new TLoader();

            await loader.LoadAsync(list).ConfigureAwait(false);

            return this;
        }

        private static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(Properties.Resources.NoPathAsArgument);
                Console.ReadLine();
                return;
            }

            // run Extract once to get information about the root folder
            new Program().Extract(path: args[0], out bool succes);

            // if the extract was not succesfull we can get out of the program
            if(!succes)
            {
                Console.WriteLine(Properties.Resources.DirectoryNotFound);
                return;
            }

            //TODO: Transform into Parallel loops

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
                                    .Process(numberOfFile) // process the data
                                    .Load<DatabaseLoader>(); // load the data into the database

                            // Increase the file number so that we read the next file
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

            Console.WriteLine(Properties.Resources.DatabasePushComplete);
            Console.ReadLine();
        }

        private Program ParalellReaderForBAG()
        {

            Parallel.For(0, amountOfDirectories, a =>
            {
                int directoryNumber = 1;
                // Adjust the directory to read from the directory
                directoryReader.SetDirectoryNumber(directoryNumber);

                // Get the amount of files in the directory
                fileCount = directoryReader.GetFileCountInDirectory();

                //NOTE: This will be absolete if the paralell methods are implemented
                // Calculate how many batches it needs to run based on the batchSize
                int whole = fileCount / batchSize;

                // calculates howmany files there are left
                int rest = fileCount % batchSize;

                // counter to increment to the next file in a list
                int numberOfFile = 0;

                // loop through all the files
                Parallel.For(0, fileCount, b =>
                {
                    Interlocked.Increment(ref numberOfFile); // increase the file number
                    new Program().Process(numberOfFile).Load<DatabaseLoader>();

                });

                for (var y = 0; y < whole; ++y)
                {
                    for (var i = 0; i < batchSize; ++i)
                    {
                        new Program().Process(numberOfFile).Load<DatabaseLoader>();

                        // Increase the file number so that we read the next file
                        numberOfFile++;
                    }
                }
            });



            return this;
        }
    }
}
