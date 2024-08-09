using Xunit;
using Moq;
using System.Threading.Tasks;
using DfT.DTRO.Services;
using DfT.DTRO.Models;

public class SystemConfigServiceTests
{
    private readonly Mock<ISystemConfigDal> _mockSystemConfigDal;
    private readonly SystemConfigService _service;

    public SystemConfigServiceTests()
    {
        _mockSystemConfigDal = new Mock<ISystemConfigDal>();
        _service = new SystemConfigService(_mockSystemConfigDal.Object);
    }

    [Fact]
    public async Task GetSystemNameAsync_ReturnsSystemName()
    {
        // Arrange
        var expectedSystemName = "TestSystem";
        var systemConfig = new SystemConfig { SystemName = expectedSystemName };
        _mockSystemConfigDal.Setup(dal => dal.GetSystemConfigAsync())
            .ReturnsAsync(systemConfig);

        // Act
        var result = await _service.GetSystemNameAsync();

        // Assert
        Assert.Equal(expectedSystemName, result);
    }
}
