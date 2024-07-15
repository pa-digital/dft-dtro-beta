using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DfT.DTRO.Models.SwaCode;
using Microsoft.EntityFrameworkCore;

namespace DfT.DTRO.DAL;

/// <summary>
/// An implementation of <see cref="ISwaCodeDal"/>
/// that uses an SQL database as its store 
/// </summary>

[ExcludeFromCodeCoverage]
public class SwaCodeDal : ISwaCodeDal
{
    private readonly DtroContext _dtroContext;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="dtroContext">
    /// An instance of <see cref="DtroContext"/>
    /// representing the current database session.
    /// </param>
    public SwaCodeDal(DtroContext dtroContext) => _dtroContext = dtroContext;

    /// <summary>
    /// An implementation of <see cref="GetAllCodes"/>
    /// </summary>
    /// <returns>List of swa code responses</returns>
    public async Task<List<SwaCodeResponse>> GetAllCodes() =>
        await _dtroContext.SwaCodes
            .OrderBy(swaCode => swaCode.Name)
            .Select(swaCode => new SwaCodeResponse
            {
                TraId = swaCode.TraId,
                Name = swaCode.Name,
                Prefix = swaCode.Prefix,
                IsAdmin = swaCode.IsAdmin
            })
            .ToListAsync();
}