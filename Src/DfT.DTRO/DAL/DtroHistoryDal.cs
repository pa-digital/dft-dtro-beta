using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DfT.DTRO.Extensions;
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

    /// <summary>
    /// Save current DTRO to DTRO History Table
    /// </summary>
    /// <param name="dtroHistory">the history dtro instance</param>
    /// <returns>
    /// A <see cref="Task"/> that resolved to <see langword="true"/>
    /// if the DTRO was successfully saved
    /// or <see langword="false"/> if it was not.
    /// </returns>
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

    /// <summary>
    /// Get list of the D-TROs from DTRO History Table
    /// </summary>
    /// <param name="reference">Source reference passed</param>
    /// <returns>List of historic D-TROs</returns>
    public async Task<List<DTROHistory>> GetDtroSourceHistory(string reference)
    {
        List<DTROHistory> histories = await _dtroContext.DtroHistories.ToListAsync();

        List<DTROHistory> currentHistory = new();
        foreach (DTROHistory history in histories)
        {
            var sourceReference = history.Data.GetExpando("source").GetValueOrDefault<string>("reference");
            if (sourceReference == reference)
            {
                currentHistory.Add(history);
            }
        }

        return currentHistory.OrderByDescending(history=>history.LastUpdated).ToList();
    }
}