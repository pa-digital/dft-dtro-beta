using System.Linq;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using Newtonsoft.Json.Linq;
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

        public static string CreateTempFileWithTraModified(string schemaVersion, string fileName, string userGuidToGenerateFileNamePrefix, string traId)
        {
            string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);
            string uniquePrefixOnFileName = $"{userGuidToGenerateFileNamePrefix.Substring(0, 5)}-";
            string nameOfCopyFile = $"{uniquePrefixOnFileName}{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, dtroJsonWithTraModified);
            return tempFilePath;
        }

        public static string CreateTempFileWithTraModifiedAndProvisionReferenceDuplicated(string schemaVersion, string fileName, string userGuidToGenerateFileNamePrefix, string traId)
        {
            string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);
            string dtroWithDuplicateProvisionReference = JsonMethods.CloneFirstItemInArrayAndAppend(dtroJsonWithTraModified, "data.source.provision");
            string uniquePrefixOnFileName = $"{userGuidToGenerateFileNamePrefix.Substring(0, 5)}-";
            string nameOfCopyFile = $"{uniquePrefixOnFileName}{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, dtroWithDuplicateProvisionReference);
            return tempFilePath;
        }

        // public static string CreateTempFileWithTraModifiedAndExternalReferenceLastUpdateDateInFuture(string schemaVersion, string fileName, string userGuidToGenerateFileNamePrefix, string traId)
        // {
        //     string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
        //     string dtroJson = File.ReadAllText(dtroFile);
        //     string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);

        //     JObject jsonObj = JObject.Parse(dtroJsonWithTraModified);
        //     jsonObj["data"]["source"]["provision"][0]["regulatedPlace"][0]["pointGeometry"]["point"] = pointGeometryString;

        //     string uniquePrefixOnFileName = $"{userGuidToGenerateFileNamePrefix.Substring(0, 5)}-";
        //     string nameOfCopyFile = $"{uniquePrefixOnFileName}{fileName}";
        //     string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
        //     WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, jsonObj.ToString());
        //     return tempFilePath;
        // }

        public static string CreateTempFileWithTraAndPointGeometryModified(string schemaVersion, string fileName, string userGuidToGenerateFileNamePrefix, string traId, string pointGeometryString)
        {
            string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);

            JObject jsonObj = JObject.Parse(dtroJsonWithTraModified);
            jsonObj["data"]["source"]["provision"][0]["regulatedPlace"][0]["pointGeometry"]["point"] = pointGeometryString;

            string uniquePrefixOnFileName = $"{userGuidToGenerateFileNamePrefix.Substring(0, 5)}-";
            string nameOfCopyFile = $"{uniquePrefixOnFileName}{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, jsonObj.ToString());
            return tempFilePath;
        }
    }
}