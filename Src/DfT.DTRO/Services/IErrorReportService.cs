namespace DfT.DTRO.Services;

/// <summary>
/// Error report service
/// </summary>
public interface IErrorReportService
{

    /// <summary>
    /// Create error report
    /// </summary>
    /// <param name="request">Submitted request</param>
    /// /// <param name="uploadPath">Path where files are uploaded</param>
    /// <returns>None</returns>
    Task CreateErrorReport(string username, List<string> filenames, ErrorReportRequest request);
}