using Newtonsoft.Json;

namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RecordManagementServiceTests
{
    private const string SourceJsonBasePath = "../../../../../examples/D-TROs/3.2.0";

    [Theory]
    [InlineData("3.2.0", "valid-new-x", true)]
    [InlineData("3.2.0", "invalid-source-reference", false)]
    [InlineData("3.2.0", "invalid-source-actionType", false)]
    [InlineData("3.2.0", "invalid-provision-reference", false)]
    [InlineData("3.2.0", "invalid-provision-actionType", false)]
    [InlineData("3.2.0", "invalid-duplicate-provision-reference", false)]
    public void ProducesCorrectResults(string schemaVersion, string file, bool isValid)
    {
        Mock<ISwaCodeDal> mockSwaCodeDal = new();
        IRecordManagementService sut = new RecordManagementService(mockSwaCodeDal.Object);


        mockSwaCodeDal.Setup(it => it.GetAllCodes().Result).Returns(() => Utils.SwaCodesResponse);

        string input = File.ReadAllText(Path.Join(SourceJsonBasePath, $"{file}.json"));

        DtroSubmit dtroSubmit = new()
        {
            SchemaVersion = schemaVersion,
            Data = JsonConvert.DeserializeObject<ExpandoObject>(input)
        };

        List<SemanticValidationError> actual = sut.ValidateCreationRequest(dtroSubmit, 1585);

        Assert.Equal(isValid, !actual.Any());
    }
}