using System.Diagnostics.CodeAnalysis;
using DfT.DTRO.DAL;
using DfT.DTRO.JsonLogic;
using DfT.DTRO.Models.RuleTemplate;
using Microsoft.EntityFrameworkCore;
using SchemaVersion = DfT.DTRO.Models.SchemaTemplate.SchemaVersion;

namespace DfT.DTRO.Services;

[ExcludeFromCodeCoverage]
public class SystemConfigDal : ISystemConfigDal
{
    private readonly DtroContext _dtroContext;

    public SystemConfigDal(DtroContext dtroContext)
    {
        _dtroContext = dtroContext;
    }

    public async Task<SystemConfig> GetSystemConfigAsync()
    {
        var config = await _dtroContext.SystemConfig.FirstOrDefaultAsync();
        if (config == null)
        {
            return new SystemConfig() { SystemName = "Not Set (record not found in database)" };
        }

        return config;
    }
}