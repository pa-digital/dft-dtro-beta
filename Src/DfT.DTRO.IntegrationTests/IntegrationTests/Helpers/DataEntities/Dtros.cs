using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.Consts;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
{
    public static class Dtros
    {
        public static void DeleteExistingDtros()
        {
            SqlQueries.TruncateTable("Dtros");
            SqlQueries.TruncateTable("DtroHistories");
        }

        public static async Task<HttpResponseMessage> CreateDtroFromFileAsync(string filePath, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "multipart/form-data" }
            };

            HttpResponseMessage dtroCreationResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}{RouteTemplates.DtrosCreateFromFile}", headers, pathToJsonFile: filePath);
            return dtroCreationResponse;
        }

        public static async Task<HttpResponseMessage> UpdateDtroFromFileAsync(string filePath, string dtroId, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "multipart/form-data" }
            };

            HttpResponseMessage updateDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Put, $"{BaseUri}{RouteTemplates.DtrosBase}/updateFromFile/{dtroId}", headers, pathToJsonFile: filePath);
            return updateDtroResponse;
        }

        public static async Task<HttpResponseMessage> CreateDtroFromJsonBodyAsync(string jsonBody, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "application/json" }
            };

            HttpResponseMessage dtroCreationResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}{RouteTemplates.DtrosCreateFromBody}", headers, jsonBody);
            return dtroCreationResponse;
        }

        public static async Task<HttpResponseMessage> UpdateDtroFromJsonBodyAsync(string jsonBody, string dtroId, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId },
                { "Content-Type", "application/json" }
            };

            HttpResponseMessage updateDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Put, $"{BaseUri}{RouteTemplates.DtrosBase}/updateFromBody/{dtroId}", headers, jsonBody);
            return updateDtroResponse;
        }

        public static async Task<HttpResponseMessage> GetDtroAsync(string dtroId, TestUser testUser)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", testUser.AppId }
            };

            HttpResponseMessage getDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Get, $"{BaseUri}{RouteTemplates.DtrosBase}/{dtroId}", headers);
            return getDtroResponse;
        }

        public static string ModifyTraIdInDtro(string schemaVersion, string jsonString, string traId)
        {
            JObject jsonObj = JObject.Parse(jsonString);
            int tradIdAsInt = int.Parse(traId);

            int schemaVersionAsInt = int.Parse(schemaVersion.Replace(".", ""));

            if (schemaVersionAsInt >= 332)
            {
                // New camel case format
                jsonObj["data"]["source"]["currentTraOwner"] = tradIdAsInt;
                jsonObj["data"]["source"]["traAffected"] = new JArray(tradIdAsInt);
                jsonObj["data"]["source"]["traCreator"] = tradIdAsInt;
            }
            else
            {
                // Old Pascal case format
                jsonObj["data"]["Source"]["currentTraOwner"] = tradIdAsInt;
                jsonObj["data"]["Source"]["traAffected"] = new JArray(tradIdAsInt);
                jsonObj["data"]["Source"]["traCreator"] = tradIdAsInt;
            }

            return jsonObj.ToString();
        }

        public static string ModifySchemaVersionInDtro(string jsonString, string schemaVersion)
        {
            JObject jsonObj = JObject.Parse(jsonString);
            jsonObj["schemaVersion"] = schemaVersion;
            return jsonObj.ToString();
        }

        public static string ModifyActionTypeAndTroNameForUpdate(string schemaVersion, string jsonString)
        {
            JObject jsonObj = JObject.Parse(jsonString);
            int schemaVersionAsInt = int.Parse(schemaVersion.Replace(".", ""));

            if (schemaVersionAsInt >= 332)
            {
                // New camel case format
                jsonObj["data"]["source"]["actionType"] = "amendment";
                jsonObj["data"]["source"]["troName"] = $"{jsonObj["data"]["source"]["troName"]} UPDATED";
            }
            else
            {
                // Old Pascal case format
                jsonObj["data"]["Source"]["actionType"] = "amendment";
                jsonObj["data"]["Source"]["troName"] = $"{jsonObj["data"]["Source"]["troName"]} UPDATED";
            }

            return jsonObj.ToString();
        }
        public static async Task<string> GetIdFromResponseJsonAsync(HttpResponseMessage httpResponseMessage)
        {
            string responseJson = await httpResponseMessage.Content.ReadAsStringAsync();
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(responseJson)!;
            string id = jsonDeserialised.id.ToString();
            return id;
        }

        public static async Task<string> GetDtroResponseJsonAsync(string dtroId, TestUser testUser)
        {
            HttpResponseMessage dtroGetResponse = await Dtros.GetDtroAsync(dtroId, testUser);
            string dtroGetResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            Assert.True(HttpStatusCode.OK == dtroGetResponse.StatusCode,
                $"Response JSON:\n\n{dtroGetResponseJson}");

            string getDtroResponseJson = await dtroGetResponse.Content.ReadAsStringAsync();
            return getDtroResponseJson;
        }

        public static string ModifySentJsonWithinFileForComparison(string schemaVersion, string filePath, string dtroId)
        {
            string jsonString = File.ReadAllText(filePath);
            string modifiedSentJson = ModifySentJsonForComparison(schemaVersion, jsonString, dtroId);
            return modifiedSentJson;
        }

        public static string ModifySentJsonForComparison(string schemaVersion, string jsonString, string dtroId)
        {
            JObject createJsonObject = JObject.Parse(jsonString);
            createJsonObject["id"] = dtroId;
            string sentCreateJsonWithId = createJsonObject.ToString();

            int schemaVersionAsInt = int.Parse(schemaVersion.Replace(".", ""));

            if (schemaVersionAsInt >= 332)
            {
                // New schema versions
                return sentCreateJsonWithId;
            }
            else
            {
                // Convert JSON relating to older schema versions to camel case
                string sentCreateJsonWithIdToCamelCase = JsonMethods.ConvertJsonKeysToCamelCase(sentCreateJsonWithId);
                return sentCreateJsonWithIdToCamelCase;
            }
        }

        public static string ModifyExternalReferenceLastUpdateDate(string jsonString, string newDate)
        {
            JObject jsonObj = JObject.Parse(jsonString);
            ModifyExternalReferenceLastUpdateDateRecursive(jsonObj, newDate);
            return jsonObj.ToString();
        }

        private static void ModifyExternalReferenceLastUpdateDateRecursive(JToken token, string newDate)
        {
            if (token is JObject obj)
            {
                foreach (var property in obj.Properties())
                {
                    if (property.Value is JArray array && (property.Name == "externalReference" || property.Name == "origin"))
                    {
                        foreach (var item in array.Children<JObject>())
                        {
                            if (item.ContainsKey("lastUpdateDate"))
                            {
                                item["lastUpdateDate"] = newDate;
                            }
                        }
                    }
                    else
                    {
                        ModifyExternalReferenceLastUpdateDateRecursive(property.Value, newDate);
                    }
                }
            }
            else if (token is JArray array)
            {
                foreach (var item in array)
                {
                    ModifyExternalReferenceLastUpdateDateRecursive(item, newDate);
                }
            }
        }

        public static string GetSchemaValidationErrorJson(string traId)
        {
            string expectedErrorJson = $$"""
            {
                "ruleError_0": {
                    "message": "Invalid type. Expected Integer but got String.",
                    "path": "source.currentTraOwner",
                    "value": "{{traId}}",
                    "errorType": "Type"
                },
                "ruleError_1": {
                    "message": "Invalid type. Expected Array but got Integer.",
                    "path": "source.traAffected",
                    "value": {{traId}},
                    "errorType": "Type"
                },
                "ruleError_2": {
                    "message": "Required properties are missing from object: actionType.",
                    "path": "source",
                    "value": [
                        "actionType"
                    ],
                    "errorType": "Required"
                },
                "ruleError_3": {
                    "message": "Property 'apples' has not been defined and the schema does not allow additional properties.",
                    "path": "apples",
                    "value": "apples",
                    "errorType": "AdditionalProperties"
                },
                "ruleError_4": {
                    "message": "JSON is valid against no schemas from 'oneOf'.",
                    "path": "",
                    "value": null,
                    "errorType": "OneOf"
                }
            }
            """;

            return expectedErrorJson;
        }

        public static string GetPointGeolocationErrorJson(string pointGeometryString)
        {
            string expectedErrorJson = $$"""
            {
                "ruleError_0": {
                    "name": "Invalid coordinates",
                    "message": "Geometry grid linked to 'PointGeometry'",
                    "path": "Source -> Provision -> RegulatedPlace -> PointGeometry -> point",
                    "rule": "Coordinates '{{pointGeometryString}}' are incorrect or not within Great Britain"
                }
            }
            """;

            return expectedErrorJson;
        }

        public static string GetLinearGeolocationErrorJson(string linearGeometryString)
        {
            string expectedErrorJson = $$"""
            {
                "ruleError_0": {
                    "name": "Invalid coordinates",
                    "message": "Geometry grid linked to 'LinearGeometry'",
                    "path": "Source -> Provision -> RegulatedPlace -> LinearGeometry -> linestring",
                    "rule": "Coordinates '{{linearGeometryString}}' are incorrect or not within Great Britain"
                }
            }
            """;

            return expectedErrorJson;
        }

        public static string GetDuplicateProvisionReferenceErrorJson(string provisionReference)
        {
            string expectedErrorJson = $$"""
            {
                "ruleError_0": {
                    "name": "'2' duplication reference",
                    "message": "Provision reference '{{provisionReference}}' is present 2 times.",
                    "path": "Source -> Provision -> reference",
                    "rule": "Each provision 'reference' must be unique and of type 'string'"
                }
            }
            """;

            return expectedErrorJson;
        }

        public static string GetExternalReferenceLastUpdateDateErrorJson(string fileName)
        {
            string geometryType = fileName.Contains("Derbyshire") ? "LinearGeometry" : "PointGeometry";
            string expectedErrorJson = $$"""
            {
                "ruleError_0": {
                    "name": "Invalid last update date",
                    "message": "Indicates the date the USRN reference was last updated",
                    "path": "Source -> Provision -> RegulatedPlace -> {{geometryType}} -> ExternalReference -> lastUpdateDate",
                    "rule": "'lastUpdateDate' cannot be in the future"
                }
            }
            """;

            return expectedErrorJson;
        }

        public static string GetJsonFromFileAndModifyTra(string schemaVersion, string fileName, string traId)
        {
            string dtroFile = $"{PathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);
            return dtroJsonWithTraModified;
        }

        public static string CreateTempFileWithTraModified(string schemaVersion, string fileName, string traId)
        {
            string dtroJsonWithTraModified = GetJsonFromFileAndModifyTra(schemaVersion, fileName, traId);
            string nameOfCopyFile = $"{traId}-{fileName}";
            string tempFilePath = $"{PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            FileHelper.WriteStringToFile(PathToDtroExamplesTempDirectory, nameOfCopyFile, dtroJsonWithTraModified);
            return tempFilePath;
        }

        public static string CreateTempFileForDtroUpdateBasedOnCreationFile(string schemaVersion, string pathOfCreateDtroFile)
        {
            string createDtroJson = File.ReadAllText(pathOfCreateDtroFile);
            string updateDtroJson = Dtros.ModifyActionTypeAndTroNameForUpdate(schemaVersion, createDtroJson);

            string directory = Path.GetDirectoryName(pathOfCreateDtroFile);
            string createDtroFileName = Path.GetFileName(pathOfCreateDtroFile);
            string updateDtroFileName = "update-" + createDtroFileName;
            string newFilePath = Path.Combine(directory, updateDtroFileName);

            FileHelper.WriteStringToFile(directory, updateDtroFileName, updateDtroJson);

            return newFilePath;
        }

        public static string CreateJsonForDtroUpdate(string schemaVersion, string fileName, string traId)
        {
            string dtroJsonWithModifiedTra = Dtros.GetJsonFromFileAndModifyTra(schemaVersion, fileName, traId);
            string updateDtroJson = Dtros.ModifyActionTypeAndTroNameForUpdate(schemaVersion, dtroJsonWithModifiedTra);
            return updateDtroJson;
        }

        public static string CreateTempFileForDtroUpdate(string schemaVersion, string fileName, string traId)
        {
            string updateDtroJson = CreateJsonForDtroUpdate(schemaVersion, fileName, traId);

            string directory = $"{TestConfig.PathToDtroExamplesTempDirectory}";
            string updateDtroFileName = $"update-{traId}-{fileName}";
            string newFilePath = Path.Combine(directory, updateDtroFileName);

            FileHelper.WriteStringToFile(directory, updateDtroFileName, updateDtroJson);

            return newFilePath;
        }

        public static string GetJsonFromFileAndModifyTraAndDuplicateProvisionReference(string schemaVersion, string fileName, string traId)
        {
            string dtroFile = $"{PathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);
            string dtroWithDuplicateProvisionReference = JsonMethods.CloneFirstItemInArrayAndAppend(dtroJsonWithTraModified, "data.source.provision");
            return dtroWithDuplicateProvisionReference;
        }

        public static string CreateTempFileWithTraModifiedAndProvisionReferenceDuplicated(string schemaVersion, string fileName, string traId)
        {
            string dtroWithDuplicateProvisionReference = GetJsonFromFileAndModifyTraAndDuplicateProvisionReference(schemaVersion, fileName, traId);
            string nameOfCopyFile = $"{traId}-{fileName}";
            string tempFilePath = $"{PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            FileHelper.WriteStringToFile(PathToDtroExamplesTempDirectory, nameOfCopyFile, dtroWithDuplicateProvisionReference);
            return tempFilePath;
        }

        public static string GetJsonAndCreateUpdateJsonWithTraModifiedAndDuplicateProvisionReference(string schemaVersion, string fileName, string traId)
        {
            string dtroWithDuplicateProvisionReference = GetJsonFromFileAndModifyTraAndDuplicateProvisionReference(schemaVersion, fileName, traId);
            string updateJson = ModifyActionTypeAndTroNameForUpdate(schemaVersion, dtroWithDuplicateProvisionReference);
            return updateJson;
        }

        public static string CreateTempUpdateFileWithTraModifiedAndProvisionReferenceDuplicated(string schemaVersion, string fileName, string traId)
        {
            string updateJson = GetJsonAndCreateUpdateJsonWithTraModifiedAndDuplicateProvisionReference(schemaVersion, fileName, traId);

            string nameOfCopyFile = $"update-{traId}-{fileName}";
            string tempFilePath = $"{PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            FileHelper.WriteStringToFile(PathToDtroExamplesTempDirectory, nameOfCopyFile, updateJson);
            return tempFilePath;
        }

        public static string GetJsonAndCreateUpdateJsonWithTraModifiedAndExternalReferenceLastUpdateDateInFuture(string schemaVersion, string fileName, string traId)
        {
            string dtroWithDuplicateProvisionReference = GetJsonFromFileAndModifyTraAndSetExternalReferenceLastUpdateDateToFuture(schemaVersion, fileName, traId);
            string updateJson = ModifyActionTypeAndTroNameForUpdate(schemaVersion, dtroWithDuplicateProvisionReference);
            return updateJson;
        }

        public static string CreateTempUpdateFileWithTraModifiedAndExternalReferenceLastUpdateDateInFuture(string schemaVersion, string fileName, string traId)
        {
            string updateJson = GetJsonAndCreateUpdateJsonWithTraModifiedAndExternalReferenceLastUpdateDateInFuture(schemaVersion, fileName, traId);

            string nameOfCopyFile = $"update-{traId}-{fileName}";
            string tempFilePath = $"{PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            FileHelper.WriteStringToFile(PathToDtroExamplesTempDirectory, nameOfCopyFile, updateJson);
            return tempFilePath;
        }

        public static string GetJsonFromFileAndModifyTraAndSetExternalReferenceLastUpdateDateToFuture(string schemaVersion, string fileName, string traId)
        {
            string dtroFile = $"{PathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);

            DateTime dateTomorrow = DateTime.Now.AddDays(1);
            string dateTomorrowFormatted = dateTomorrow.ToString("yyyy-MM-ddTHH:00:00");
            string dtroJsonWithFutureExternalReferenceLastUpdateDate = ModifyExternalReferenceLastUpdateDate(dtroJsonWithTraModified, dateTomorrowFormatted);
            return dtroJsonWithFutureExternalReferenceLastUpdateDate;
        }

        public static string CreateTempFileWithTraModifiedAndExternalReferenceLastUpdateDateInFuture(string schemaVersion, string fileName, string traId)
        {
            string dtroJsonWithFutureExternalReferenceLastUpdateDate = GetJsonFromFileAndModifyTraAndSetExternalReferenceLastUpdateDateToFuture(schemaVersion, fileName, traId);
            string nameOfCopyFile = $"{traId}-{fileName}";
            string tempFilePath = $"{PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            FileHelper.WriteStringToFile(PathToDtroExamplesTempDirectory, nameOfCopyFile, dtroJsonWithFutureExternalReferenceLastUpdateDate);
            return tempFilePath;
        }

        public static string GetJsonFromFileAndModifyTraAndPointGeometry(string schemaVersion, string fileName, string traId, string pointGeometryString)
        {
            string dtroFile = $"{PathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);

            JObject jsonObj = JObject.Parse(dtroJsonWithTraModified);
            jsonObj["data"]["source"]["provision"][0]["regulatedPlace"][0]["pointGeometry"]["point"] = pointGeometryString;

            return jsonObj.ToString();
        }

        public static string CreateTempFileWithTraAndPointGeometryModified(string schemaVersion, string fileName, string traId, string pointGeometryString)
        {
            string jsonWithTraAndPointGeometryModified = GetJsonFromFileAndModifyTraAndPointGeometry(schemaVersion, fileName, traId, pointGeometryString);
            string nameOfCopyFile = $"{traId}-{fileName}";
            string tempFilePath = $"{PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            FileHelper.WriteStringToFile(PathToDtroExamplesTempDirectory, nameOfCopyFile, jsonWithTraAndPointGeometryModified);
            return tempFilePath;
        }

        public static string GetJsonFromFileAndModifyTraAndLinearGeometry(string schemaVersion, string fileName, string traId, string linearGeometryString)
        {
            string dtroFile = $"{PathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);

            JObject jsonObj = JObject.Parse(dtroJsonWithTraModified);
            jsonObj["data"]["source"]["provision"][0]["regulatedPlace"][0]["linearGeometry"]["linestring"] = linearGeometryString;

            return jsonObj.ToString();
        }

        public static string CreateTempFileWithTraAndLinearGeometryModified(string schemaVersion, string fileName, string traId, string linearGeometryString)
        {
            string jsonWithTraAndPointGeometryModified = GetJsonFromFileAndModifyTraAndLinearGeometry(schemaVersion, fileName, traId, linearGeometryString);
            string nameOfCopyFile = $"{traId}-{fileName}";
            string tempFilePath = $"{PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            FileHelper.WriteStringToFile(PathToDtroExamplesTempDirectory, nameOfCopyFile, jsonWithTraAndPointGeometryModified);
            return tempFilePath;
        }

        public static string GetJsonFromFileAndModifyTraAndSchemaVersion(string schemaVersionToTest, string schemaVersionOfFilesToUse, string fileName, string traId)
        {
            string createDtroFile = $"{PathToExamplesDirectory}/D-TROs/{schemaVersionOfFilesToUse}/{fileName}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.ModifyTraIdInDtro(schemaVersionOfFilesToUse, createDtroJson, traId);
            string createDtroJsonWithSchemaVersionUpdated = Dtros.ModifySchemaVersionInDtro(createDtroJsonWithTraUpdated, schemaVersionToTest);
            return createDtroJsonWithSchemaVersionUpdated;
        }

        public static string GetJsonFromFileAndModifyToFailSchemaValidation(string schemaVersion, string fileName, string traId)
        {
            string dtroFile = $"{PathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
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
            string tempFilePath = $"{PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            FileHelper.WriteStringToFile(PathToDtroExamplesTempDirectory, nameOfCopyFile, dtroJsonWithTraModified);
            return tempFilePath;
        }

        public static string CreateTempFileWithTraAndSchemaVersionModified(string schemaVersionToTest, string schemaVersionOfFilesToUse, string fileName, string traId)
        {
            string createDtroJsonWithSchemaVersionUpdated = GetJsonFromFileAndModifyTraAndSchemaVersion(schemaVersionToTest, schemaVersionOfFilesToUse, fileName, traId);
            string nameOfCopyFile = $"{traId}-{fileName}";
            string tempFilePath = $"{PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            FileHelper.WriteStringToFile(PathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithSchemaVersionUpdated);
            return tempFilePath;
        }
    }
}