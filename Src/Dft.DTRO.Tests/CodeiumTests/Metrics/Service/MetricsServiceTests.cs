using System.Diagnostics.CodeAnalysis;
using DfT.DTRO.DAL;
using DfT.DTRO.Models.Metrics;
using DfT.DTRO.Services;
namespace Dft.DTRO.Tests.CodeiumTests.Metrics.Service;

[ExcludeFromCodeCoverage]
public class MetricsServiceTests
{
    private MetricRequest _metricRequest = new MetricRequest() { DateFrom = new DateTime(2024, 1, 2), DateTo = new DateTime(2024, 1, 12) };

    [Fact]
    public async Task IncrementMetric_SystemFailure_ReturnsTrue()
    {
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.SystemFailure, It.IsAny<int>())).ReturnsAsync(true);
        var service = new MetricsService(mockMetricDal.Object);

        var result = await service.IncrementMetric(MetricType.SystemFailure, 123);

        Assert.True(result);
    }

    [Fact]
    public async Task IncrementMetric_Submission_ReturnsTrue()
    {
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.Submission, It.IsAny<int>())).ReturnsAsync(true);
        var service = new MetricsService(mockMetricDal.Object);

        var result = await service.IncrementMetric(MetricType.Submission, 456);

        Assert.True(result);
    }

    [Fact]
    public async Task IncrementMetric_Failure_ReturnsFalse()
    {
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.Deletion, It.IsAny<int>())).ReturnsAsync(false);
        var service = new MetricsService(mockMetricDal.Object);

        var result = await service.IncrementMetric(MetricType.Deletion, 789);

        Assert.False(result);
    }

    [Fact]
    public async Task GetMetricsForTra_ValidInputs_ReturnsMetricSummary()
    {
        _metricRequest.TraId = 123;
        var expectedMetricSummary = new MetricSummary();

        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.GetMetricsForTra(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).ReturnsAsync(expectedMetricSummary);
        var service = new MetricsService(mockMetricDal.Object);

        var result = await service.GetMetrics(_metricRequest);

        Assert.Equal(expectedMetricSummary, result);
    }

    [Fact]
    public async Task GetMetricsForTra_InvalidTraId_ReturnsNull()
    {
        _metricRequest.TraId = -1;

        var mockMetricDal = new Mock<IMetricDal>();

        mockMetricDal.Setup(x => x.GetMetricsForTra(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).ReturnsAsync((MetricSummary)null);
        var service = new MetricsService(mockMetricDal.Object);

        var result = await service.GetMetrics(_metricRequest);

        Assert.Null(result);
    }

    [Fact]
    public async Task CheckDataBase_ValidConnection_ReturnsTrue()
    {
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.HasValidConnectionAsync()).ReturnsAsync(true);
        var service = new MetricsService(mockMetricDal.Object);

        var result = await service.CheckDataBase();

        Assert.True(result);
    }

    [Fact]
    public async Task CheckDataBase_InvalidConnection_ReturnsFalse()
    {
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.HasValidConnectionAsync()).ReturnsAsync(false);
        var service = new MetricsService(mockMetricDal.Object);

        var result = await service.CheckDataBase();

        Assert.False(result);
    }
}