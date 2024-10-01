namespace Dft.DTRO.Tests.CodeiumTests.Metrics.Service;

[ExcludeFromCodeCoverage]
public class MetricsServiceTests
{
    private readonly MetricRequest _metricRequest = new()
    {
        DateFrom = new DateTime(2024, 1, 2),
        DateTo = new DateTime(2024, 1, 12),
        DtroUserId = Guid.NewGuid(),
        UserGroup = UserGroup.All
    };

    private readonly DtroUser _dtroUser = new();

    [Fact]
    public async Task IncrementMetric_SystemFailure_ReturnsTrue()
    {
        // Arrange
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.SystemFailure, It.IsAny<Guid>(), It.IsAny<UserGroup>()))
            .ReturnsAsync(true);

        var mockDtroUserDal = new Mock<IDtroUserDal>();
        mockDtroUserDal.Setup(x => x.GetDtroUserOnAppIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_dtroUser);

        var service = new MetricsService(mockMetricDal.Object, mockDtroUserDal.Object);

        // Act
        bool result = await service.IncrementMetric(MetricType.SystemFailure, Guid.NewGuid());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IncrementMetric_Submission_ReturnsTrue()
    {
        // Arrange
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.Submission, It.IsAny<Guid>(), It.IsAny<UserGroup>()))
            .ReturnsAsync(true);

        var mockDtroUserDal = new Mock<IDtroUserDal>();
        mockDtroUserDal.Setup(x => x.GetDtroUserOnAppIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_dtroUser);

        var service = new MetricsService(mockMetricDal.Object, mockDtroUserDal.Object);

        // Act
        bool result = await service.IncrementMetric(MetricType.Submission, Guid.NewGuid());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IncrementMetric_Failure_ReturnsFalse()
    {
        // Arrange
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.IncrementMetric(MetricType.Deletion, It.IsAny<Guid>(), It.IsAny<UserGroup>()))
            .ReturnsAsync(false);

        var mockDtroUserDal = new Mock<IDtroUserDal>();
        mockDtroUserDal.Setup(x => x.GetDtroUserOnAppIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_dtroUser);

        var service = new MetricsService(mockMetricDal.Object, mockDtroUserDal.Object);

        // Act
        bool result = await service.IncrementMetric(MetricType.Deletion, Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetMetricsForTra_ValidInputs_ReturnsMetricSummary()
    {
        // Arrange
        MetricSummary expectedMetricSummary = new();

        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.GetMetricsForDtroUser(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<UserGroup>()))
            .ReturnsAsync(expectedMetricSummary);

        var mockDtroUserDal = new Mock<IDtroUserDal>();
        mockDtroUserDal.Setup(x => x.GetDtroUserOnAppIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_dtroUser);

        var service = new MetricsService(mockMetricDal.Object, mockDtroUserDal.Object);

        // Act
        MetricSummary? result = await service.GetMetrics(_metricRequest);

        // Assert
        Assert.Equal(expectedMetricSummary, result);
    }

    [Fact]
    public async Task GetMetricsForTra_InvalidTraId_ReturnsNull()
    {
        // Arrange
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.GetMetricsForDtroUser(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<UserGroup>()))
            .ReturnsAsync(() => null);

        var mockDtroUserDal = new Mock<IDtroUserDal>();
        mockDtroUserDal.Setup(x => x.GetDtroUserOnAppIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_dtroUser);

        var service = new MetricsService(mockMetricDal.Object, mockDtroUserDal.Object);

        // Act
        MetricSummary? result = await service.GetMetrics(_metricRequest);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CheckDataBase_ValidConnection_ReturnsTrue()
    {
        // Arrange
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.HasValidConnectionAsync())
            .ReturnsAsync(true);

        var mockDtroUserDal = new Mock<IDtroUserDal>();
        mockDtroUserDal.Setup(x => x.GetDtroUserOnAppIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_dtroUser);

        var service = new MetricsService(mockMetricDal.Object, mockDtroUserDal.Object);

        // Act
        bool result = await service.CheckDataBase();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CheckDataBase_InvalidConnection_ReturnsFalse()
    {
        // Arrange
        var mockMetricDal = new Mock<IMetricDal>();
        mockMetricDal.Setup(x => x.HasValidConnectionAsync())
            .ReturnsAsync(false);

        var mockDtroUserDal = new Mock<IDtroUserDal>();
        mockDtroUserDal.Setup(x => x.GetDtroUserOnAppIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_dtroUser);

        var service = new MetricsService(mockMetricDal.Object, mockDtroUserDal.Object);


        // Act
        bool result = await service.CheckDataBase();

        // Assert
        Assert.False(result);
    }
}