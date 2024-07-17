namespace DfT.DTRO.DAL;

public interface IDtroHistoryDal
{
    Task<bool> SaveDtroInHistoryTable(DTROHistory dtroHistory);

    Task<List<DTROHistory>> GetDtroHistory(Guid dtroId);
}