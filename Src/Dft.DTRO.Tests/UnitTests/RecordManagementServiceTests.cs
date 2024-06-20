using System.Dynamic;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Validation;
using DfT.DTRO.Services.Validation;
using Newtonsoft.Json;

namespace Dft.DTRO.Tests;

public class RecordManagementServiceTests
{
    private const string SourceJsonBasePath = "./DtroJsonDataExamples/v3.2.0";

    [Theory]
    [InlineData("3.2.0", "valid-new-x", true)]
    [InlineData("3.2.0", "invalid-source-reference", false)]
    [InlineData("3.2.0", "invalid-source-actionType", false)]
    [InlineData("3.2.0", "invalid-provision-reference", false)]
    [InlineData("3.2.0", "invalid-provision-actionType", false)]
    [InlineData("3.2.0", "invalid-duplicate-provision-reference", false)]
    public void ProducesCorrectResults(string schemaVersion, string file, bool isValid)
    {
        IRecordManagementService sut = new RecordManagementService();

        var input = File.ReadAllText(Path.Join(SourceJsonBasePath, $"{file}.json"));

        DtroSubmit dtroSubmit = new()
        {
            SchemaVersion = schemaVersion, 
            Data = JsonConvert.DeserializeObject<ExpandoObject>(input)
        };

        List<SemanticValidationError> actual = sut.ValidateCreationRequest(dtroSubmit,null);

        Assert.Equal(isValid, !actual.Any());
    }
}