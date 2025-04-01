using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit;
using static Dft.DTRO.Tests.Utils;

namespace Dft.DTRO.Tests.SchemaValidationTests;

public class RegulationSchemaValidationTests : IDisposable
{
    private static readonly JSchema _schema;

    static RegulationSchemaValidationTests()
    {
        string schemaPath = GetSchema340();
        _schema = JSchema.Parse(File.ReadAllText(schemaPath));
    }

    [Fact]
    public void RegulationValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Regulation", "valid", "Regulation.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void RegulationValidateRequiredProperties()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Regulation", "valid", "Regulation.json");
        string[] requiredProperties = { "isDynamic", "timeZone" };
        foreach (string property in requiredProperties)
        {
            JObject json = JObject.Parse(File.ReadAllText(path));
            JObject regulation = (JObject)json["source"]["provision"][0]["regulation"][0];
            regulation.Remove(property);
            bool isValid = json.IsValid(_schema, out IList<string> errors);
            Assert.False(isValid);
        }
    }

    [Fact]
    public void RegulationOneOfConstraintViolationShouldFail()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Regulation", "invalid", "ViolatesRegulationOneOfConstraint.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void MissingConditionConditionsOrConditionSetShouldFail()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Regulation", "invalid", "MissingConditionConditionsOrConditionSet.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void ConditionOneOfConstraintViolationShouldFail()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Regulation", "invalid", "ViolatesConditionOneOfConstraint.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    public void Dispose() { }

}