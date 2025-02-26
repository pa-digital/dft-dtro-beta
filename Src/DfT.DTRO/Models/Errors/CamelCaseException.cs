namespace DfT.DTRO.Models.Errors;
public class CamelCaseException : Exception
{
    public CamelCaseException()
        : base("Camel case naming convention exception")
    {
    }

    public CamelCaseException(string message)
        : base(message)
    {
    }

    public CamelCaseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}