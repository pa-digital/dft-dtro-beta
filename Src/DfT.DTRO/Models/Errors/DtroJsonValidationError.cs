using Newtonsoft.Json.Schema;
namespace DfT.DTRO.Models.Errors;

public class DtroJsonValidationError
{
    public string Message { get; set; }

    public int LineNumber { get; set; }

    public int LinePosition { get; set; }

    public string Path { get; set; }

    public object? Value { get; set; }

    public string ErrorType { get; set; }

    public IList<DtroJsonValidationError> ChildErrors { get; set; } = new List<DtroJsonValidationError>();
}