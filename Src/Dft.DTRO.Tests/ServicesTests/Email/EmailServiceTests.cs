namespace Dft.DTRO.Tests.ServicesTests.Email;

public class EmailServiceTests
{
    private readonly Mock<ISecretManagerClient> _mockSecretManagerClient = new();
    private readonly Mock<INotificationClient> _mockNotificationClient = new();

    private readonly string _testApiKey = $"testapikey-{Guid.NewGuid()}";
    private readonly string _testApplication = "testApplication";
    private ApigeeDeveloperApp _apigeeDeveloperApp = new();
    private readonly string _testEmail = "jon.doe@email.com";
    private readonly string _testId = Guid.NewGuid().ToString();

    private readonly Mock<IEmailService> _sut;

    public EmailServiceTests()
    {
        _mockSecretManagerClient
            .Setup(it => it.GetSecret("test-api-key"))
            .Returns(_testApiKey);

        _mockNotificationClient.As<INotificationClient>().Verify();

        _sut = new Mock<IEmailService>();
    }

    [Theory]
    [InlineData(nameof(ApplicationStatusType.Active.Status))]
    [InlineData(nameof(ApplicationStatusType.Inactive.Status))]
    public void SendEmailWhenApplicationCreated(string status)
    {
        _sut
            .Setup(it => it.SendEmail(_testApplication, _testEmail, status))
            .Returns(new EmailNotificationResponse(){ id = _testId });

        var actual = _sut.Object.SendEmail(_testApplication, _testEmail, status);
        Assert.NotNull(actual.id);
    }

    [Theory]
    [InlineData("FirstApplication","ben.pauley@dft.gov.uk")]
    [InlineData("SecondApplication","fareed.faisal@dft.gov.uk")]
    [InlineData("ThirdApplication","marcus.cumming@dft.gov.uk")]
    [InlineData("FourthApplication","gabriel.popescu@dft.gov.uk")]
    public void SendEmailReturnWhenRefreshSecrets(string applicationName, string requestEmail)
    {
        _apigeeDeveloperApp = new ApigeeDeveloperApp()
        {
            Name = applicationName,
            AppId = Guid.NewGuid().ToString(),
            Credentials = new List<ApigeeDeveloperAppCredential>()
            {
                new ApigeeDeveloperAppCredential()
                {
                    ConsumerKey = Guid.NewGuid().ToString(),
                    ConsumerSecret = Guid.NewGuid().ToString()
                }
            }
        };
        _sut
            .Setup(it => it.SendEmail(_apigeeDeveloperApp, requestEmail))
            .Returns(new EmailNotificationResponse() { id = _testId });
    }
}