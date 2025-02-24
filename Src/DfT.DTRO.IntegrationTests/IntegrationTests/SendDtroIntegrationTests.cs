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
            var directoryPath = new DirectoryInfo(AbsolutePathToDtroExamplesDirectory);
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetFileNames))]
        public async Task CreateDtroShouldBeSavedCorrectly(string fileName)
        {
            var publisher = TestUsers.GenerateUser(UserGroup.Tra);
            var createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            var createDtroResponse = await Dtros.CreateDtroAsync(fileName, publisher);
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
    }
}