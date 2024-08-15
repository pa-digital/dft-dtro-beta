using DfT.DTRO.Migrations;

namespace DfT.DTRO;

public class DbInitialize
{
    public static void GrantPermission(IApplicationBuilder app)
    {
        using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
        {
            using (DtroContext context = serviceScope.ServiceProvider.GetService<DtroContext>())
            {
                const string rawSql = "GRANT SELECT ON ALL TABLES IN SCHEMA public TO *";
                context.Database.ExecuteSqlRaw(rawSql);
            }
        }
    }

    public static void EmptySwaCodesTable(IApplicationBuilder app)
    {
        using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
        {
            DtroContext context = serviceScope
                .ServiceProvider
                .GetService<DtroContext>();

            context.SwaCodes.RemoveRange();
            context.SaveChanges();
        }
    }

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
}