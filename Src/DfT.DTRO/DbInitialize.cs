using DfT.DTRO.DAL;

namespace DfT.DTRO;

//TODO: The class and method below will be removed once
//TODO: access to query the deployed database is granted
public class DbInitialize
{
    private readonly ILogger<DbInitialize> _logger;

    public DbInitialize(ILogger<DbInitialize> logger)
    {
        _logger = logger;
    }

    public void RunSqlStatement(IApplicationBuilder app)
    {
        using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
        {
            DtroContext context = serviceScope
                .ServiceProvider
                .GetService<DtroContext>();
            Console.WriteLine(DateTime.UtcNow);
            _logger.LogInformation(DateTime.UtcNow.ToString("F"));
            int swaCodesCount = context.SwaCodes.Count();
            Console.WriteLine(swaCodesCount);
            _logger.LogDebug($"SWA Code Count: {swaCodesCount}");
            List<string> swaCodesNames = context
                .SwaCodes
                .Select(swaCode => swaCode.Name)
                .ToList();
            swaCodesNames.ForEach(Console.WriteLine);
            _logger.LogDebug($"SWA Code Names: {string.Join(",", swaCodesNames)}");
            Console.WriteLine(DateTime.UtcNow);
            _logger.LogInformation(DateTime.UtcNow.ToString("F"));
        }
    }
}