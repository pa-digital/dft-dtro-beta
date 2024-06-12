using System.Threading.Tasks;
using DfT.DTRO.Models.DtroDtos;

namespace DfT.DTRO.Services;

public class DtroHistoryService : IDtroHistoryService
{
    public void UpdateDetails(Models.DataBase.DTRO currentDtro, DtroSubmit dtroSubmit)
    {
        Models.DataBase.DTRO dtro = new()
        {
            SchemaVersion = dtroSubmit.SchemaVersion, 
            Created = currentDtro.Created,
            RegulationStart = currentDtro.RegulationStart,
            RegulationEnd = currentDtro.RegulationEnd,
            TrafficAuthorityId = currentDtro.TrafficAuthorityId,
            TroName = currentDtro.TroName,
            CreatedCorrelationId = currentDtro.CreatedCorrelationId,
            Deleted = currentDtro.Deleted,
            DeletionTime = currentDtro.DeletionTime,
            Data = dtroSubmit.Data,
            RegulationTypes = currentDtro.RegulationTypes,
            VehicleTypes = currentDtro.VehicleTypes,
            OrderReportingPoints = currentDtro.OrderReportingPoints,
            Location = currentDtro.Location
        };

        dtro.LastUpdated = dtro.Created;
        dtro.LastUpdatedCorrelationId = dtro.CreatedCorrelationId;
    }
}