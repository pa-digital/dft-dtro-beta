using System.Linq;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
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

        public static string CreateTempFileWithTraUpdated(string schemaVersion, string fileName, string userGuid, string traId)
        {
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersion, createDtroJson, traId);
            string uniquePrefixOnFileName = $"{userGuid.Substring(0, 5)}-";
            string nameOfCopyFile = $"{uniquePrefixOnFileName}{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithTraUpdated);
            return tempFilePath;
        }
    }
}