using System.Linq;
using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Helpers
{
    public static class FileHelper
    {
        public static void WriteStringToFile(string directory, string fileName, string stringToWrite)
        {
            Directory.CreateDirectory(directory);
            string filePath = $"{directory}/{fileName}";
            File.WriteAllText(filePath, stringToWrite);
        }

        public static void DeleteFilesInDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);

                foreach (string file in files)
                {
                    File.Delete(file);
                    Console.WriteLine($"Deleted file {file}");
                }
            }
        }

        public static string[] GetFileNames(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                return Directory.GetFiles(directoryPath)
                                .Select(Path.GetFileName)
                                .ToArray();
            }
            else
            {
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");
            }
        }
    }
}