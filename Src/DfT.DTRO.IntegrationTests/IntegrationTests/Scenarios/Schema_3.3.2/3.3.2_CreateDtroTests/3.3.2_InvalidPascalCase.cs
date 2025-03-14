// using Newtonsoft.Json;
// using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
// using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelper;
// using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

// namespace DfT.DTRO.IntegrationTests.IntegrationTests.CreateDtroTests.Schema_3_3_2
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
//             TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
//             HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
//             Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
//             string userGuid = await GetIdFromResponseJsonAsync(createUserResponse);
//             // Avoid files being overwritten by using a unique prefix in file names for each test
//             string uniquePrefixOnFileName = userGuid.Substring(0, 5);

//             // Prepare DTRO
//             string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{filesWithInvalidPascalCase}/{fileName}";
//             string createDtroJson = File.ReadAllText(createDtroFile);
//             string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(filesWithInvalidPascalCase, createDtroJson, publisher.TraId);
//             string createDtroJsonWithSchemaVersionUpdated = Dtros.UpdateSchemaVersionInDtro(createDtroJsonWithTraUpdated, schemaVersionToTest);
//             string nameOfCopyFile = $"{uniquePrefixOnFileName}{fileName}";
//             string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
//             WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithSchemaVersionUpdated);

//             // Send DTRO
//             HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
//             Assert.Equal(HttpStatusCode.BadRequest, createDtroResponse.StatusCode);

//             // Check DTRO response JSON
//             string dtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
//             dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(dtroResponseJson)!;
//             string responseMessage = jsonDeserialised.message.ToString();
//             string responseError = jsonDeserialised.error.ToString();
//             Assert.Equal("Case naming convention exception", responseMessage);
//             Assert.StartsWith("All property names must conform to camel case naming conventions. The following properties violate this: [Source, Provision, RegulatedPlace", responseError);
//         }

//         [Theory]
//         [MemberData(nameof(GetDtroFileNames))]
//         public async Task DtroSubmittedFromJsonBodyWithPascalCaseShouldBeRejected(string fileName)
//         {
//             Console.WriteLine($"\nTesting with file {fileName}...");

//             // Generate user to send DTRO and read it back
//             TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
//             HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
//             Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

//             // Prepare DTRO
//             string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{filesWithInvalidPascalCase}/{fileName}";
//             string createDtroJson = File.ReadAllText(createDtroFile);
//             string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(filesWithInvalidPascalCase, createDtroJson, publisher.TraId);
//             string createDtroJsonWithSchemaVersionUpdated = Dtros.UpdateSchemaVersionInDtro(createDtroJsonWithTraUpdated, schemaVersionToTest);

//             // Send DTRO
//             HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithSchemaVersionUpdated, publisher);
//             Assert.Equal(HttpStatusCode.BadRequest, createDtroResponse.StatusCode);

//             // Check DTRO response JSON
//             string dtroResponseJson = await createDtroResponse.Content.ReadAsStringAsync();
//             dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(dtroResponseJson)!;
//             string responseMessage = jsonDeserialised.message.ToString();
//             string responseError = jsonDeserialised.error.ToString();
//             Assert.Equal("Case naming convention exception", responseMessage);
//             Assert.StartsWith("All property names must conform to camel case naming conventions. The following properties violate this: [Source, Provision, RegulatedPlace", responseError);
//         }
//     }
// }