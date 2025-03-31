namespace DfT.DTRO.Models.Errors;
public class CaseException : Exception
{
    public CaseException()
        : base("Case naming convention exception")
    {
    }

    public CaseException(string message)
        : base(message)
    {
    }

    public CaseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}