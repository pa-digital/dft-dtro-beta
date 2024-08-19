namespace Dft.DTRO.Tests.CodeiumTests.Tra.Service;

public class TraServiceTests
{
    private readonly Mock<IDtroUserDal> _mockSwaCodeDal;
    private readonly DtroUserService _traService;

    public TraServiceTests()
    {
        _mockSwaCodeDal = new Mock<IDtroUserDal>();
        _traService = new DtroUserService(_mockSwaCodeDal.Object);
    }

    [Fact]
    public async Task SearchSwaCodes_ShouldFormatNamesCorrectly()
    {
        // Arrange
        var partialName = "test";
        var swaCodeResponses = new List<DtroUserResponse>
        {
            new DtroUserResponse { Name = "test name" },
            new DtroUserResponse { Name = "(test) another name" },
            new DtroUserResponse { Name = "SHORT NAME" }
        };

        _mockSwaCodeDal.Setup(dal => dal.SearchDtroUsersAsync(partialName))
            .ReturnsAsync(swaCodeResponses);

        // Act
        var result = await _traService.SearchDtroUsers(partialName);

        // Assert
        Assert.Equal("Test Name", result[0].Name);
        Assert.Equal("(test) Another Name", result[1].Name);
        Assert.Equal("Short Name", result[2].Name);
    }

    [Fact]
    public async Task GetSwaCodeAsync_ShouldFormatNamesCorrectly()
    {
        // Arrange
        var swaCodeResponses = new List<DtroUserResponse>
        {
            new DtroUserResponse { Name = "example name" },
            new DtroUserResponse { Name = "another EXAMPLE" }
        };

        _mockSwaCodeDal.Setup(dal => dal.GetAllDtroUsersAsync())
            .ReturnsAsync(swaCodeResponses);

        // Act
        var result = await _traService.GetAllDtroUsersAsync();

        // Assert
        Assert.Equal("Example Name", result[0].Name);
        Assert.Equal("Another Example", result[1].Name);
    }

    [Fact]
    public async Task SaveTraAsync_ShouldThrowInvalidOperationException_WhenTraExists()
    {
        // Arrange
        var swaCodeRequest = new DtroUserRequest { TraId = 1 };
        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync((int) swaCodeRequest.TraId))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _traService.SaveDtroUserAsync(swaCodeRequest));
    }

    [Fact]
    public async Task SaveTraAsync_ShouldReturnGuidResponse_WhenTraDoesNotExist()
    {
        // Arrange
        var swaCodeRequest = new DtroUserRequest { TraId = 1 };
        var response = new GuidResponse { Id = Guid.NewGuid() };

        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync((int) swaCodeRequest.TraId))
            .ReturnsAsync(false);
        _mockSwaCodeDal.Setup(dal => dal.SaveDtroUserAsync(swaCodeRequest))
            .ReturnsAsync(response);

        // Act
        var result = await _traService.SaveDtroUserAsync(swaCodeRequest);

        // Assert
        Assert.Equal(response.Id, result.Id);
    }

    [Fact]
    public async Task UpdateTraAsync_ShouldThrowNotFoundException_WhenTraDoesNotExist()
    {
        // Arrange
        var swaCodeRequest = new DtroUserRequest { TraId = 1 };
        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync((int) swaCodeRequest.TraId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _traService.UpdateDtroUserAsync(swaCodeRequest));
    }

    [Fact]
    public async Task UpdateTraAsync_ShouldReturnGuidResponse_WhenTraExists()
    {
        // Arrange
        var swaCodeRequest = new DtroUserRequest { TraId = 1 };
        var response = new GuidResponse { Id = Guid.NewGuid() };

        _mockSwaCodeDal.Setup(dal => dal.TraExistsAsync((int) swaCodeRequest.TraId))
            .ReturnsAsync(true);
        _mockSwaCodeDal.Setup(dal => dal.UpdateDtroUserAsync(swaCodeRequest))
            .ReturnsAsync(response);

        // Act
        var result = await _traService.UpdateDtroUserAsync(swaCodeRequest);

        // Assert
        Assert.Equal(response.Id, result.Id);
    }
}
