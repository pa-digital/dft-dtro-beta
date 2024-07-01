using System.Diagnostics.CodeAnalysis;
using DfT.DTRO.DAL;
using DfT.DTRO.Models.Metrics;
using DfT.DTRO.Services;
namespace Dft.DTRO.Tests.CodeiumTests.Metrics.Service;

[ExcludeFromCodeCoverage]
public class MetricsServiceTests
{
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
        int traId = 123;
        DateOnly fromDate = new DateOnly(2022, 1, 1);
        DateOnly toDate = new DateOnly(2022, 1, 31);
        var expectedMetricSummary = new MetricSummary();

        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.GetMetricsForTra(traId, fromDate, toDate)).ReturnsAsync(expectedMetricSummary);
        var service = new MetricsService(mockMetricDal.Object);

        // Act
        var result = await service.GetMetricsForTra(traId, fromDate, toDate);

        // Assert
        Assert.Equal(expectedMetricSummary, result);
    }

    [Fact]
    public async Task GetMetricsForTra_InvalidTraId_ReturnsNull()
    {
        // Arrange
        int traId = -1;
        DateOnly fromDate = new DateOnly(2022, 1, 1);
        DateOnly toDate = new DateOnly(2022, 1, 31);

        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.GetMetricsForTra(traId, fromDate, toDate)).ReturnsAsync((MetricSummary)null);
        var service = new MetricsService(mockMetricDal.Object);

        // Act
        var result = await service.GetMetricsForTra(traId, fromDate, toDate);

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