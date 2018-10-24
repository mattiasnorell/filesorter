using System;
using System.IO;
using System.Linq;

namespace FileSorter
{
    class Program
    {
        static void Main(string[] args)
        {

            if(string.IsNullOrEmpty(args[0])){
                throw new Exception("No input folder");
            }

            if(string.IsNullOrEmpty(args[1])){
                throw new Exception("No output folder");
            }

            var sourceDirectory = args[0];     
            var destinationDirectory = args[1];
            var files = Directory.EnumerateFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);
            var filesCount = files.Count();
            var currentFileIndex = 0;

            foreach (string filePath in files)
            {
                currentFileIndex++;
                Console.Write($"\rMoving file {currentFileIndex} of {filesCount}");

                var fileModifiedDate = File.GetLastWriteTime(filePath);
                var destinationDirectoryName = fileModifiedDate.ToString("yyyy-MM-dd");
                var directoryPath = Path.Combine(destinationDirectory, destinationDirectoryName);
                var fileDestination = Path.Combine(directoryPath, Path.GetFileName(filePath));

                if(!Directory.Exists(directoryPath)){
                    Directory.CreateDirectory(directoryPath);
                }

                if(File.Exists(fileDestination)){
                    fileDestination = GetUniqeFilename(filePath, directoryPath);
                }

                File.Move(filePath, fileDestination);
            }
        }

        static string GetUniqeFilename(string sourcePath, string destinationPath, int index = 1){
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(sourcePath);
            var fileExt = Path.GetExtension(sourcePath);
            var fileName = $"{fileNameWithoutExt}_{index}{fileExt}";
            var newFileDestination = Path.Combine(destinationPath, fileName);
            
            if(File.Exists(newFileDestination)){
                return GetUniqeFilename(sourcePath, destinationPath, index + 1);
            }

            return newFileDestination;
        }
    }
}