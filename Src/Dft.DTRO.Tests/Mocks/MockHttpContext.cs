namespace Dft.DTRO.Tests.Mocks;

public static class MockHttpContext
{
    public static Mock<HttpContext> Setup()
    {
        var context = new Mock<HttpContext>();

        return context;
    }
}