using System.Collections.Generic;
using System.Threading.Tasks;
using DfT.DTRO.Models.SwaCode;

namespace DfT.DTRO.DAL;

public interface ISwaCodeDal
{
    Task<List<SwaCodeResponse>> GetAllCodes();
}