﻿namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class MappingServiceTests
{
    private static readonly string[] ValidDtroHistories =
    {
        "../../../TestFiles/D-TROs/3.2.3/temporary TRO - new-Polygon.json",
        "../../../TestFiles/D-TROs/3.2.3/valid-noChange.json",
        "../../../TestFiles/D-TROs/3.2.3/temporary TRO - fullRevoke.json"
    };

    private readonly IDtroMappingService _sut;

    public MappingServiceTests()
    {
        Dictionary<string, string> dictionary = new() { { "Key1", "Value1" } };
        IConfigurationRoot? mockConfiguration = new ConfigurationBuilder().AddInMemoryCollection(dictionary).Build();
        Mock<IBoundingBoxService> mockBoundingBoxService = new();

        _sut = new DtroMappingService(mockConfiguration, mockBoundingBoxService.Object);
    }

    [Fact]
    public void GetSource_Returns_DtroHistoryResponse()
    {
        List<DTROHistory> histories = Utils.CreateRequestDtroHistoryObject(ValidDtroHistories);
        List<DtroHistorySourceResponse> actual = histories
            .Select(_sut.GetSource)
            .Where(response => response != null)
            .ToList();

        Assert.True(actual.Any());

        Assert.Equal(2, actual.Count);

        Assert.Equal(actual[0].Created, actual[1].Created);

        Assert.True(actual[1].LastUpdated > actual[0].LastUpdated);
    }

    [Fact]
    public void GetProvision_Returns_DtroHistoryProvision()
    {
        List<DTROHistory> histories = Utils.CreateRequestDtroHistoryObject(ValidDtroHistories);
        List<DtroHistoryProvisionResponse> actual = histories
            .SelectMany(_sut.GetProvision)
            .Where(response => response != null)
            .ToList();

        Assert.True(actual.Any());

        Assert.Equal(2, actual.Count);

        Assert.Equal(actual[0].Created, actual[1].Created);

        Assert.True(actual[1].LastUpdated > actual[0].LastUpdated);
    }
}