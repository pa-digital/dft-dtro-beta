using System.Collections.Generic;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

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
        public async Task CreateDtroShouldBeSavedCorrectly(string filePath)
        {
            var createUserResponse = await DtroUsers.CreateUserAsync(User.Publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            var createDtroResponse = await Dtros.CreateDtroAsync(User.Publisher, filePath);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);
        }
    }
}