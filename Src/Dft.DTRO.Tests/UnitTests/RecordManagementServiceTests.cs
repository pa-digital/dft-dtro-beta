using Newtonsoft.Json;

namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RecordManagementServiceTests
{
    private const string SourceJsonBasePath = "../../../TestFiles/D-TROs/3.2.3";

    [Theory]
    [InlineData("3.2.3", "temporary TRO - fullAmendment", false)]
    [InlineData("3.2.3", "temporary TRO - fullRevoke", false)]
    [InlineData("3.2.3", "temporary TRO - new-directedLinear", false)]
    [InlineData("3.2.3", "temporary TRO - new-linearGeometry", false)]
    [InlineData("3.2.3", "temporary TRO - new-pointGeometry", false)]
    [InlineData("3.2.3", "temporary TRO - new-polygon", false)]
    [InlineData("3.2.3", "temporary TRO - partialAmendment", false)]
    [InlineData("3.2.3", "temporary TRO - partialRevoke", false)]
    [InlineData("3.2.3", "valid-noChange", false)]
    public void ProducesCorrectResults(string schemaVersion, string file, bool isInvalid)
    {
        Mock<IDtroUserDal> mockSwaCodeDal = new();
        IRecordManagementService sut = new RecordManagementService(mockSwaCodeDal.Object);


        mockSwaCodeDal.Setup(it => it.GetAllDtroUsersAsync().Result).Returns(() => Utils.SwaCodesResponse);

        string input = File.ReadAllText(Path.Join(SourceJsonBasePath, $"{file}.json"));

        DtroSubmit dtroSubmit = new()
        {
            SchemaVersion = schemaVersion,
            Data = JsonConvert.DeserializeObject<ExpandoObject>(input)
        };

        List<SemanticValidationError> actual = sut.ValidateRecordManagement(dtroSubmit, 1000);

        Assert.Equal(isInvalid, !actual.Any());
    }
}