
namespace DfT.DTRO.DAL;

public class ErrorReportDal(DtroContext context) : IErrorReportDal
{
    private readonly DtroContext _context = context;

    public async Task CreateErrorReport(User user, Models.DataBase.DTRO? dtro, List<string> tras, List<string> regulationTypes, List<string> troTypes, string type, string otherType, string moreInformation, List<string> files)
    {
        var errorReport = new ErrorReport
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TroId = dtro?.Id,
            Tras = tras,
            RegulationTypes = regulationTypes,
            TroTypes = troTypes,
            Type = type,
            OtherType = string.IsNullOrWhiteSpace(otherType) ? null : otherType,
            MoreInformation = moreInformation,
            FilePaths = files != null && files.Any() ? string.Join(",", files) : null
        };

        _context.ErrorReport.Add(errorReport);
        await _context.SaveChangesAsync();
    }


}