namespace DfT.DTRO.DAL;

public interface IErrorReportDal
{
    Task CreateErrorReport(User user, DTRO.Models.DataBase.DTRO dtro, List<string> tras, List<string> regulationTypes, List<string> troTypes, string type, string? otherType, string moreInformation, List<string>? files);
}