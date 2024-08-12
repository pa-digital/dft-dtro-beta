namespace Dft.DTRO.Admin.Helpers;

public static class Helper
{
    public static void AddHeaders(ref HttpRequestMessage httpRequestMessage)
    {
        int ta = DftAdminTraId();
        httpRequestMessage.Headers.Add("ta", ta.ToString());
    }

    public static int DftAdminTraId()
    {
        return -1; //1585;
    }
}
