namespace DfT.DTRO.Models.Validation;

public class SemanticValidationError
{
    public string Message { get; set; }

    public string Path { get; set; }

    public string ErrorType
    {
        get { return "semantic"; }
    }
}