namespace Dft.DTRO.Tests.CodeiumTests.Users.Service;

public class UserServiceTests
{
    private readonly Mock<IDtroUserDal> _mockDtroUserDal;
    private readonly IDtroUserService _sut;
    private readonly Mock<IMetricDal> _mockMetricsDal;
    public UserServiceTests()
    {
        _mockDtroUserDal = new Mock<IDtroUserDal>();
        _mockMetricsDal = new Mock<IMetricDal>();
        _sut = new DtroUserService(_mockDtroUserDal.Object, _mockMetricsDal.Object);
    }

    [Fact]
    public async Task SearchDtroUsersAsync_ByPartialName_ShouldReturnUsers()
    {
        // Arrange
        var partialName = "test";
        var expectedUsers = new List<DtroUserResponse>
        {
            new() { Name = "test name" },
            new() { Name = "(test) another name" },
            new() { Name = "SHORT NAME" }
        };

        _mockDtroUserDal.Setup(dal => dal.SearchDtroUsersAsync(partialName))
            .ReturnsAsync(expectedUsers);

        // Act
        var actual = await _sut.SearchDtroUsers(partialName);

        // Assert
        Assert.Equal(expectedUsers[0].Name, actual[0].Name);
        Assert.Equal(expectedUsers[1].Name, actual[1].Name);
        Assert.Equal(expectedUsers[2].Name, actual[2].Name);
    }

    [Fact]
    public async Task GetAllDtroUsersAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var expectedUsers = new List<DtroUserResponse>
        {
            new() { Name = "example name" },
            new() { Name = "another EXAMPLE" }
        };

        _mockDtroUserDal.Setup(dal => dal.GetAllDtroUsersAsync())
            .ReturnsAsync(expectedUsers);

        // Act
        var result = await _sut.GetAllDtroUsersAsync();

        // Assert
        Assert.Equal(expectedUsers[0].Name, result[0].Name);
        Assert.Equal(expectedUsers[1].Name, result[1].Name);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SaveDtroUserAsync_ShouldReturnGuidResponse(bool doesTraIdExists)
    {
        // Arrange
        Guid sameId = Guid.NewGuid();
        var request = new DtroUserRequest { TraId = 1, Id = sameId };
        var response = new GuidResponse { Id = sameId };

        _mockDtroUserDal.Setup(dal => dal.TraExistsAsync((int)request.TraId))
            .ReturnsAsync(doesTraIdExists);

        _mockDtroUserDal.Setup(dal => dal.SaveDtroUserAsync(request))
            .ReturnsAsync(response);

        // Act
        var actual = await _sut.SaveDtroUserAsync(request);

        // Assert
        Assert.IsAssignableFrom<GuidResponse>(actual);
        Assert.Equal(response.Id, actual.Id);
    }

    [Fact]
    public async Task UpdateDtroUserAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var swaCodeRequest = new DtroUserRequest { TraId = 1 };

        _mockDtroUserDal
            .Setup(dal => dal.TraExistsAsync((int)swaCodeRequest.TraId))
            .ReturnsAsync(false);

        _mockDtroUserDal
            .Setup(dal => dal.UpdateDtroUserAsync(swaCodeRequest))
            .ThrowsAsync(new NotFoundException());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateDtroUserAsync(swaCodeRequest));
    }

    [Fact]
    public async Task UpdateDtroUserAsync_ShouldReturnGuidResponse_WhenUserExists()
    {
        // Arrange
        var swaCodeRequest = new DtroUserRequest { TraId = 1 };
        var response = new GuidResponse { Id = Guid.NewGuid() };

        _mockDtroUserDal.Setup(dal => dal.TraExistsAsync((int)swaCodeRequest.TraId))
            .ReturnsAsync(true);

        _mockDtroUserDal.Setup(dal => dal.GetDtroUserByIdAsync(It.IsAny<Guid>()))
           .ReturnsAsync(new DtroUserResponse() { Id = Guid.NewGuid(), UserGroup = UserGroup.All });

        _mockDtroUserDal.Setup(dal => dal.UpdateDtroUserAsync(swaCodeRequest))
            .ReturnsAsync(response);

        // Act
        var result = await _sut.UpdateDtroUserAsync(swaCodeRequest);

        // Assert
        Assert.Equal(response.Id, result.Id);
    }
}
