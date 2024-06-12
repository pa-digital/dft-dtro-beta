using System.Threading.Tasks;
using DfT.DTRO.Models.DtroDtos;

namespace DfT.DTRO.Services;

public interface IDtroHistoryService
{
    void UpdateDetails(Models.DataBase.DTRO currentDtro, DtroSubmit dtroSubmit);
}