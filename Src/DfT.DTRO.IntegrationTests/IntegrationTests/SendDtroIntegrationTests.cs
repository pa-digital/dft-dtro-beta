using System.Collections.Generic;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities;

namespace DfT.DTRO.IntegrationTests.IntegrationTests
{
    public class SendDtroIntegrationTests : BaseTest
    {
        public static IEnumerable<object[]> GetFileNames()
        {
            var directoryPath = new DirectoryInfo(TestConfig.AbsolutePathToDtroExamplesFolder);
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetFileNames))]
        public async Task CreateDtroShouldBeSavedCorrectly(string filePath)
        {
            var userResponse = await DtroUsers.CreateUserAsync(User.Publisher);
            Assert.Equal(HttpStatusCode.Created, userResponse.StatusCode);

            var dtroResponse = await Dtros.CreateDtroAsync(User.Publisher, filePath);
            Assert.Equal(HttpStatusCode.Created, dtroResponse.StatusCode);
        }
    }
}