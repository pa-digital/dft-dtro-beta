using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit;
using static Dft.DTRO.Tests.Utils;

namespace Dft.DTRO.Tests.SchemaValidationTests;

public class RateSchemaValidationTests : IDisposable
{
    private static readonly JSchema _schema;

    static RateSchemaValidationTests()
    {
        string schemaPath = GetSchema340();
        _schema = JSchema.Parse(File.ReadAllText(schemaPath));
    }

    [Fact]
    public void RateValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Rate", "valid", "Rate.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void RateLineValidateRequiredProperties()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Rate", "valid", "Rate.json");
        string[] requiredProperties = { "sequence", "type", "value", };
        foreach (string property in requiredProperties)
        {
            JObject json = JObject.Parse(File.ReadAllText(path));
            JObject rateLine = (JObject)json["source"]["provision"][1]["regulation"][0]["condition"][0]["rateTable"]["rateLineCollection"][0]["rateLine"][0];
            rateLine.Remove(property);
            bool isValid = json.IsValid(_schema, out IList<string> errors);
            Assert.False(isValid);
        }
    }

    [Fact]
    public void RateLineCollectionValidateRequiredProperties()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Rate", "valid", "Rate.json");
        string[] requiredProperties = { "sequence", "applicableCurrency", "startValidUsagePeriod", "rateLine" };
        foreach (string property in requiredProperties)
        {
            JObject json = JObject.Parse(File.ReadAllText(path));
            JObject rateLineCollection = (JObject)json["source"]["provision"][1]["regulation"][0]["condition"][0]["rateTable"]["rateLineCollection"][0];
            rateLineCollection.Remove(property);
            bool isValid = json.IsValid(_schema, out IList<string> errors);
            Assert.False(isValid);
        }
    }

    [Fact]
    public void RateTableValidateRequiredProperties()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Rate", "valid", "Rate.json");
        string[] requiredProperties = { "rateLineCollection" };
        foreach (string property in requiredProperties)
        {
            JObject json = JObject.Parse(File.ReadAllText(path));
            JObject rateTable = (JObject)json["source"]["provision"][1]["regulation"][0]["condition"][0]["rateTable"];
            rateTable.Remove(property);
            bool isValid = json.IsValid(_schema, out IList<string> errors);
            Assert.False(isValid);
        }
    }

    [Fact]
    public void RateIncorrectDurationFormatFailsValidation()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Rate", "invalid", "IncorrectDurationFormat.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    public void Dispose() { }

}