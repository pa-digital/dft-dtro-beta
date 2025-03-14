// using Newtonsoft.Json;
// using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
// using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelper;
// using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

// namespace DfT.DTRO.IntegrationTests.IntegrationTests.UpdateDtroTests.Schema_3_3_2
// {
//     public class InvalidPascalCase : BaseTest
//     {
//         readonly static string schemaVersionToTest = "3.3.2";
//         readonly static string schemaVersionOfFilesWithInvalidPascalCase = "3.3.1";
//         string fileToUseWithCamelCaseVersion3_3_2 = "JSON-3.3.2-example-Derbyshire 2024 DJ388 partial.json";

//         public static IEnumerable<object[]> GetDtroFileNames()
//         {
//             DirectoryInfo directoryPath = new DirectoryInfo($"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionOfFilesWithInvalidPascalCase}");
//             FileInfo[] files = directoryPath.GetFiles();

//             foreach (FileInfo file in files)
//             {
//                 yield return new object[] { file.Name };
//             }
//         }

//         [Theory]
//         [MemberData(nameof(GetDtroFileNames))]
//         public async Task DtroUpdatedFromFileWithPascalCaseShouldBeRejected(string nameOfFileWithPascalCaseVersion3_3_1)
//         {
//             Console.WriteLine($"\nTesting with file {nameOfFileWithPascalCaseVersion3_3_1}...");

//             // Generate user to send DTRO and read it back
//             TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
//             HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
//             Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
//             string userGuid = await GetIdFromResponseJsonAsync(createUserResponse);
//             // Avoid files being overwritten by using a unique prefix in file names for each test
//             string uniquePrefixOnFileName = userGuid.Substring(0, 5);

//             // Prepare DTRO
//             string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersionToTest}/{fileToUseWithCamelCaseVersion3_3_2}";
//             string createDtroJson = File.ReadAllText(createDtroFile);
//             string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionToTest, createDtroJson, publisher.TraId);
//             string nameOfCopyFile = $"{uniquePrefixOnFileName}{fileToUseWithCamelCaseVersion3_3_2}";
//             string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
//             WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithTraUpdated);

//             // Send DTRO
//             HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
//             Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

//             // Prepare DTRO update
//             string updateDtroFile = $"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionOfFilesWithInvalidPascalCase}/{nameOfFileWithPascalCaseVersion3_3_1}";
//             string updateDtroJson = File.ReadAllText(updateDtroFile);
//             string updateDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionOfFilesWithInvalidPascalCase, updateDtroJson, publisher.TraId);
//             string updateDtroJsonWithSchemaVersionUpdated = Dtros.UpdateSchemaVersionInDtro(updateDtroJsonWithTraUpdated, schemaVersionToTest);
//             string updateJsonWithModifiedActionTypeAndTroName = Dtros.UpdateActionTypeAndTroName(updateDtroJsonWithSchemaVersionUpdated, schemaVersionOfFilesWithInvalidPascalCase);
//             string nameOfUpdateCopyFile = $"update{uniquePrefixOnFileName}{nameOfFileWithPascalCaseVersion3_3_1}";
//             string tempUpdateFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfUpdateCopyFile}";
//             WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfUpdateCopyFile, updateJsonWithModifiedActionTypeAndTroName);

//             // Send DTRO update
//             string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);
//             HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromFileAsync(tempUpdateFilePath, dtroId, publisher);
//             Assert.Equal(HttpStatusCode.BadRequest, updateDtroResponse.StatusCode);

//             // Check DTRO response JSON
//             string dtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
//             dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(dtroResponseJson)!;
//             string responseMessage = jsonDeserialised.message.ToString();
//             string responseError = jsonDeserialised.error.ToString();
//             Assert.Equal("Case naming convention exception", responseMessage);
//             Assert.StartsWith("All property names must conform to camel case naming conventions. The following properties violate this: [Source, Provision, RegulatedPlace", responseError);
//         }

//         [Theory]
//         [MemberData(nameof(GetDtroFileNames))]
//         public async Task DtroUpdatedFromJsonBodyWithPascalCaseShouldBeRejected(string nameOfFileWithPascalCaseVersion3_3_1)
//         {
//             Console.WriteLine($"\nTesting with file {nameOfFileWithPascalCaseVersion3_3_1}...");

//             // Generate user to send DTRO and read it back
//             TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
//             HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
//             Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

//             // Prepare DTRO
//             string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersionToTest}/{fileToUseWithCamelCaseVersion3_3_2}";
//             string createDtroJson = File.ReadAllText(createDtroFile);
//             string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionToTest, createDtroJson, publisher.TraId);

//             // Send DTRO
//             HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraUpdated, publisher);
//             Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

//             // Prepare DTRO update
//             string updateDtroFile = $"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionOfFilesWithInvalidPascalCase}/{nameOfFileWithPascalCaseVersion3_3_1}";
//             string updateDtroJson = File.ReadAllText(updateDtroFile);
//             string updateDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionOfFilesWithInvalidPascalCase, updateDtroJson, publisher.TraId);
//             string updateDtroJsonWithSchemaVersionUpdated = Dtros.UpdateSchemaVersionInDtro(updateDtroJsonWithTraUpdated, schemaVersionToTest);
//             string updateJsonWithModifiedActionTypeAndTroName = Dtros.UpdateActionTypeAndTroName(updateDtroJsonWithSchemaVersionUpdated, schemaVersionOfFilesWithInvalidPascalCase);

//             // Send DTRO update
//             string dtroId = await GetIdFromResponseJsonAsync(createDtroResponse);
//             HttpResponseMessage updateDtroResponse = await Dtros.UpdateDtroFromJsonBodyAsync(updateJsonWithModifiedActionTypeAndTroName, dtroId, publisher);
//             Assert.Equal(HttpStatusCode.BadRequest, updateDtroResponse.StatusCode);

//             // Check DTRO response JSON
//             string dtroResponseJson = await updateDtroResponse.Content.ReadAsStringAsync();
//             dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(dtroResponseJson)!;
//             string responseMessage = jsonDeserialised.message.ToString();
//             string responseError = jsonDeserialised.error.ToString();
//             Assert.Equal("Case naming convention exception", responseMessage);
//             Assert.StartsWith("All property names must conform to camel case naming conventions. The following properties violate this: [Source, Provision, RegulatedPlace", responseError);
//         }
//     }
// }