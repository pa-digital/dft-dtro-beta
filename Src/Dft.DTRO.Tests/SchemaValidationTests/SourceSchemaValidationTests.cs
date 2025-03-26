using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit;
using static Dft.DTRO.Tests.Utils;

namespace Dft.DTRO.Tests.SchemaValidationTests;

public class SourceSchemaValidationTests : IDisposable
{
    private static readonly JSchema _schema;

    static SourceSchemaValidationTests()
    {
        string schemaPath = GetSchema340();
        _schema = JSchema.Parse(File.ReadAllText(schemaPath));
    }

    [Fact]
    public void SourceValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Source", "valid", "Source.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void SourceValidateRequiredProperties()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Source", "valid", "Source.json");
        string[] requiredProperties = { "actionType", "currentTraOwner", "reference", "section", "traAffected", "traCreator", "troName", "provision", "statementDescription" };
        foreach (string property in requiredProperties)
        {
            JObject json = JObject.Parse(File.ReadAllText(path));
            JObject source = (JObject)json["source"];
            source.Remove(property);
            bool isValid = json.IsValid(_schema, out IList<string> errors);
            Assert.False(isValid);
        }
    }

    [Fact]
    public void MissingMadeDateAndComingIntoForceDateFailsValidation()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Source", "invalid", "MissingMadeDateAndComingIntoForceDate.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void PresentMadeDateAndComingIntoForceDateWithWrongOrderReportingPointFailsValidation()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Source", "invalid", "MadeDateAndComingIntoForceDateWithWrongOrderReportingPoint.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    public void Dispose() { }

}