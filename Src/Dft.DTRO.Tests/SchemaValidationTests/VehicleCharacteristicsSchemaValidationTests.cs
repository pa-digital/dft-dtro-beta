using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit;
using static Dft.DTRO.Tests.Utils;

namespace Dft.DTRO.Tests.SchemaValidationTests;

public class VehicleCharacteristicsSchemaValidationTests : IDisposable
{
    private static readonly JSchema _schema;

    static VehicleCharacteristicsSchemaValidationTests()
    {
        string schemaPath = GetSchema340();
        _schema = JSchema.Parse(File.ReadAllText(schemaPath));
    }

    [Fact]
    public void VehicleCharacteristicsValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "VehicleCharacteristics", "valid", "VehicleCharacteristics.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void VehicleCharacteristicsVehicleUsageOtherButNoVehicleUsageTypeExtensionFailsValidation()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "VehicleCharacteristics", "invalid", "VehicleCharacteristicsVehicleUsageOtherButNoVehicleUsageTypeExtension.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void VehicleCharacteristicsVehicleUsageTypeOtherRequiresVehicleUsageTypeExtension()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "VehicleCharacteristics", "valid", "VehicleCharacteristicsVehicleUsageTypeOtherRequiresVehicleUsageTypeExtension.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    public void Dispose() { }

}