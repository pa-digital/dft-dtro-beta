using DfT.DTRO.Consts;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            HttpResponseMessage createDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}{RouteTemplates.DtrosCreateFromFile}", headers, pathToJsonFile: filePath);
            return createDtroResponse;
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

            HttpResponseMessage createDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{BaseUri}{RouteTemplates.DtrosCreateFromBody}", headers, jsonBody);
            return createDtroResponse;
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
            HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, testUser);
            Assert.Equal(HttpStatusCode.OK, getDtroResponse.StatusCode);
            string getDtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();
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

        public static string GetJsonFromFileAndModifyTra(string schemaVersion, string fileName, string traId)
        {
            string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);
            return dtroJsonWithTraModified;
        }

        public static string GetJsonFromFileAndModifyTraAndDuplicateProvisionReference(string schemaVersion, string fileName, string traId)
        {
            string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);
            string dtroWithDuplicateProvisionReference = JsonMethods.CloneFirstItemInArrayAndAppend(dtroJsonWithTraModified, "data.source.provision");
            return dtroWithDuplicateProvisionReference;
        }

        public static string GetJsonFromFileAndModifyTraAndSetExternalReferenceLastUpdateDateToFuture(string schemaVersion, string fileName, string traId)
        {
            string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);

            DateTime dateTomorrow = DateTime.Now.AddDays(1);
            string dateTomorrowFormatted = dateTomorrow.ToString("yyyy-MM-ddTHH:00:00");
            string dtroJsonWithFutureExternalReferenceLastUpdateDate = ModifyExternalReferenceLastUpdateDate(dtroJsonWithTraModified, dateTomorrowFormatted);
            return dtroJsonWithFutureExternalReferenceLastUpdateDate;
        }

        public static string GetJsonFromFileAndModifyTraAndPointGeometry(string schemaVersion, string fileName, string traId, string pointGeometryString)
        {
            string dtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
            string dtroJson = File.ReadAllText(dtroFile);
            string dtroJsonWithTraModified = Dtros.ModifyTraIdInDtro(schemaVersion, dtroJson, traId);

            JObject jsonObj = JObject.Parse(dtroJsonWithTraModified);
            jsonObj["data"]["source"]["provision"][0]["regulatedPlace"][0]["pointGeometry"]["point"] = pointGeometryString;

            return jsonObj.ToString();
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
            string expectedErrorJson = """
            {
                "ruleError_0": {
                    "message": "Invalid type. Expected Integer but got String.",
                    "path": "source.currentTraOwner",
                    "value": "993344436",
                    "errorType": "Type"
                },
                "ruleError_1": {
                    "message": "Invalid type. Expected Array but got Integer.",
                    "path": "source.traAffected",
                    "value": 993344436,
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
            """.Replace("993344436", traId);

            return expectedErrorJson;
        }
    }
}