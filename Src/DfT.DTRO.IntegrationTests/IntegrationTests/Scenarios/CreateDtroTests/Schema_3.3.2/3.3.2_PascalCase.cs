using System.Collections.Generic;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
using Newtonsoft.Json;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.CreateDtroTests.Schema_3_3_2
{
    public class PascalCase : BaseTest
    {
        readonly static string schemaVersionToTest = "3.3.2";
        readonly static string filesWithInvalidPascalCase = "3.3.1";

        public static IEnumerable<object[]> GetDtroFileNames()
        {
            DirectoryInfo directoryPath = new DirectoryInfo($"{AbsolutePathToDtroExamplesDirectory}/{filesWithInvalidPascalCase}");
            FileInfo[] files = directoryPath.GetFiles();

            foreach (FileInfo file in files)
            {
                yield return new object[] { file.Name };
            }
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromFileShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{filesWithInvalidPascalCase}/{fileName}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(filesWithInvalidPascalCase, createDtroJson, publisher.TraId);
            string createDtroJsonWithSchemaVersionUpdated = Dtros.UpdateSchemaVersionInDtro(createDtroJsonWithTraUpdated, schemaVersionToTest);
            string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{fileName}";
            WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, fileName, createDtroJsonWithSchemaVersionUpdated);

            // Send DTRO
            HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
            Assert.Equal(HttpStatusCode.BadRequest, createDtroResponse.StatusCode);

            // Check DTRO response JSON
            string dtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(dtroResponseJson)!;
            string responseMessage = jsonDeserialised.message.ToString();
            string responseError = jsonDeserialised.error.ToString();
            Assert.Equal(responseMessage, "Case naming convention exception");
            Assert.StartsWith("All property names must conform to camel case naming conventions. The following properties violate this: [Source, Provision, RegulatedPlace", responseError);
        }

        [Theory]
        [MemberData(nameof(GetDtroFileNames))]
        public async Task DtroSubmittedFromJsonBodyShouldBeSavedCorrectly(string fileName)
        {
            Console.WriteLine($"\nTesting with file {fileName}...");

            // Generate user to send DTRO and read it back
            TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
            HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
            Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

            // Prepare DTRO
            string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{filesWithInvalidPascalCase}/{fileName}";
            string createDtroJson = File.ReadAllText(createDtroFile);
            string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(filesWithInvalidPascalCase, createDtroJson, publisher.TraId);
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
            Assert.StartsWith("All property names must conform to camel case naming conventions. The following properties violate this: [Source, Provision, RegulatedPlace", responseError);
        }
    }
}