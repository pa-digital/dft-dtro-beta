using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelper;
using Newtonsoft.Json;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.CreateDtroTests.Schema_3_3_1
{
    public class InvalidCamelCase : BaseTest
    {
        readonly static string schemaVersionToTest = "3.3.1";
        readonly static string filesWithInvalidCamelCase = "3.3.2";

        public static IEnumerable<object[]> GetDtroFileNames()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{AbsolutePathToDtroExamplesDirectory}/{filesWithInvalidCamelCase}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromFileWithCamelCaseShouldBeRejected(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
            string userGuid = await GetIdFromResponseJsonAsync(createUserResponse);

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{filesWithInvalidCamelCase}/{fileName}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(filesWithInvalidCamelCase, createDtroJson, publisher.TraId);
            string createDtroJsonWithSchemaVersionUpdated = Dtros.UpdateSchemaVersionInDtro(createDtroJsonWithTraUpdated, schemaVersionToTest);
            string nameOfCopyFile = $"{userGuid.Substring(0, 5)}{fileName}";
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithSchemaVersionUpdated);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            Assert.Equal(HttpStatusCode.BadRequest, createDtroResponse.StatusCode);

            // Check DTRO response JSON
            string dtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(dtroResponseJson)!;
            string responseMessage = jsonDeserialised.message.ToString();
            string responseError = jsonDeserialised.error.ToString();
            Assert.Equal(responseMessage, "Case naming convention exception");
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", responseError);
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromJsonBodyWithCamelCaseShouldBeRejected(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{filesWithInvalidCamelCase}/{fileName}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(filesWithInvalidCamelCase, createDtroJson, publisher.TraId);
            string createDtroJsonWithSchemaVersionUpdated = Dtros.UpdateSchemaVersionInDtro(createDtroJsonWithTraUpdated, schemaVersionToTest);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithSchemaVersionUpdated, publisher);
            Assert.Equal(HttpStatusCode.BadRequest, createDtroResponse.StatusCode);

            // Check DTRO response JSON
            string dtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(dtroResponseJson)!;
            string responseMessage = jsonDeserialised.message.ToString();
            string responseError = jsonDeserialised.error.ToString();
            Assert.Equal(responseMessage, "Case naming convention exception");
            Assert.StartsWith("All property names must conform to pascal case naming conventions. The following properties violate this: [source, provision, regulatedPlace", responseError);
        }
    }
}