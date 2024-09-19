namespace DfT.DTRO.Services;

public class ParserService : IParserService
{
    public string Capture(string source)
    {
        int position = source.IndexOf("geometry:", StringComparison.Ordinal);
        int adjPos = position + 1;
        return source[adjPos..];
    }

    public string Adjust(string source)
    {
        throw new NotImplementedException();
    }

    public string Replace(string source)
    {
        throw new NotImplementedException();
    }
}