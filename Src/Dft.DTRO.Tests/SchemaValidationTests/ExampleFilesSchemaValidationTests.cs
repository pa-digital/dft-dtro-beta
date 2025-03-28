using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit;
using Dft.DTRO.Tests;
using static Dft.DTRO.Tests.Utils;

namespace Dft.DTRO.Tests.SchemaValidationTests;

public class ExampleFilesSchemaValidationTests : IDisposable
{
    private static readonly JSchema _schema;

    static ExampleFilesSchemaValidationTests()
    {
        string schemaPath = GetSchema340();
        _schema = JSchema.Parse(File.ReadAllText(schemaPath));
    }

    public static IEnumerable<object[]> GetExampleFiles()
    {
        string path = Path.Combine(GetProjectRoot(), "examples", "D-TROs", "3.4.0");
        string[] files = Directory.GetFiles(path, "*.json");
        foreach (var file in files)
        {
            yield return new object[] { file };
        }
    }

    [Theory]
    [MemberData(nameof(GetExampleFiles))]
    public void ValidateExampleFile(string filePath)
    {
        JObject json = JObject.Parse(File.ReadAllText(filePath))["data"].Value<JObject>();
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    public void Dispose() { }

}