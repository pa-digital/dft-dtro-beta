namespace Dft.DTRO.Admin.Helpers;

public static class Helper
{
    public static void AddHeaders(ref HttpRequestMessage httpRequestMessage)
    {
        var xAppId = new Guid("f553d1ec-a7ca-43d2-b714-60dacbb4d004");
        httpRequestMessage.Headers.Add("x-app-id", xAppId.ToString());
    }

    public static int DftAdminTraId()
    {
        return -1; //1585;
    }
}
