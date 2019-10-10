using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LaixerGMLTest
{
    /// <summary>
    ///  Class to read the content of the specified directory
    /// </summary>
    internal class DirectoryReader
    {
        private readonly uint directoryDepth = 0;
        private int readDirectoryFolder;
        private ILoader loader;

        private LaixerBagReader myReader;
        private string folderPath;
        private List<string> listOfFilesInDirectory;
        private List<string> listOfDirectories;

        /// <summary>
        /// The constructor
        /// </summary>
        public DirectoryReader()
        {
            // This is a constructor.... (^.^)b
        }

        public void SetLoader(ILoader loader)
        {
            this.loader = loader;
        }

        /// <summary>
        /// Read the content of an directory and show the amount of files and directories found
        /// </summary>
        /// <param name="filePath">The path to the directory</param>
        public void readFolder(string filePath)
        {
            folderPath = filePath;
            readFolderContentAsync(filePath);
        }

        /// <summary>
        /// read the folder async ( for now this is still WIP)
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <param name="subFiles">Enable reading the subfiles</param>
        /// <param name="readFirst">Enable reading the first map and first file in that map</param>
        private void readFolderContentAsync(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                // Keep a list of directories and files
                listOfDirectories = Directory.EnumerateDirectories(filePath).ToList();
                listOfFilesInDirectory = Directory.EnumerateFiles(filePath).ToList();
                Console.WriteLine($"Found: {listOfDirectories.Count} Directories and {listOfFilesInDirectory.Count} Files");
                Console.WriteLine("These are the directories found in the current directory:");

                foreach (var path in listOfDirectories)
                {
                    Console.WriteLine($"\tFound: {path}");
                }
            }
        }


        /// <summary>
        /// Read the file
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadFile(string filePath)
        {
            ReadFileAsync(filePath);
        }

        /// <summary>
        /// Read the file
        /// </summary>
        /// <param name="filePath"></param>
        private void ReadFileAsync(string filePath)
        {
            Console.WriteLine($"Going to read the following file : {Path.GetFileName(filePath)}");

            myReader = new LaixerBagReader();
            // read the file
            myReader.ReadXML(filePath);
        }

        /// <summary>
        /// Return the total amount of objects
        /// </summary>
        /// <returns></returns>
        public int GetTotalObjects()
        {
            // Making sure that a reader exists
            return myReader == null ? 0 : myReader.listOfBAGObjects.Count;
        }

        /// <summary>
        /// Return a list of objects
        /// </summary>
        /// <returns>list of BAGobjects</returns>
        public List<BAGObject> GetAllObjects()
        {
            // Making sure that a reader exists
            return myReader == null ? null : myReader.listOfBAGObjects;
        }

        /// <summary>
        /// Get the amount of files in te directory
        /// </summary>
        /// <returns></returns>
        public int GetFileCountInDirectory()
        {
            return string.IsNullOrEmpty(folderPath) ? 0 : Directory.EnumerateFiles(listOfDirectories[readDirectoryFolder]).ToList().Count;
        }


        /// <summary>
        /// Get a list of directories
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfDirectories()
        {
            return string.IsNullOrEmpty(folderPath) ? null : listOfDirectories;
        }

        public List<string> GetListOfFilesInDirectory()
        {
            return string.IsNullOrEmpty(folderPath) ? null : Directory.EnumerateFiles(listOfDirectories[readDirectoryFolder]).ToList();
        }

        /// <summary>
        /// Set the directory to read
        /// </summary>
        /// <param name="number">the number of the directory</param>
        public void SetDirectoryNumber(int number)
        {
            readDirectoryFolder = number;
        }

        //TODO: Create function that reads a directory based on the name
    }
}
