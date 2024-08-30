namespace Dft.DTRO.Tests.CodeiumTests.SystemConfig.Service;

public class SystemConfigServiceTests
{
    private readonly Mock<ISystemConfigDal> _mockSystemConfigDal;
    private readonly Mock<IDtroUserDal> _mockDtroUserDal;
    private readonly SystemConfigService _service;

    public SystemConfigServiceTests()
    {
        _mockSystemConfigDal = new Mock<ISystemConfigDal>();
        _mockDtroUserDal = new Mock<IDtroUserDal>();
        _service = new SystemConfigService(_mockSystemConfigDal.Object, _mockDtroUserDal.Object);
    }
    [Fact]
    public async Task GetSystemConfigAsync_ReturnsSystemName()
    {
        // Arrange
        var expectedSystemName = "TestSystem";
        var systemConfig = new DfT.DTRO.Models.DataBase.SystemConfig { SystemName = expectedSystemName, IsTest = true }; // Make sure SystemConfig matches what you are using
        var systemConfigResponse = new SystemConfigResponse { SystemName = expectedSystemName, IsTest = systemConfig.IsTest };
        var userResponse = new DtroUserResponse { Id = Guid.NewGuid() };
        _mockSystemConfigDal.Setup(dal => dal.GetSystemConfigAsync())
            .ReturnsAsync(systemConfigResponse);

        _mockDtroUserDal.Setup(dal => dal.GetDtroUserByIdAsync(It.IsAny<Guid>()))
          .ReturnsAsync(userResponse);

        // Act
        var result = await _service.GetSystemConfigAsync(Guid.NewGuid());

        // Assert
        Assert.Equal(expectedSystemName, result.SystemName);
        Assert.Equal(systemConfig.IsTest, result.IsTest); // Check other properties if needed
    }

}