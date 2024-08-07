using DfT.DTRO.DAL;
using DfT.DTRO.Migrations;

namespace DfT.DTRO;

public static class DbInitialize
{
    public static void SeedAppData(IApplicationBuilder app)
    {
        SeedSwaCodes(app);
        SeedConfig(app);
    }

    private static void SeedSwaCodes(IApplicationBuilder app)
    {
        using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
        {
            DtroContext context = serviceScope
                .ServiceProvider
                .GetService<DtroContext>();

            if (!context.SwaCodes.Any())
            {
                context.SwaCodes.AddRange(SeedData.TrafficAuthorities);
                context.SaveChanges();
            }
        }
    }
    private static void SeedConfig(IApplicationBuilder app)
    {
        using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
        {
            DtroContext context = serviceScope
                .ServiceProvider
                .GetService<DtroContext>();

            if (!context.SystemConfig.Any())
            {
                context.SystemConfig.Add(SeedData.SystemConfig);
                context.SaveChanges();
            }
        }
    }
    //TODO: The method below will be removed once
    //TODO: access to query the deployed database is granted
    public static void RunSqlStatement(IApplicationBuilder app)
    {
        using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
        {
            DtroContext dtroContext = serviceScope
                .ServiceProvider
                .GetService<DtroContext>();
            Console.WriteLine(DateTime.UtcNow);
            int swaCodesCount = dtroContext.SwaCodes.Count();
            Console.WriteLine(swaCodesCount);
            List<string> swaCodesNames = dtroContext
                .SwaCodes
                .Select(swaCode => swaCode.Name)
                .ToList();
            swaCodesNames.ForEach(Console.WriteLine);
            Console.WriteLine(DateTime.UtcNow);
        }
    }
 
}