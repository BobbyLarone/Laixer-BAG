using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LaixerGMLTest
{
    internal sealed class Program
    {
        private static readonly int batchSize = 10;
        private static DirectoryReader directoryReader;
        private string path;
        private ILoader loader;
        private List<BAGObject> list;
        private static int amountOfDirectories;
        private static int fileCount;

        private Program Extract(string path)
        {
            // TODO: Step 1
            directoryReader = new DirectoryReader();// Make a new directory reader

            directoryReader.readFolder(path);// Then read the filepath

            amountOfDirectories = directoryReader.GetListOfDirectories().Count;// Store the amount of directories in this variable

            directoryReader.SetDirectoryNumber(0);// Adjust the directory to read from the first directory found

            fileCount = directoryReader.GetFileCountInDirectory();// Get the amount of files in the first directory

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

        private Program Transform(string filePath)
        {
            // TODO: Step2
            directoryReader = new DirectoryReader();// Make a new directory reader

            // Generate a list of BAGObjects form the list of files 
            // by reading the file in the position of the filenumber
            directoryReader.ReadFile(filePath);

            // retrieve the list of BagObjects
            list = directoryReader.GetAllObjects();
            var a = 1;

            return this;
        }

        // TODO: Rewrite to not use Async as this will create race conditions for parallel threading.
        private async Task<Program> Load<TLoader>()
            where TLoader : ILoader, new()
        {
            // TODO: Step 3
            await Task.Yield();

            loader = new TLoader();

            //await loader.LoadAsync(list).ConfigureAwait(false);

            return this;
        }

        private static async Task Main(string[] args)
        {
            // Make sure that when this program is ran from the CMD that an argument is passed
            if (args.Length == 0)
            {
                Console.WriteLine(Properties.Resources.NoPathAsArgument);
                Console.ReadLine();
                return;
            }

            // Make sure that the path passed as an argument exists
            if(!Directory.Exists(args[0]))
            {
                Console.WriteLine(Properties.Resources.DirectoryNotFound);
                return;
            }

            #region Get information about the rootfolder
            // Because the folder is small, this can be done synchronously. 
            // The paralell overhead is greater then doing this synchronously.

            var timer = new Stopwatch();
            timer.Start();

            new Program().ParalellReaderForBAG(args[0]);

            timer.Stop();
            Console.WriteLine($@"Run through folder in : {timer.Elapsed.TotalSeconds}");
            #endregion

            // RUNS THROUGH ENTIRE FOLDER IN : 2275 seconds -> 37 minuten

            //NOTE: Bottleneck is the database connection and computer memory and maybe the amount of cores
            //TODO: Implement Dapper Transaction

            var useless = 1;

            Console.WriteLine(Properties.Resources.DatabasePushComplete);
            Console.ReadLine();
        }

        /// <summary>
        /// Run the BAG reader as an Asynch Task based program
        /// </summary>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        private async Task<Program> AsyncTaskBasedBAGReader(string rootPath)
        {
            // run Extract once to get information about the root folder
            new Program().Extract(path: rootPath);


            // Loop through the folders in the root map
            for (var x = 0; x < amountOfDirectories; ++x)
            {
                directoryReader.SetDirectoryNumber(x); // Adjust the directory to read from the directory

                fileCount = directoryReader.GetFileCountInDirectory();// Get the amount of files in the directory

                int whole = fileCount / batchSize; // calculate how many batches it needs to run based on the batchSize

                int rest = fileCount % batchSize; // calculates howmany files there are left

                int numberOfFile = 0; // counter to increment to the next file in a list


                if (whole > 0)// loop through the files
                {
                    Task[] taskList = new Task[batchSize];// Holds the amount of tasks based on the batchsize

                    for (var y = 0; y < whole; ++y)// loop through all the files
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
                    Task[] taskList2 = new Task[rest];// Create a list of task based on how many files there are left

                    for (int z = 0; z < rest; ++z)// Loop through the remaining files
                    {
                        taskList2[z] = new Program()
                                .Process(numberOfFile)
                                .Load<DatabaseLoader>();
                        numberOfFile++;
                    }
                    await Task.WhenAll(taskList2).ConfigureAwait(false);
                }
            }

            return this;
        }

        /// <summary>
        /// Run the BAG Reader as a Paralell Task based program
        /// </summary>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        private Program ParalellReaderForBAG(string rootPath)
        {
            var listOfDirectories = Directory.EnumerateDirectories(rootPath).ToList(); // Get a list of directories in the rootmap

            //var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };

            Parallel.ForEach(listOfDirectories, (directoryInList) =>
            {
                var listOfFiles = Directory.EnumerateFiles(directoryInList).ToList();
                //var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };

                Parallel.ForEach(listOfFiles, (fileInList) =>
                {
                    Console.WriteLine(fileInList);

                    new Program().Transform(fileInList).Load<DatabaseLoader>(); //.Load<DatabaseLoader>();
                });
            });

            return this;
        }
    }
}
