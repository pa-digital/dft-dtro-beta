﻿namespace Dft.DTRO.Tests.ServicesTests.Email;

public class EmailServiceTests
{
    private readonly Mock<ISecretManagerClient> _mockSecretManagerClient = new();
    private readonly Mock<INotificationClient> _mockNotificationClient = new();

    private readonly string _testApiKey = $"testapikey-{Guid.NewGuid()}";
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
    [InlineData("First Application",nameof(ApplicationStatusType.Active.Status))]
    [InlineData("Second Application",nameof(ApplicationStatusType.Active.Status))]
    [InlineData("Third Application",nameof(ApplicationStatusType.Active.Status))]
    [InlineData("Fourth Application",nameof(ApplicationStatusType.Active.Status))]
    [InlineData("First Application",nameof(ApplicationStatusType.Inactive.Status))]
    [InlineData("Second Application",nameof(ApplicationStatusType.Inactive.Status))]
    [InlineData("Third Application",nameof(ApplicationStatusType.Inactive.Status))]
    [InlineData("Fourth Application",nameof(ApplicationStatusType.Inactive.Status))]
    public void SendEmailWhenApplicationCreated(string application, string status)
    {
        _sut
            .Setup(it => it.SendEmail(application, _testEmail, status))
            .Returns(new EmailNotificationResponse(){ id = _testId });

        var actual = _sut.Object.SendEmail(application, _testEmail, status);
        Assert.NotNull(actual.id);
    }

    [Theory]
    [InlineData("ben.pauley@dft.gov.uk","FirstApplication" )]
    [InlineData("fareed.faisal@dft.gov.uk","SecondApplication")]
    [InlineData("marcus.cumming@dft.gov.uk","ThirdApplication" )]
    [InlineData("gabriel.popescu@dft.gov.uk","FourthApplication")]
    public void SendEmailReturnWhenRefreshSecrets(string requestEmail, string applicationName)
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
            .Setup(it => it.SendEmail(requestEmail, _apigeeDeveloperApp))
            .Returns(new EmailNotificationResponse() { id = _testId });

        var actual = _sut.Object.SendEmail(requestEmail, _apigeeDeveloperApp);
        Assert.NotNull(actual.id);

    }
}