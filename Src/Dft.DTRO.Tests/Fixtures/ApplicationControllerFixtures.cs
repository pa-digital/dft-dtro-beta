namespace Dft.DTRO.Tests.Fixtures
{
    public class ApplicationControllerTestFixture
    {
        public ControllerContext ControllerContext { get; }

        public ApplicationControllerTestFixture()
        {
            var userId = "user@test.com";

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(ctx => ctx.Items["UserId"]).Returns(userId);

            ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };
        }
    }
}
