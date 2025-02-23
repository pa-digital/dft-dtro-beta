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
            var publisher = TestUsers.GenerateUser(UserGroup.Tra);
            var createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            var createDtroResponse = await Dtros.CreateDtroAsync(filePath, publisher);
            Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);
        }
    }
}