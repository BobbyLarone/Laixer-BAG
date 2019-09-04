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
                //List<string>
                var listOfDirectories = Directory.EnumerateDirectories(filePath).ToList();
                var listOfFiles = Directory.EnumerateFiles(filePath).ToList();

                Console.WriteLine($"Found: {listOfDirectories.Count} Directories and {listOfFiles.Count} Files");
                Console.WriteLine("These are the directories found in the current directory: ");

                foreach(var path in listOfDirectories)
                {
                    Console.WriteLine($"    Found: {path}");
                }

                Console.WriteLine("\nThese are the files found in the current directory:");

                foreach (var file in listOfFiles)
                {
                    var length = file.Length;
                    var splitFile = file.Split(".");
                    var result = splitFile[0];
                    Console.WriteLine($"    Found: {splitFile[0]}");
                    Console.WriteLine($"    " +
                        $"File has the following extension: {splitFile[1]}");


                }
            }
        }
    }
}
