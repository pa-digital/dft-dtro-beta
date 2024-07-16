
public static class Helper
{
    public static void AddHeaders(ref HttpRequestMessage httpRequestMessage)
    {
        int ta = TraId();
        httpRequestMessage.Headers.Add("ta", ta.ToString());
    }

    public static int TraId()
    {
        return 1585;
    }
}
