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

    public static string ModifySchemaVersion(this string jsonString, string schemaVersion)
    {
        string updatedJson = Dtros.ModifySchemaVersionInDtro(jsonString, schemaVersion);
        return updatedJson;
    }

    public static string ModifyToFailSchemaValidation(this string jsonString)
    {
        var jsonObj = JObject.Parse(jsonString);

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
        Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        headers.Add("Content-Type", "application/json");

        await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Tra, appId);

        HttpResponseMessage dtroCreationResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}{RouteTemplates.DtrosCreateFromBody}", headers, jsonString);
        return dtroCreationResponse;
    }

    public static async Task<HttpResponseMessage> SendJsonInDtroUpdateRequestAsync(this string jsonString, string dtroId, String appId)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        headers.Add("Content-Type", "application/json");

        await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Tra, appId);

        HttpResponseMessage updateDtroResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Put, $"{TestConfig.BaseUri}{RouteTemplates.DtrosBase}/updateFromBody/{dtroId}", headers, jsonString);
        return updateDtroResponse;
    }

    public static async Task<HttpResponseMessage> SendFileInDtroCreationRequestAsync(this string dtroFilePath, String appId)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        headers.Add("Content-Type", "multipart/form-data");

        await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Tra, appId);

        HttpResponseMessage dtroCreationResponse = await HttpRequestHelper.MakeHttpRequestAsync(HttpMethod.Post, $"{TestConfig.BaseUri}{RouteTemplates.DtrosCreateFromFile}", headers, pathToJsonFile: dtroFilePath);
        return dtroCreationResponse;
    }

    public static async Task<HttpResponseMessage> SendFileInDtroUpdateRequestAsync(this string dtroFilePath, string dtroId, String appId)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        headers.Add("Content-Type", "multipart/form-data");

        await headers.AddValidHeadersForEnvironmentAsync(UserGroup.Tra, appId);

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

    public static string CreateDtroTempFile(this string jsonString, string fileName, TestUser testUser)
    {
        // If the name is null, we're using a pre-existing user on dev / test / integration, so we have to create a unique file name prefix rather than use the TRA ID as a prefix
        string uniquePrefix = testUser.Name == null ? Guid.NewGuid().ToString("N").Substring(0, 6) : testUser.TraId;
        string nameOfCopyFile = $"{uniquePrefix}-{fileName}";
        string tempFilePath = $"{TestConfig.PathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
        FileHelper.WriteStringToFile(TestConfig.PathToDtroExamplesTempDirectory, nameOfCopyFile, jsonString);
        return tempFilePath;
    }

    public static string CreateDtroTempFileForUpdate(this string jsonString, string fileName, TestUser testUser)
    {
        // If the name is null, we're using a pre-existing user on dev / test / integration, so we have to create a unique file name prefix rather than use the TRA ID as a prefix
        string uniquePrefix = testUser.Name == null ? Guid.NewGuid().ToString("N").Substring(0, 6) : testUser.TraId;
        string nameOfCopyFile = $"update-{uniquePrefix}-{fileName}";
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

    public static string ConvertJsonKeysToCamelCase(this string json)
    {
        JObject jsonObject = JObject.Parse(json);
        JObject camelCasedObject = JsonMethods.ConvertKeysToCamelCase(jsonObject);
        return JsonConvert.SerializeObject(camelCasedObject, Formatting.Indented);
    }
}