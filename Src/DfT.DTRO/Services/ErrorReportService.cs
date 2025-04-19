namespace DfT.DTRO.Services;

public class ErrorReportService : IErrorReportService
{
    private readonly IErrorReportDal _errorReportDal;
    private readonly IUserDal _userDal;
    private readonly IDtroDal _dtroDal;

    public ErrorReportService(IErrorReportDal errorReportDal, IUserDal userDal, IDtroDal dtroDal)
    {
        _errorReportDal = errorReportDal;
        _userDal = userDal;
        _dtroDal = dtroDal;
    }

    public async Task CreateErrorReport(string username, List<string> filenames, ErrorReportRequest request)
    {
        User user = await _userDal.GetUserFromEmail(username);

        Guid? dtroId = null;
        DTRO.Models.DataBase.DTRO? dtro = null;
        if (request.TroId != null)
        {
            dtroId = Guid.Parse(request.TroId);
            dtro = await _dtroDal.GetDtroByIdAsync(dtroId.Value);
        }

        await _errorReportDal.CreateErrorReport(user, dtro, request.Tras, request.RegulationTypes, request.TroTypes, request.Type, request.OtherType, request.MoreInformation, filenames);
    }
}