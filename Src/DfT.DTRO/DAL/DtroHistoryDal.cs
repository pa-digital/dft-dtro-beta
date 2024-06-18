﻿using System;
using System.Threading.Tasks;
using DfT.DTRO.Caching;
using DfT.DTRO.Models.DataBase;
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
    /// <param name="currentDtro"></param>
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
}