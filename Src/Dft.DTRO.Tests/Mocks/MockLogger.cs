namespace Dft.DTRO.Tests.Mocks;

public static class MockLogger
{
    public static ILogger<T> Setup<T>()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        var factory = serviceProvider.GetService<ILoggerFactory>();
        var logger = factory?.CreateLogger<T>();

        return (logger ?? null) ?? throw new InvalidOperationException();
    }
}