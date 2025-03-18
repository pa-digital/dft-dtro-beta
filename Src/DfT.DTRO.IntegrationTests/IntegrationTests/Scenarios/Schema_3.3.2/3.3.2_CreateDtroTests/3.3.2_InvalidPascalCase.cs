// using Newtonsoft.Json;
// using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
// using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
// using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;
// using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers.ErrorJsonResponseProcessor;

// namespace DfT.DTRO.IntegrationTests.IntegrationTests.Schema_3_3_2.CreateDtroTests
// {
//     public class InvalidPascalCase : BaseTest
//     {
//         readonly static string schemaVersionToTest = "3.3.2";
//         readonly static string filesWithInvalidPascalCase = "3.3.1";

//         public static IEnumerable<object[]> GetDtroFileNames()
//         {
//             DirectoryInfo directoryPath = new DirectoryInfo($"{AbsolutePathToDtroExamplesDirectory}/{filesWithInvalidPascalCase}");
//             FileInfo[] files = directoryPath.GetFiles();

//             foreach (FileInfo file in files)
//             {
//                 yield return new object[] { file.Name };
//             }
//         }

//         [Theory]
//         [MemberData(nameof(GetDtroFileNames))]
//         public async Task DtroSubmittedFromFileWithPascalCaseShouldBeRejected(string fileName)
//         {
//             Console.WriteLine($"\nTesting with file {fileName}...");

//             // Generate user to send DTRO and read it back
//             TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
//             HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
//             Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
//             string userGuidToGenerateFileNamePrefix = await JsonMethods.GetIdFromResponseJsonAsync(createUserResponse);
//             // Avoid files being overwritten by using a unique prefix in file names for each test
//             string uniquePrefixOnFileName = $"{userGuidToGenerateFileNamePrefix.Substring(0, 5)}-";

//             // Prepare DTRO
//             string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{filesWithInvalidPascalCase}/{fileName}";
//             string createDtroJson = File.ReadAllText(createDtroFile);
//             string createDtroJsonWithTraUpdated = Dtros.ModifyTraIdInDtro(filesWithInvalidPascalCase, createDtroJson, publisher.TraId);
//             string createDtroJsonWithSchemaVersionUpdated = Dtros.ModifySchemaVersionInDtro(createDtroJsonWithTraUpdated, schemaVersionToTest);
//             string nameOfCopyFile = $"{uniquePrefixOnFileName}{fileName}";
//             string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
//             WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithSchemaVersionUpdated);

//             // Send DTRO
//             HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
//             Assert.Equal(HttpStatusCode.BadRequest, createDtroResponse.StatusCode);

//             // Check DTRO response JSON
//             ErrorJson jsonErrorResponse = await ErrorJsonResponseProcessor.GetErrorJson(createDtroResponse);
//             Assert.Equal("Case naming convention exception", jsonErrorResponse.Message);
//             Assert.StartsWith("All property names must conform to camel case naming conventions. The following properties violate this: [Source, Provision, RegulatedPlace", jsonErrorResponse.Error);
//         }

//         [Theory]
//         [MemberData(nameof(GetDtroFileNames))]
//         public async Task DtroSubmittedFromJsonBodyWithPascalCaseShouldBeRejected(string fileName)
//         {
//             Console.WriteLine($"\nTesting with file {fileName}...");

//             // Generate user to send DTRO and read it back
//             TestUser publisher = TestUsers.GenerateUserDetails(UserGroup.Tra);
//             HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
//             Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

//             // Prepare DTRO
//             string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{filesWithInvalidPascalCase}/{fileName}";
//             string createDtroJson = File.ReadAllText(createDtroFile);
//             string createDtroJsonWithTraUpdated = Dtros.ModifyTraIdInDtro(filesWithInvalidPascalCase, createDtroJson, publisher.TraId);
//             string createDtroJsonWithSchemaVersionUpdated = Dtros.ModifySchemaVersionInDtro(createDtroJsonWithTraUpdated, schemaVersionToTest);

//             // Send DTRO
//             HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithSchemaVersionUpdated, publisher);
//             Assert.Equal(HttpStatusCode.BadRequest, createDtroResponse.StatusCode);

//             // Check DTRO response JSON
//             ErrorJson jsonErrorResponse = await ErrorJsonResponseProcessor.GetErrorJson(createDtroResponse);
//             Assert.Equal("Case naming convention exception", jsonErrorResponse.Message);
//             Assert.StartsWith("All property names must conform to camel case naming conventions. The following properties violate this: [Source, Provision, RegulatedPlace", jsonErrorResponse.Error);
//         }
//     }
// }