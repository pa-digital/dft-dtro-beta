using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DfT.DTRO.Models.SwaCode;
using Microsoft.EntityFrameworkCore;

namespace DfT.DTRO.DAL;

[ExcludeFromCodeCoverage]
public class SwaCodeDal : ISwaCodeDal
{
    private readonly DtroContext _dtroContext;

    public SwaCodeDal(DtroContext dtroContext) => _dtroContext = dtroContext;

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