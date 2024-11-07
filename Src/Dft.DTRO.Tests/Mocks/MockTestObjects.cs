namespace Dft.DTRO.Tests.Mocks;

public static class MockTestObjects
{
    public static IFormFile TestFile
    {
        get
        {
            var bytes = Encoding.UTF8.GetBytes("{ \"schemaVersion\": \"1.0.0\", \"data\": { \"item\": \"This is a test file\"}}");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            return file;
        }
    }

    public static DtroSubmit DtroSubmit =>
        new()
        {
            SchemaVersion = new SchemaVersion("1.0.0"),
            Data = new ExpandoObject()
        };

    public static GuidResponse GuidResponse =>
        new()
        {
            Id = Guid.NewGuid()
        };
}