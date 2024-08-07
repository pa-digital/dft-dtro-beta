﻿namespace Dft.DTRO.Tests.CodeiumTests.Metrics.Service;

[ExcludeFromCodeCoverage]
public class MetricsServiceTests
{
    private readonly MetricRequest _metricRequest =
        new() { DateFrom = new DateTime(2024, 1, 2), DateTo = new DateTime(2024, 1, 12) };

    [Fact]
    public async Task IncrementMetric_SystemFailure_ReturnsTrue()
    {
        Mock<IMetricDal> mockMetricDal = new();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.SystemFailure, It.IsAny<int>())).ReturnsAsync(true);
        MetricsService service = new(mockMetricDal.Object);

        bool result = await service.IncrementMetric(MetricType.SystemFailure, 123);

        Assert.True(result);
    }

    [Fact]
    public async Task IncrementMetric_Submission_ReturnsTrue()
    {
        Mock<IMetricDal> mockMetricDal = new();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.Submission, It.IsAny<int>())).ReturnsAsync(true);
        MetricsService service = new(mockMetricDal.Object);

        bool result = await service.IncrementMetric(MetricType.Submission, 456);

        Assert.True(result);
    }

    [Fact]
    public async Task IncrementMetric_Failure_ReturnsFalse()
    {
        Mock<IMetricDal> mockMetricDal = new();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.Deletion, It.IsAny<int>())).ReturnsAsync(false);
        MetricsService service = new(mockMetricDal.Object);

        bool result = await service.IncrementMetric(MetricType.Deletion, 789);

        Assert.False(result);
    }

    [Fact]
    public async Task GetMetricsForTra_ValidInputs_ReturnsMetricSummary()
    {
        _metricRequest.TraId = 123;
        MetricSummary expectedMetricSummary = new();

        Mock<IMetricDal> mockMetricDal = new();
        mockMetricDal.Setup(x => x.GetMetricsForTra(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(expectedMetricSummary);
        MetricsService service = new(mockMetricDal.Object);

        MetricSummary? result = await service.GetMetrics(_metricRequest);

        Assert.Equal(expectedMetricSummary, result);
    }

    [Fact]
    public async Task GetMetricsForTra_InvalidTraId_ReturnsNull()
    {
        _metricRequest.TraId = -1;

        Mock<IMetricDal> mockMetricDal = new();

        mockMetricDal.Setup(x => x.GetMetricsForTra(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(() => null);
        MetricsService service = new(mockMetricDal.Object);

        MetricSummary? result = await service.GetMetrics(_metricRequest);

        Assert.Null(result);
    }

    [Fact]
    public async Task CheckDataBase_ValidConnection_ReturnsTrue()
    {
        Mock<IMetricDal> mockMetricDal = new();
        mockMetricDal.Setup(x => x.HasValidConnectionAsync()).ReturnsAsync(true);
        MetricsService service = new(mockMetricDal.Object);

        bool result = await service.CheckDataBase();

        Assert.True(result);
    }

    [Fact]
    public async Task CheckDataBase_InvalidConnection_ReturnsFalse()
    {
        Mock<IMetricDal> mockMetricDal = new();
        mockMetricDal.Setup(x => x.HasValidConnectionAsync()).ReturnsAsync(false);
        MetricsService service = new(mockMetricDal.Object);

        bool result = await service.CheckDataBase();

        Assert.False(result);
    }
}