namespace Dft.DTRO.Tests.RepositoryTests;

[ExcludeFromCodeCoverage]
public class DtroTestDataHelper : ITestDataHelper<DfT.DTRO.Models.DataBase.DTRO>
{
    public Task<DfT.DTRO.Models.DataBase.DTRO> GenerateSingleEntry(DtroContext context)
    {
        throw new NotImplementedException();

    }

    public Task<IEnumerable<DfT.DTRO.Models.DataBase.DTRO>> GenerateMultipleEntries(DtroContext context)
    {
        throw new NotImplementedException();
    }
}