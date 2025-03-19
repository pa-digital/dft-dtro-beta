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

        public static string CreateTempFileWithTraModified(string schemaVersion, string fileName, string traId)
        {
            string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);
            string nameOfCopyFile = $"{traId}-{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, dtroJsonWithTraModified);
            return tempFilePath;
        }

        public static string CreateTempFileWithTraModifiedAndProvisionReferenceDuplicated(string schemaVersion, string fileName, string traId)
        {
            string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);
            string dtroWithDuplicateProvisionReference = JsonMethods.CloneFirstItemInArrayAndAppend(dtroJsonWithTraModified, "data.source.provision");
            string nameOfCopyFile = $"{traId}-{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, dtroWithDuplicateProvisionReference);
            return tempFilePath;
        }

        public static string CreateTempFileWithTraModifiedAndExternalReferenceLastUpdateDateInFuture(string schemaVersion, string fileName, string userGuidToGenerateFileNamePrefix, string traId)
        {
            string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);

            DateTime dateTomorrow = DateTime.Now.AddDays(1);
            string dateTomorrowFormatted = dateTomorrow.ToString("yyyy-MM-ddTHH:00:00");
            string dtroJsonWithFutureExternalReferenceLastUpdateDate = JsonMethods.ModifyExternalReferenceLastUpdateDate(dtroJsonWithTraModified, dateTomorrowFormatted);

            string uniquePrefixOnFileName = $"{userGuidToGenerateFileNamePrefix.Substring(0, 5)}-";
            string nameOfCopyFile = $"{uniquePrefixOnFileName}{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, dtroJsonWithFutureExternalReferenceLastUpdateDate);
            return tempFilePath;
        }

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

        public static string GetJsonFromFileAndModifyToFailSchemaValidation(string schemaVersion, string fileName, string traId)
        {
            string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);

            var jsonObj = JObject.Parse(dtroJsonWithTraModified);

            // Remove "data.source.actionType"
            jsonObj.SelectToken("data.source.actionType")?.Parent.Remove();

            // Convert "data.source.currentTraOwner" to a string by wrapping it in quotes
            var currentTraOwner = jsonObj.SelectToken("data.source.currentTraOwner");
            if (currentTraOwner != null && currentTraOwner.Type == JTokenType.Integer)
            {
                jsonObj["data"]["source"]["currentTraOwner"] = currentTraOwner.ToString();
            }

            // Extract first element from "data.source.traAffected" array and replace "traAffected" key with that value
            var traAffectedArray = jsonObj.SelectToken("data.source.traAffected") as JArray;
            if (traAffectedArray != null && traAffectedArray.Count > 0)
            {
                jsonObj["data"]["source"]["traAffected"] = traAffectedArray[0];
            }

            // Add undefined new field
            jsonObj["data"]["apples"] = "bananas";

            return jsonObj.ToString();
        }

        public static string CreateTempFileToFailSchemaValidation(string schemaVersion, string fileName, string traId)
        {
            string dtroJsonWithTraModified = GetJsonFromFileAndModifyToFailSchemaValidation(schemaVersion, fileName, traId);

            string nameOfCopyFile = $"{traId}-{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, dtroJsonWithTraModified);
            return tempFilePath;
        }

        public static string GetJsonFromFileAndModifyTraAndSchemaVersion(string schemaVersionToTest, string schemaVersionOfFilesToUse, string fileName, string traId)
        {
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersionOfFilesToUse}/{fileName}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.ModifyTraIdInDtro(schemaVersionOfFilesToUse, createDtroJson, traId);
            string createDtroJsonWithSchemaVersionUpdated = Dtros.ModifySchemaVersionInDtro(createDtroJsonWithTraUpdated, schemaVersionToTest);
            return createDtroJsonWithSchemaVersionUpdated;
            // string nameOfCopyFile = $"{traId}-{fileName}";
            // string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            // FileHelper.WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithSchemaVersionUpdated);
            // return tempFilePath;
        }

        public static string CreateTempFileWithTraAndSchemaVersionModified(string schemaVersionToTest, string schemaVersionOfFilesToUse, string fileName, string traId)
        {
            // string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersionOfFilesToUse}/{fileName}";
            // string createDtroJson = File.ReadAllText(createDtroFile);
            // string createDtroJsonWithTraUpdated = Dtros.ModifyTraIdInDtro(schemaVersionOfFilesToUse, createDtroJson, traId);
            // string createDtroJsonWithSchemaVersionUpdated = Dtros.ModifySchemaVersionInDtro(createDtroJsonWithTraUpdated, schemaVersionToTest);

            string createDtroJsonWithSchemaVersionUpdated = GetJsonFromFileAndModifyTraAndSchemaVersion(schemaVersionToTest, schemaVersionOfFilesToUse, fileName, traId);
            string nameOfCopyFile = $"{traId}-{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            FileHelper.WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithSchemaVersionUpdated);
            return tempFilePath;
        }
    }
}