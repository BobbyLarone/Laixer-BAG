using System;
using System.Collections.Generic;
using System.IO;
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

        public DirectoryReader()
        {

        }

        public void readFolder(string filePath)
        {

            readFolderAsync(filePath);
        }

        private void readFolderAsync(string filePath)
        {
            if(Directory.Exists(filePath))
            {
                var listOfDirectories = Directory.EnumerateDirectories(filePath);
                var listOfFiles = Directory.EnumerateFiles(filePath);

                foreach(var path in listOfDirectories)
                {
                    Console.WriteLine($"Found: {path}");
                }

                Console.WriteLine("These are the files found in the current directory");

                foreach (var file in listOfFiles)
                {
                    Console.WriteLine($"Found: {file}");
                }
            }
        }
    }
}
