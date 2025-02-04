namespace DfT.DTRO;

/// <summary>
/// Database initialize
/// </summary>
[ExcludeFromCodeCoverage]
public static class DbInitialize
{
    ///// <summary>
    ///// Empty Digital Traffic Regulation Order User table
    ///// </summary>
    ///// <param name="app">Application used</param>
    //public static void EmptyDtroUsersTable(this IApplicationBuilder app)
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

    ///// <summary>
    ///// Seed application data
    ///// </summary>
    ///// <param name="app">Application used</param>
    //public static void SeedAppData(this IApplicationBuilder app)
    //{
    //    app.SeedDtroUsers();
    //    app.SeedConfig();
    //}

    /// <summary>
    /// Seed table
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="app">Application used</param>
    /// <param name="entities">Entities to add</param>
    public static void Seed<T>(this IApplicationBuilder app, List<T> entities) where T : BaseEntity
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            DtroContext context = serviceScope.ServiceProvider.GetService<DtroContext>();
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }
    }

    private static void SeedDtroUsers(this IApplicationBuilder app)
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

    private static void SeedConfig(this IApplicationBuilder app)
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