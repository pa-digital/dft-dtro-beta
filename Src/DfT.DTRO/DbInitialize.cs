using DfT.DTRO.Migrations;

namespace DfT.DTRO;

public class DbInitialize
{
    public static void SeedAppData(IApplicationBuilder app)
    {
        SeedConfig(app);
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