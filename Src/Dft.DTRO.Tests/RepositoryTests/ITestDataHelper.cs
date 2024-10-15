namespace Dft.DTRO.Tests.RepositoryTests;

public interface ITestDataHelper<T>
{
    Task<T> GenerateSingleEntry(DtroContext context);

    Task<IEnumerable<T>> GenerateMultipleEntries(DtroContext context);
}