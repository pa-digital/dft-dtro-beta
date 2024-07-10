using System.Collections.Generic;
using System.Threading.Tasks;
using DfT.DTRO.Models.SwaCode;

namespace DfT.DTRO.DAL;

/// <summary>
/// Service layer for Swa codes
/// </summary>
public interface ISwaCodeDal
{
    /// <summary>
    /// Get all Swa codes
    /// </summary>
    /// <returns>The list of swa codes response</returns>
    Task<List<SwaCodeResponse>> GetAllCodes();
}