using System.Threading.Tasks;

namespace DfT.DTRO.DAL;

public interface IDtroHistoryDal
{
    Task<bool> SaveDtroInHistoryTable(Models.DataBase.DTRO currentDtro);
}