using System.Collections.Generic;

namespace DfT.DTRO.Models.Errors;

/// <summary>
/// Object definition for API errors so that callers can received a common format.
/// </summary>
public class ApiErrorResponse
{
    /// <summary>
    /// The nature of the error.
    /// </summary>
    /// <value>Description of the error.</value>
    public string Message { get; set; }

    /// <summary>
    /// A list of errors encountered when processing the request.
    /// </summary>
    /// <value>Details of errors encountered.</value>
    public List<object> Errors { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="message">A summary message describing the error.</param>
    /// <param name="errors">Details of any errors encountered.</param>
    public ApiErrorResponse(string message, List<object> errors)
    {
        Message = message;
        Errors = errors;
    }

    /// <summary>
    /// Overload for when a single string is being yield as an error message.
    /// </summary>
    /// <param name="message">A summary message describing the error.</param>
    /// <param name="error">Details of an error encountered.</param>
    public ApiErrorResponse(string message, string error)
    {
        Message = message;
        Errors = new List<object> { error };
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public ApiErrorResponse()
    {
    }
}