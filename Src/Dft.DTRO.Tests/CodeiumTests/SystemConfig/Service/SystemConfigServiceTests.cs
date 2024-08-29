namespace Dft.DTRO.Tests.CodeiumTests.SystemConfig.Service;

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
    public async Task GetSystemConfigAsync_ReturnsSystemName()
    {
        // Arrange
        var expectedSystemName = "TestSystem";
        var systemConfig = new DfT.DTRO.Models.DataBase.SystemConfig { SystemName = expectedSystemName, IsTest = true }; // Make sure SystemConfig matches what you are using
        var systemConfigResponse = new SystemConfigResponse { SystemName = expectedSystemName, IsTest = systemConfig.IsTest };

        _mockSystemConfigDal.Setup(dal => dal.GetSystemConfigAsync())
            .ReturnsAsync(systemConfigResponse);

        // Act
        var result = await _service.GetSystemConfigAsync();

        // Assert
        Assert.Equal(expectedSystemName, result.SystemName);
        Assert.Equal(systemConfig.IsTest, result.IsTest); // Check other properties if needed
    }

}