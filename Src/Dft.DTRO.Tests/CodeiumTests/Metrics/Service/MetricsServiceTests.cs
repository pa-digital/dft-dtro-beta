using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using DfT.DTRO.DAL;
using DfT.DTRO.Models.Metrics;
using DfT.DTRO.Services;
using Google.Protobuf.WellKnownTypes;
namespace Dft.DTRO.Tests.CodeiumTests.Metrics.Service;

[ExcludeFromCodeCoverage]
public class MetricsServiceTests
{
    private MetricRequest _metricRequest = new MetricRequest() { DateFrom = new DateTime(2024, 1, 2), DateTo = new DateTime(2024, 1, 12) };

    [Fact]
    public async Task IncrementMetric_SystemFailure_ReturnsTrue()
    {
        // Arrange
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.SystemFailure, It.IsAny<int>())).ReturnsAsync(true);
        var service = new MetricsService(mockMetricDal.Object);

        // Act
        var result = await service.IncrementMetric(MetricType.SystemFailure, 123);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IncrementMetric_Submission_ReturnsTrue()
    {
        // Arrange
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.Submission, It.IsAny<int>())).ReturnsAsync(true);
        var service = new MetricsService(mockMetricDal.Object);

        // Act
        var result = await service.IncrementMetric(MetricType.Submission, 456);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IncrementMetric_Failure_ReturnsFalse()
    {
        // Arrange
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.Deletion, It.IsAny<int>())).ReturnsAsync(false);
        var service = new MetricsService(mockMetricDal.Object);

        // Act
        var result = await service.IncrementMetric(MetricType.Deletion, 789);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetMetricsForTra_ValidInputs_ReturnsMetricSummary()
    {
        // Arrange
        _metricRequest.TraId = 123;
        var expectedMetricSummary = new MetricSummary();

        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.GetMetricsForTra(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).ReturnsAsync(expectedMetricSummary);
        var service = new MetricsService(mockMetricDal.Object);

        // Act
        var result = await service.GetMetrics(_metricRequest);

        // Assert
        Assert.Equal(expectedMetricSummary, result);
    }

    [Fact]
    public async Task GetMetricsForTra_InvalidTraId_ReturnsNull()
    {
        // Arrange
        _metricRequest.TraId = -1;

        var mockMetricDal = new Mock<IMetricDal>();

        mockMetricDal.Setup(x => x.GetMetricsForTra(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).ReturnsAsync((MetricSummary)null);
        var service = new MetricsService(mockMetricDal.Object);

        // Act
        var result = await service.GetMetrics(_metricRequest);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CheckDataBase_ValidConnection_ReturnsTrue()
    {
        // Arrange
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.HasValidConnectionAsync()).ReturnsAsync(true);
        var service = new MetricsService(mockMetricDal.Object);

        // Act
        var result = await service.CheckDataBase();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CheckDataBase_InvalidConnection_ReturnsFalse()
    {
        // Arrange
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.HasValidConnectionAsync()).ReturnsAsync(false);
        var service = new MetricsService(mockMetricDal.Object);

        // Act
        var result = await service.CheckDataBase();

        // Assert
        Assert.False(result);
    }
}