namespace DfT.DTRO.Services;

public interface IParserService
{
    string Capture(string source);

    string Adjust(string source);

    string Replace(string source);
}