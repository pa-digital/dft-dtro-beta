using Newtonsoft.Json.Linq;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelper;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests;

[ExcludeFromCodeCoverage]
public class SendDtroIntegrationTests : BaseTest
{
    public static IEnumerable<object[]> GetFileNames()
    {
        DirectoryInfo directoryPath = new DirectoryInfo(AbsolutePathToDtroExamplesDirectory);
        FileInfo[] files = directoryPath.GetFiles();

        foreach (FileInfo file in files)
        {
            yield return new object[] { file.Name };
        }
    }

    [Theory]
    [MemberData(nameof(GetFileNames))]
    public async Task DtroSubmittedFromFileShouldBeSavedCorrectly(string fileName)
    {
        Console.WriteLine($"\nTesting with file {fileName}...");
        TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
        HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
        Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

        string createDtroFile = $"{AbsolutePathToDtroExamplesDirectory}/{fileName}";
        string createDtroJson = File.ReadAllText(createDtroFile);
        string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(createDtroJson, publisher.TraId);
        string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{fileName}";
        WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, fileName, createDtroJsonWithTraUpdated);

        HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
        Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

        string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);
        HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
        Assert.Equal(HttpStatusCode.OK, getDtroResponse.StatusCode);
        string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();

        JObject createJsonObject = JObject.Parse(createDtroJsonWithTraUpdated);
        createJsonObject["id"] = dtroId;

        string sentCreateJsonWithIdForComparison = createJsonObject.ToString();
        CompareJson(sentCreateJsonWithIdForComparison, dtroResponseJson);
    }

    [Theory]
    [MemberData(nameof(GetFileNames))]
    public async Task DtroSubmittedFromJsonBodyShouldBeSavedCorrectly(string fileName)
    {
        Console.WriteLine($"\nTesting with file {fileName}...");
        TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
        HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
        Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

        string createDtroFile = $"{AbsolutePathToDtroExamplesDirectory}/{fileName}";
        string createDtroJson = File.ReadAllText(createDtroFile);
        string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(createDtroJson, publisher.TraId);

        HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraUpdated, publisher);
        Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

        string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);
        HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
        Assert.Equal(HttpStatusCode.OK, getDtroResponse.StatusCode);
        string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();

        JObject createJsonObject = JObject.Parse(createDtroJsonWithTraUpdated);
        createJsonObject["id"] = dtroId;

        string sentCreateJsonWithIdForComparison = createJsonObject.ToString();
        CompareJson(sentCreateJsonWithIdForComparison, dtroResponseJson);
    }
}