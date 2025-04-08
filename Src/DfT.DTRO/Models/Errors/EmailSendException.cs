namespace DfT.DTRO.Models.Errors;

/// <summary>
/// Represents errors that occur during email sending.
/// </summary>
public class EmailSendException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailSendException"/> class with a default error message.
    /// </summary>
    public EmailSendException() : base("Email send failure") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailSendException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public EmailSendException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailSendException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public EmailSendException(string message, Exception innerException) : base(message, innerException)
    {
    }
}