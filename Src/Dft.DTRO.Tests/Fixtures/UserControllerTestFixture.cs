namespace Dft.DTRO.Tests.Fixtures;

[ExcludeFromCodeCoverage]
public class UserControllerTestFixture
{
    public ControllerContext ControllerContext { get; }

    public string UserId { get; set; }

    public UserControllerTestFixture()
    {
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(ctx => ctx.Items["UserId"]).Returns(UserId);

        ControllerContext = new ControllerContext
        {
            HttpContext = mockHttpContext.Object
        };
    }
}