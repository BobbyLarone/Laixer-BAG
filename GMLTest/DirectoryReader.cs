using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMLTest
{
    /// <summary>
    ///  Class to read the content of the specified directory
    /// </summary>
    class DirectoryReader
    {
        private static readonly string fileDirectory = "";
        private string[] splitFile;
        private uint directoryDepth = 0;

        public DirectoryReader()
        {

        }

        /// <summary>
        /// Read the content of an directory and show the amount of files and directories found
        /// </summary>
        /// <param name="filePath">The path to the directory</param>
        public void readFolder(string filePath)
        {

            readFolderContentAsync(filePath,true,true);
        }

        /// <summary>
        /// read the folder async ( for now this is still WIP)
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <param name="subFiles">Enable reading the subfiles</param>
        /// <param name="readFirst">Enable reading the first map and first file in that map</param>
        private void readFolderContentAsync(string filePath,bool subFiles = false, bool readFirst = false)
        {
            if(Directory.Exists(filePath))
            {
                // This is to keep track of the amount of files
                uint fileCount = 0;

                // Specify the amount of files to read.
                uint maxFileCount = 2;

                // Keep a list of directories and files
                var listOfDirectories = Directory.EnumerateDirectories(filePath).ToList();
                var listOfFiles = Directory.EnumerateFiles(filePath).ToList();

                Console.WriteLine($"Current directory depth = {directoryDepth}");
                Console.WriteLine($"Found: {listOfDirectories.Count} Directories and {listOfFiles.Count} Files");
                Console.WriteLine("These are the directories found in the current directory:");

                foreach(var path in listOfDirectories)
                {
                    Console.WriteLine($"\tFound: {path}");
                }

                Console.WriteLine("\nThese are the files found in the current directory:");

                foreach (var file in listOfFiles)
                {
                    // If we reached the maximum amount of files to read.. we stop reading further
                    if(fileCount >= maxFileCount){ break; }

                    // Split the file on the " . " and store this in a new array.
                    splitFile = file.Split(".");

                    Console.WriteLine($"\tFound: {splitFile[0]}. Extension = {splitFile[1]}");
                    fileCount++;
                }

                // If read the files in the subFolders
                if(subFiles)
                {
                    foreach(var fileOrDirectory in listOfDirectories)
                    {
                        Console.WriteLine($"\nIn directory: {fileOrDirectory}");
                        directoryDepth++;
                        readFolderContentAsync(fileOrDirectory,subFiles);
                        directoryDepth--;
                    }
                }

                //TODO: transform this into a foreach so that it can read ALL the files

                // Read the first file of te first map
                if(readFirst)
                {
                    // Read the first map in the directory and make a list of the files
                    var filesInDirectory = Directory.EnumerateFiles(listOfDirectories[0]).ToList();

                    // Read the first file in the list
                    ReadFileAsync(filesInDirectory[0]);
                }
            }
        }


        public void ReadFile(string filePath)
        {
            ReadFileAsync(filePath);
        }

        private void ReadFileAsync(string filePath)
        {
            var myReader = new LaixerBagReader();
            Console.WriteLine($"Going to read the following file : {filePath}");

            // Make some room in the console
            Console.WriteLine("\n\n\n\n\n");
            myReader.withXML(filePath);

        }
    }
}
