namespace Dft.DTRO.Admin.Services;
public class XappIdService : IXappIdService
{
    private Guid _xAppId;

    public XappIdService(IConfiguration configuration)
    {
        var xAppIdEnv = Environment.GetEnvironmentVariable("X_APP_ID");

        if (!string.IsNullOrEmpty(xAppIdEnv) && Guid.TryParse(xAppIdEnv, out Guid envXAppId))
        {
            _xAppId = envXAppId;
        }
        else
        {
            var xAppIdConfigValue = configuration.GetValue<string>("X_APP_ID");

            if (!string.IsNullOrEmpty(xAppIdConfigValue) && Guid.TryParse(xAppIdConfigValue, out Guid configXAppId))
            {
                _xAppId = configXAppId;
            }
            else
            {
                throw new InvalidOperationException("XAppId must be provided in either environment variables or configuration.");
            }
        }
    }

    public void AddXAppIdHeader(ref HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Add("x-app-id", _xAppId.ToString());
    }

    public Guid MyXAppId()
    {
        return _xAppId;
    }

    public void ChangeXAppId(Guid guid)
    {
        _xAppId = guid;
    }
}
