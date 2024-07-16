using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DfT.DTRO.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DfT.DTRO.DAL;

public class DtroHistoryDal : IDtroHistoryDal
{
    private readonly DtroContext _dtroContext;

    public DtroHistoryDal(DtroContext dtroContext)
    {
        _dtroContext = dtroContext;
    }

    public async Task<bool> SaveDtroInHistoryTable(DTROHistory dtroHistory)
    {
        EntityEntry<DTROHistory> entry = await _dtroContext.DtroHistories.AddAsync(dtroHistory);
        if (entry.Entity.Id == Guid.Empty)
        {
            return false;
        }

        await _dtroContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<DTROHistory>> GetDtroHistory(Guid dtroId)
    {
        var result = await _dtroContext.DtroHistories
            .Where(history => history.DtroId == dtroId)
            .OrderByDescending(history => history.LastUpdated)
            .ToListAsync();

        return result;
    }
}