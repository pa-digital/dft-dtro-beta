namespace Dft.DTRO.Tests.Mocks;

[ExcludeFromCodeCoverage]
public static class MockTestObjects
{
    public static IFormFile TestFile
    {
        get
        {
            var bytes = "{ \"schemaVersion\": \"1.0.0\", \"data\": { \"item\": \"This is a test file\"}}"u8.ToArray();
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            return file;
        }
    }

    public static GuidResponse GuidResponse =>
        new()
        {
            Id = Guid.NewGuid()
        };

    public static List<DtroUserResponse> UserResponses =>
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name  = "Somerset Council",
            TraId = 3300,
            Prefix = "LG",
            UserGroup = UserGroup.Tra,
            xAppId = Guid.NewGuid()
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name  = "Derbyshire",
            TraId = 1050,
            Prefix = "DJ",
            UserGroup = UserGroup.Tra,
            xAppId = Guid.NewGuid()
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name  = "Appyway",
            TraId = 4,
            Prefix = "AP",
            UserGroup = UserGroup.Tra,
            xAppId = Guid.NewGuid()
        }
    ];
}