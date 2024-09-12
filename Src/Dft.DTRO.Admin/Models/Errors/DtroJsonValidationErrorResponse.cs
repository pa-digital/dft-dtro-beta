namespace Dft.DTRO.Admin.Models.Errors;

public class DtroJsonValidationErrorResponse
{
    public string Message { get; set; }

    public int LineNumber { get; set; }

    public int LinePosition { get; set; }

    public string Path { get; set; }

    public object? Value { get; set; }

    public string ErrorType { get; set; }

    public IList<DtroJsonValidationErrorResponse> ChildErrors { get; set; } = new List<DtroJsonValidationErrorResponse>();
}