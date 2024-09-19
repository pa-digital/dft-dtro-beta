namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class ParserTests
{
    private const string SourceJsonBasePath = "../../../../../examples/D-TROs/3.2.3";

    private readonly IParserService _parserService;

    public ParserTests(IParserService parserService)
    {
        _parserService = parserService;
    }

    [Fact]
    public void CaptureReturnsGeometrySection()
    {
        string fullPath = Path.Join(SourceJsonBasePath, "temporary TRO - new.json");
        string input = File.ReadAllText(fullPath);

        const string expected = "\"Polygon\": {\"polygon\": \"((30 10, 40 40, 20 40, 10 20, 30 10))\"}";
        string actual = _parserService.Capture(input);

        Assert.NotNull(actual);
        Assert.Contains(expected, actual);
    }
}