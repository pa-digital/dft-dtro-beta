using System.Diagnostics.CodeAnalysis;
using DfT.DTRO.Models.DtroHistory;
using DfT.DTRO.Services.Conversion;
using DfT.DTRO.Services.Mapping;
using Microsoft.Extensions.Configuration;

namespace Dft.DTRO.Tests;

[ExcludeFromCodeCoverage]
public class MappingServiceTests
{
    private static readonly string[] ValidDtroHistories =
    {
        "./DtroJsonDataExamples/v3.2.0/valid-new-x.json",
        "./DtroJsonDataExamples/v3.2.0/valid-noChange.json",
        "./DtroJsonDataExamples/v3.2.0/valid-fullRevoke.json"
    };

    private readonly IDtroMappingService _sut;

    public MappingServiceTests()
    {
        Dictionary<string, string> dictionary = new() { { "Key1", "Value1" } };
        IConfigurationRoot? mockConfiguration = new ConfigurationBuilder().AddInMemoryCollection(dictionary).Build();
        Mock<ISpatialProjectionService> mockSpatialProjectionService = new();

        _sut = new DtroMappingService(mockConfiguration, mockSpatialProjectionService.Object);
    }

    [Fact]
    public async Task StripProvisions_Returns_DtroHistoryResponse()
    {
        List<DtroHistoryRequest> requests = await CreateRequestDtroHistoryObject(ValidDtroHistories);
        List<DtroHistoryResponse> actual = requests.Select(_sut.StripProvision).Where(response => response != null).ToList();

        Assert.True(actual.Any());

        Assert.Equal(2, actual.Count);

        Assert.Equal(actual[0].Created, actual[1].Created);

        Assert.True(actual[1].LastUpdated > actual[0].LastUpdated);
    }
}