using System.Collections.Generic;
using System.Threading.Tasks;
using DfT.DTRO.Models.DataBase;

namespace DfT.DTRO.DAL;

public interface IDtroHistoryDal
{
    Task<bool> SaveDtroInHistoryTable(DTROHistory dtroHistory);

    Task<List<DTROHistory>> GetDtroSourceHistory(string reference);
}