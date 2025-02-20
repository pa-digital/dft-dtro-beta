using DfT.DTRO.Services.Validation.Contracts;
using Newtonsoft.Json;

namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class RecordManagementServiceTests
{
    private const string SourceJsonBasePath = "../../../TestFiles/D-TROs/3.2.3";

    [Theory]
    [InlineData("3.2.3", "temporary TRO - fullAmendment", true)]
    [InlineData("3.2.3", "temporary TRO - fullRevoke", true)]
    [InlineData("3.2.3", "temporary TRO - new-directedLinear", true)]
    [InlineData("3.2.3", "temporary TRO - new-linearGeometry", true)]
    [InlineData("3.2.3", "temporary TRO - new-pointGeometry", true)]
    [InlineData("3.2.3", "temporary TRO - new-polygon", true)]
    [InlineData("3.2.3", "temporary TRO - partialAmendment", true)]
    [InlineData("3.2.3", "temporary TRO - partialRevoke", true)]
    [InlineData("3.2.3", "valid-noChange", true)]
    public void ProducesCorrectResults(string schemaVersion, string file, bool isValid)
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

        Assert.Equal(isValid, !actual.Any());
    }
}