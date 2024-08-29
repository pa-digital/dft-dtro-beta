namespace Dft.DTRO.Admin.Helpers;

public static class Helper
{
    public static void AddXAppIdHeader(ref HttpRequestMessage httpRequestMessage)
    {
        Guid xAppId = MyXAppId();
        httpRequestMessage.Headers.Add("x-app-id", xAppId.ToString());
    }

    public static Guid MyXAppId()
    {
        return new Guid("f553d1ec-a7ca-43d2-b714-60dacbb4d004");
       // return new Guid("f553d1ec-a7ca-43d2-b714-60dacbb4d005");
    }
}
