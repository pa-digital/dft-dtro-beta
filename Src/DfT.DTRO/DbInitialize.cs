using DfT.DTRO.DAL;

namespace DfT.DTRO;

public static class DbInitialize
{
    public static void RunSqlStatement(IApplicationBuilder app)
    {
        using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
        {
            DtroContext context = serviceScope
                .ServiceProvider
                .GetService<DtroContext>();
            Console.WriteLine(DateTime.UtcNow);
            int swaCodesCount = context.SwaCodes.Count();
            Console.WriteLine(swaCodesCount);
            List<string> swaCodesNames = context
                .SwaCodes
                .Select(swaCode => swaCode.Name)
                .ToList();
            swaCodesNames.ForEach(Console.WriteLine);
            Console.WriteLine(DateTime.UtcNow);
        }
    }
}