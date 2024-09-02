namespace Dft.DTRO.Admin.Services;
public interface IXappIdService
{
    void AddXAppIdHeader(ref HttpRequestMessage httpRequestMessage);
    Guid MyXAppId();

    void ChangeXAppId(Guid guid);
}