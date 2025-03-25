using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DfT.DTRO.Consts;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Extensions;

public static class DtroExtensions
{
    public static string GetJsonFromFile(this string fileName, string schemaVersion)
    {
        string dtroFile = $"{TestConfig.PathToExamplesDirectory}/D-TROs/{schemaVersion}/{fileName}";
        string dtroJson = File.ReadAllText(dtroFile);
        return dtroJson;
    }

    public static string ModifyTraInDtroJson(this string jsonString, string schemaVersion, string traId)
    {
        JObject jsonObj = JObject.Parse(jsonString);
        int traIdAsInt = int.Parse(traId);

        int schemaVersionAsInt = int.Parse(schemaVersion.Replace(".", ""));

        string sourceCapitalisation = schemaVersionAsInt >= 332 ? "source" : "Source";

        jsonObj["data"][sourceCapitalisation]["currentTraOwner"] = traIdAsInt;
        jsonObj["data"][sourceCapitalisation]["traAffected"] = new JArray(traIdAsInt);
        jsonObj["data"][sourceCapitalisation]["traCreator"] = traIdAsInt;

        return jsonObj.ToString();
    }

    public static string ModifySourceActionType(this string jsonString, string schemaVersion, string actionType)
    {
        JObject jsonObj = JObject.Parse(jsonString);
        int schemaVersionAsInt = int.Parse(schemaVersion.Replace(".", ""));

        string sourceCapitalisation = schemaVersionAsInt >= 332 ? "source" : "Source";
        jsonObj["data"][sourceCapitalisation]["actionType"] = actionType;

        return jsonObj.ToString();
    }

    public static string ModifyTroNameForUpdate(this string jsonString, string schemaVersion)
    {
        JObject jsonObj = JObject.Parse(jsonString);
        int schemaVersionAsInt = int.Parse(schemaVersion.Replace(".", ""));

        string sourceCapitalisation = schemaVersionAsInt >= 332 ? "source" : "Source";

        jsonObj["data"][sourceCapitalisation]["troName"] = $"{jsonObj["data"][sourceCapitalisation]["troName"]} UPDATED";

        return jsonObj.ToString();
    }

    public static async Task<HttpResponseMessage> SendJsonInDtroCreationRequestAsync(this string jsonString, String appId)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", appId },
                { "Content-Type", "application/json" }
            };

        HttpResponseMessage dtroCreationResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}{RouteTemplates.DtrosCreateFromBody}", headers, jsonString);
        return dtroCreationResponse;
    }

    public static async Task<HttpResponseMessage> SendJsonInDtroUpdateRequestAsync(this string jsonString, string dtroId, String appId)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", appId },
                { "Content-Type", "application/json" }
            };

        HttpResponseMessage updateDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Put, $"{TestConfig.BaseUri}{RouteTemplates.DtrosBase}/updateFromBody/{dtroId}", headers, jsonString);
        return updateDtroResponse;
    }

    public static async Task<HttpResponseMessage> SendFileInDtroCreationRequestAsync(this string dtroFilePath, String appId)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", appId },
                { "Content-Type", "multipart/form-data" }
            };

        HttpResponseMessage dtroCreationResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}{RouteTemplates.DtrosCreateFromFile}", headers, pathToJsonFile: dtroFilePath);
        return dtroCreationResponse;
    }

    public static async Task<HttpResponseMessage> SendFileInDtroUpdateRequestAsync(this string dtroFilePath, string dtroId, String appId)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "x-App-Id", appId },
                { "Content-Type", "multipart/form-data" }
            };

        HttpResponseMessage dtroUpdateResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Put, $"{TestConfig.BaseUri}{RouteTemplates.DtrosBase}/updateFromFile/{dtroId}", headers, pathToJsonFile: dtroFilePath);
        return dtroUpdateResponse;
    }

    public static async Task<String> GetIdFromResponseJsonAsync(this HttpResponseMessage httpResponseMessage)
    {
        string responseJson = await httpResponseMessage.Content.ReadAsStringAsync();
        dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(responseJson)!;
        string id = jsonDeserialised.id.ToString();
        return id;
    }

    public static async Task<HttpResponseMessage> GetDtroResponseByIdAsync(this string dtroId, TestUser testUser)
    {
        HttpResponseMessage dtroGetResponse = await Dtros.GetDtroAsync(dtroId, testUser);
        return dtroGetResponse;
    }

    public static string AddDtroIdToJson(this string jsonString, string dtroId)
    {
        JObject jsonCreationObject = JObject.Parse(jsonString);
        jsonCreationObject["id"] = dtroId;
        string sentCreateJsonWithId = jsonCreationObject.ToString();
        return sentCreateJsonWithId;
    }

    public static string CreateDtroTempFile(this string jsonString, string fileName, string traId)
    {
        string nameOfCopyFile = $"{traId}-{fileName}";
        string tempFilePath = $"{TestConfig.PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
        FileHelper.WriteStringToFile(TestConfig.PathToDtroExamplesTempDirectory, nameOfCopyFile, jsonString);
        return tempFilePath;
    }

    public static string CreateDtroTempFileForUpdate(this string jsonString, string fileName, string traId)
    {
        string nameOfCopyFile = $"update-{traId}-{fileName}";
        string tempFilePath = $"{TestConfig.PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
        FileHelper.WriteStringToFile(TestConfig.PathToDtroExamplesTempDirectory, nameOfCopyFile, jsonString);
        return tempFilePath;
    }

    public static string DuplicateProvisionReferenceInDtro(this string jsonString)
    {
        string dtroWithDuplicateProvisionReference = JsonMethods.CloneFirstItemInArrayAndAppend(jsonString, "data.source.provision");
        return dtroWithDuplicateProvisionReference;
    }

    public static string SetExternalReferenceLastUpdatedDateInFuture(this string jsonString)
    {
        DateTime dateTomorrow = DateTime.Now.AddDays(1);
        string dateTomorrowFormatted = dateTomorrow.ToString("yyyy-MM-ddTHH:00:00");
        string dtroJsonWithFutureExternalReferenceLastUpdateDate = Dtros.ModifyExternalReferenceLastUpdateDate(jsonString, dateTomorrowFormatted);
        return dtroJsonWithFutureExternalReferenceLastUpdateDate;
    }

    public static string ModifyPointGeometry(this string jsonString, string pointGeometryString)
    {
        string updatedPointGeometry = JsonMethods.SetValueAtJsonPath(jsonString, "data.source.provision[0].regulatedPlace[0].pointGeometry.point", pointGeometryString);
        return updatedPointGeometry;
    }

    public static string ModifyLinearGeometry(this string jsonString, string linearGeometryString)
    {
        string updatedLinearGeometry = JsonMethods.SetValueAtJsonPath(jsonString, "data.source.provision[0].regulatedPlace[0].linearGeometry.linestring", linearGeometryString);
        return updatedLinearGeometry;
    }
}