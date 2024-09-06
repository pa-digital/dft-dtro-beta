using DfT.DTRO.Migrations;

namespace DfT.DTRO;

public class DbInitialize
{
    //public static void EmptyDtroUsersTable(IApplicationBuilder app)
    //{
    //    using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
    //    {
    //        DtroContext context = serviceScope
    //            .ServiceProvider
    //            .GetService<DtroContext>();

    //        context.DtroUsers.RemoveRange();
    //        context.SaveChanges();
    //    }
    //}

    public static void SeedAppData(IApplicationBuilder app)
    {
        SeedDtroUsers(app);
        SeedConfig(app);
    }

    private static void SeedDtroUsers(IApplicationBuilder app)
    {
        using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
        {
            DtroContext context = serviceScope
                .ServiceProvider
                .GetService<DtroContext>();

            if (!context.DtroUsers.Any())
            {
                context.DtroUsers.AddRange(SeedData.TrafficAuthorities);
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