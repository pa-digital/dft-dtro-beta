namespace Dft.DTRO.Admin.Services;
public interface IXappIdService
{
    Task<bool> AddXAppIdHeader(HttpRequestMessage httpRequestMessage);
    Guid MyXAppId();

    void ChangeXAppId(Guid guid);
}