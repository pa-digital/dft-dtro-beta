namespace Dft.DTRO.Tests.RepositoryTests;

public class TestContextBuilder : IDisposable
{
    protected readonly DtroContext MockDbContext;

    protected TestContextBuilder()
    {
        DbContextOptions<DtroContext> options = new DbContextOptionsBuilder<DtroContext>()
            .UseInMemoryDatabase("DbInMemory")
            .Options;

        MockDbContext = new DtroContext(options);
    }

    public void Dispose()
    {
        MockDbContext.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }

    protected static dynamic? GetTestDataHelper<T>()
    {
        var type = typeof(T).ToString().Split('.').Last();
        return type switch
        {
            "DTRO" => new DtroTestDataHelper(),
            _ => null
        };
    }
}