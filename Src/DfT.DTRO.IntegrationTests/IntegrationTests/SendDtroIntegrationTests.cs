using System.Collections.Generic;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelper;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.IntegrationTests.IntegrationTests
{
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
            var publisher = TestUsers.GenerateUser(UserGroup.Tra);
            var createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            var createDtroResponse = await Dtros.CreateDtroFromFileAsync(fileName, publisher);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

            string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);

            var getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
            Assert.Equal(HttpStatusCode.OK, getDtroResponse.StatusCode);
            var dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();

            var sentCreateDtroFile = $"{AbsolutePathToDtroExamplesTempDirectory}/{fileName}";
            var sentCreateDtroJson = File.ReadAllText(sentCreateDtroFile);

            JObject createJsonObject = JObject.Parse(sentCreateDtroJson);
            createJsonObject["id"] = dtroId;

            var sentCreateJsonWithIdForComparison = createJsonObject.ToString();
            CompareJson(sentCreateJsonWithIdForComparison, dtroResponseJson);
        }

        [Theory]
        [MemberData(nameof(GetFileNames))]
        public async Task DtroSubmittedFromJsonBodyShouldBeSavedCorrectly(string fileName)
        {
            var publisher = TestUsers.GenerateUser(UserGroup.Tra);
            var createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            var createDtroFile = $"{AbsolutePathToDtroExamplesTempDirectory}/{fileName}";
            var createDtroJson = File.ReadAllText(createDtroFile);
            var createDtroJsonWithTraUpdated = Dtros.UpdateTraId(createDtroJson, publisher.TraId);

            var createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraUpdated, publisher);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

            string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);

            var getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
            Assert.Equal(HttpStatusCode.OK, getDtroResponse.StatusCode);
            var dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();

            JObject createJsonObject = JObject.Parse(createDtroJsonWithTraUpdated);
            createJsonObject["id"] = dtroId;

            var sentCreateJsonWithIdForComparison = createJsonObject.ToString();
            CompareJson(sentCreateJsonWithIdForComparison, dtroResponseJson);
        }
    }
}