// using Newtonsoft.Json.Linq;
// using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers;
// using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.FileHelper;
// using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

// namespace DfT.DTRO.IntegrationTests.IntegrationTests.CreateDtroTests.Schema_3_4_0
// {
//     public class DuplicateItems : BaseTest
//     {
//         readonly static string schemaVersionToTest = "3.4.0";
//         readonly string fileName = "JSON-3.4.0-example-TTRO-duplicate-provision-reference.json";

//         [Fact]
//         public async Task DtroSubmittedFromFileWithDuplicateProvisionReferenceShouldBeRejected()
//         {
//             // Generate user to send DTRO and read it back
//             TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
//             HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
//             Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
//             string userGuid = await JsonMethods.GetIdFromResponseJsonAsync(createUserResponse);
//             // Avoid files being overwritten by using a unique prefix in file names for each test
//             string uniquePrefixOnFileName = userGuid.Substring(0, 5);

//             // Prepare DTRO
//             string createDtroFile = $"{AbsolutePathToExamplesDirectory}/D-TROs/{schemaVersionToTest}/{fileName}";
//             string createDtroJson = File.ReadAllText(createDtroFile);
//             string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionToTest, createDtroJson, publisher.TraId);
//             string nameOfCopyFile = $"{uniquePrefixOnFileName}{fileName}";
//             string tempFilePath = $"{AbsolutePathToDtroExamplesTempDirectory}/{nameOfCopyFile}";
//             WriteStringToFile(AbsolutePathToDtroExamplesTempDirectory, nameOfCopyFile, createDtroJsonWithTraUpdated);

//             // Send DTRO
//             HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromFileAsync(tempFilePath, publisher);
//             Assert.Equal(HttpStatusCode.BadRequest, createDtroResponse.StatusCode);

//             // Check JSON response
//             var jsonErrorResponse = await JsonErrorResponseProcessor.GetErrorJson(createDtroResponse);
//             Assert.Equal("Error", jsonErrorResponse.Error);
//             Assert.Equal("Message", jsonErrorResponse.Message);

//             // // Get created DTRO
//             // string dtroId = await JsonMethods.GetIdFromResponseJsonAsync(createDtroResponse);
//             // HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
//             // Assert.Equal(HttpStatusCode.OK, getDtroResponse.StatusCode);
//             // string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();

//             // // Add ID to sent DTRO for comparison purposes
//             // JObject createJsonObject = JObject.Parse(createDtroJsonWithTraUpdated);
//             // createJsonObject["id"] = dtroId;

//             // // Check retrieved DTRO matches sent DTRO
//             // string sentCreateJsonWithId = createJsonObject.ToString();
//             // CompareJson(sentCreateJsonWithId, dtroResponseJson);
//         }

//         [Fact]
//         public async Task DtroSubmittedFromJsonBodyWithDuplicateProvisionReferenceShouldBeRejected()
//         {
//             // Generate user to send DTRO and read it back
//             TestUser publisher = TestUsers.GenerateUser(UserGroup.Tra);
//             HttpResponseMessage createUserResponse = await DtroUsers.CreateUserAsync(publisher);
//             Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);

//             // Prepare DTRO
//             string createDtroFile = $"{AbsolutePathToDtroExamplesDirectory}/{schemaVersionToTest}/{fileName}";
//             string createDtroJson = File.ReadAllText(createDtroFile);
//             string createDtroJsonWithTraUpdated = Dtros.UpdateTraIdInDtro(schemaVersionToTest, createDtroJson, publisher.TraId);

//             // Send DTRO
//             HttpResponseMessage createDtroResponse = await Dtros.CreateDtroFromJsonBodyAsync(createDtroJsonWithTraUpdated, publisher);
//             Assert.Equal(HttpStatusCode.Created, createDtroResponse.StatusCode);

//             // Get created DTRO
//             string dtroId = await JsonMethods.GetIdFromResponseJsonAsync(createDtroResponse);
//             HttpResponseMessage getDtroResponse = await Dtros.GetDtroAsync(dtroId, publisher);
//             Assert.Equal(HttpStatusCode.OK, getDtroResponse.StatusCode);
//             string dtroResponseJson = await getDtroResponse.Content.ReadAsStringAsync();

//             // Add ID to sent DTRO for comparison purposes
//             JObject createJsonObject = JObject.Parse(createDtroJsonWithTraUpdated);
//             createJsonObject["id"] = dtroId;

//             // Check retrieved DTRO matches sent DTRO
//             string sentCreateJsonWithId = createJsonObject.ToString();
//             JsonMethods.CompareJson(sentCreateJsonWithId, dtroResponseJson);
//         }
//     }
// }