using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit;
using static Dft.DTRO.Tests.Utils;

namespace Dft.DTRO.Tests.SchemaValidationTests;

public class ProvisionSchemaValidationTests : IDisposable
{
    private static readonly JSchema _schema;

    static ProvisionSchemaValidationTests()
    {
        string schemaPath = GetSchema340();
        _schema = JSchema.Parse(File.ReadAllText(schemaPath));
    }

    [Fact]
    public void ProvisionValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Provision", "valid", "Provision.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void ProvisionValidateRequiredProperties()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Provision", "valid", "Provision.json");
        string[] requiredProperties = { "actionType", "orderReportingPoint", "provisionDescription", "reference" };
        foreach (string property in requiredProperties)
        {
            JObject json = JObject.Parse(File.ReadAllText(path));
            JObject provision = (JObject)json["source"]["provision"][0];
            provision.Remove(property);
            bool isValid = json.IsValid(_schema, out IList<string> errors);
            Assert.False(isValid);
        }
    }

    [Fact]
    public void TemporaryOrderReportingPointValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Provision", "valid", "TemporaryOrderReportingPoint.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void ExperimentalCessationAndExperimentalVariationValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Provision", "valid", "ExperimentalCessationAndExperimentalVariation.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void ActualStartOrStopPresentWithNonTROOnRoadActiveStatusOrderReportingPointShouldFail()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Provision", "invalid", "ActualStartOrStopPresentWithNonTROOnRoadActiveStatusOrderReportingPoint.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void ExperimentalOrderReportingPointButNoExperimentalCessationOrExperimentalVariationShouldFail()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Provision", "invalid", "ExperimentalOrderReportingPointButNoExperimentalCessationOrExperimentalVariation.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void NonExperimentalOrderReportingPointWithExperimentalCessationAndExperimentalVariationShouldFail()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Provision", "invalid", "NonExperimentalOrderReportingPointWithExperimentalCessationAndExperimentalVariation.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void InvalidExpectedOccupancyDurationShouldFail()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Provision", "invalid", "ExpectedOccupancyDurationInvalidFormat.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void ActualStartOrStopPresentWithTROOnRoadActiveStatusOrderReportingPoint()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Provision", "valid", "ActualStartOrStopPresentWithTROOnRoadActiveStatusOrderReportingPoint.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    public void Dispose() { }

}