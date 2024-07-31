
namespace Dft.DTRO.Tests.CodeiumTests.Tra.Service;
public class TraServiceTests
{
    private readonly Mock<ISwaCodeDal> _mockSwaCodeDal;
    private readonly TraService _traService;

    public TraServiceTests()
    {
        _mockSwaCodeDal = new Mock<ISwaCodeDal>();
        _traService = new TraService(_mockSwaCodeDal.Object);
    }

    [Fact]
    public async Task SearchSwaCodes_ShouldFormatNamesCorrectly()
    {
        // Arrange
        var partialName = "test";
        var swaCodeResponses = new List<SwaCodeResponse>
        {
            new SwaCodeResponse { Name = "test name" },
            new SwaCodeResponse { Name = "(test) another name" },
            new SwaCodeResponse { Name = "SHORT NAME" }
        };

        _mockSwaCodeDal.Setup(dal => dal.SearchSwaCodesAsync(partialName))
            .ReturnsAsync(swaCodeResponses);

        // Act
        var result = await _traService.SearchSwaCodes(partialName);

        // Assert
        Assert.Equal("Test Name", result[0].Name);
        Assert.Equal("(test) Another Name", result[1].Name);
        Assert.Equal("Short Name", result[2].Name);
    }

    [Fact]
    public async Task GetSwaCodeAsync_ShouldFormatNamesCorrectly()
    {
        // Arrange
        var swaCodeResponses = new List<SwaCodeResponse>
        {
            new SwaCodeResponse { Name = "example name" },
            new SwaCodeResponse { Name = "another EXAMPLE" }
        };

        _mockSwaCodeDal.Setup(dal => dal.GetAllCodesAsync())
            .ReturnsAsync(swaCodeResponses);

        // Act
        var result = await _traService.GetSwaCodeAsync();

        // Assert
        Assert.Equal("Example Name", result[0].Name);
        Assert.Equal("Another Example", result[1].Name);
    }

    [Fact]
    public async Task ActivateTraAsync_ShouldThrowNotFoundException_WhenTraDoesNotExist()
    {
        // Arrange
        var traId = 1;
        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync(traId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _traService.ActivateTraAsync(traId));
    }

    [Fact]
    public async Task ActivateTraAsync_ShouldReturnGuidResponse_WhenTraExists()
    {
        // Arrange
        var traId = 1;
        var response = new GuidResponse { Id = Guid.NewGuid() };

        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync(traId))
            .ReturnsAsync(true);
        _mockSwaCodeDal.Setup(dal => dal.ActivateTraAsync(traId))
            .ReturnsAsync(response);

        // Act
        var result = await _traService.ActivateTraAsync(traId);

        // Assert
        Assert.Equal(response.Id, result.Id);
    }

    [Fact]
    public async Task DeActivateTraAsync_ShouldThrowNotFoundException_WhenTraDoesNotExist()
    {
        // Arrange
        var traId = 1;
        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync(traId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _traService.DeActivateTraAsync(traId));
    }

    [Fact]
    public async Task DeActivateTraAsync_ShouldReturnGuidResponse_WhenTraExists()
    {
        // Arrange
        var traId = 1;
        var response = new GuidResponse { Id = Guid.NewGuid() };

        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync(traId))
            .ReturnsAsync(true);
        _mockSwaCodeDal.Setup(dal => dal.DeActivateTraAsync(traId))
            .ReturnsAsync(response);

        // Act
        var result = await _traService.DeActivateTraAsync(traId);

        // Assert
        Assert.Equal(response.Id, result.Id);
    }

    [Fact]
    public async Task SaveTraAsync_ShouldThrowInvalidOperationException_WhenTraExists()
    {
        // Arrange
        var swaCodeRequest = new SwaCodeRequest { TraId = 1 };
        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync(swaCodeRequest.TraId))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _traService.SaveTraAsync(swaCodeRequest));
    }

    [Fact]
    public async Task SaveTraAsync_ShouldReturnGuidResponse_WhenTraDoesNotExist()
    {
        // Arrange
        var swaCodeRequest = new SwaCodeRequest { TraId = 1 };
        var response = new GuidResponse { Id = Guid.NewGuid() };

        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync(swaCodeRequest.TraId))
            .ReturnsAsync(false);
        _mockSwaCodeDal.Setup(dal => dal.SaveTraAsync(swaCodeRequest))
            .ReturnsAsync(response);

        // Act
        var result = await _traService.SaveTraAsync(swaCodeRequest);

        // Assert
        Assert.Equal(response.Id, result.Id);
    }

    [Fact]
    public async Task UpdateTraAsync_ShouldThrowNotFoundException_WhenTraDoesNotExist()
    {
        // Arrange
        var swaCodeRequest = new SwaCodeRequest { TraId = 1 };
        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync(swaCodeRequest.TraId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _traService.UpdateTraAsync(swaCodeRequest));
    }

    [Fact]
    public async Task UpdateTraAsync_ShouldReturnGuidResponse_WhenTraExists()
    {
        // Arrange
        var swaCodeRequest = new SwaCodeRequest { TraId = 1 };
        var response = new GuidResponse { Id = Guid.NewGuid() };

        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync(swaCodeRequest.TraId))
            .ReturnsAsync(true);
        _mockSwaCodeDal.Setup(dal => dal.UpdateTraAsync(swaCodeRequest))
            .ReturnsAsync(response);

        // Act
        var result = await _traService.UpdateTraAsync(swaCodeRequest);

        // Assert
        Assert.Equal(response.Id, result.Id);
    }
}
