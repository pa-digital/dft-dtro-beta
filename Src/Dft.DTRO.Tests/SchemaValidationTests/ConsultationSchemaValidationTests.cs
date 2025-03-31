using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit;
using static Dft.DTRO.Tests.Utils;

namespace Dft.DTRO.Tests.SchemaValidationTests;

public class ConsultationSchemaValidationTests : IDisposable
{
    private static readonly JSchema _schema;

    static ConsultationSchemaValidationTests()
    {
        string schemaPath = GetSchema340();
        _schema = JSchema.Parse(File.ReadAllText(schemaPath));
    }

    [Fact]
    public void ConsultationValid()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Consultation", "valid", "Consultation.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.True(isValid);
    }

    [Fact]
    public void ConsultationValidateRequiredProperties()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Consultation", "valid", "Consultation.json");
        string[] requiredProperties = { "consultationName", "endOfConsultation", "statementOfReason", "source" };
        foreach (string property in requiredProperties)
        {
            JObject json = JObject.Parse(File.ReadAllText(path));
            JObject consultation = (JObject)json["consultation"];
            consultation.Remove(property);
            bool isValid = json.IsValid(_schema, out IList<string> errors);
            Assert.False(isValid);
        }
    }

    [Fact]
    public void ConsultationEndOfConsultationInvalidDateFormatDoesNotValidate()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Consultation", "invalid", "InvalidEndOfConsultationDate.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        string s = json.ToString();
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    [Fact]
    public void ConsultationPointOfContactEmailInvalidFormatDoesNotValidate()
    {
        string path = Path.Combine(GetProjectRoot(), "Src", "Dft.DTRO.Tests", "SchemaValidationTests", "Consultation", "invalid", "InvalidPointOfContactEmail.json");
        JObject json = JObject.Parse(File.ReadAllText(path));
        string s = json.ToString();
        bool isValid = json.IsValid(_schema, out IList<string> errors);
        Assert.False(isValid);
    }

    public void Dispose() { }

}