using DfT.DTRO.DAL;

namespace DfT.DTRO;

public static class DbInitialize
{
    public static void RunSqlStatement(DtroContext dtroContext)
    {
        Console.WriteLine(DateTime.UtcNow);
        int swaCodesCount = dtroContext.SwaCodes.Count();
        Console.WriteLine(swaCodesCount);
        List<string> swaCodesNames = dtroContext.SwaCodes.Select(swaCode => swaCode.Name).ToList();
        swaCodesNames.ForEach(Console.WriteLine);
        Console.WriteLine(DateTime.UtcNow);
    }
}