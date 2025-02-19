namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public class UserDetails
    {
        public string AppId { get; }
        public string TraId { get; }
        public string Name { get; }
        public int UserGroup { get; }

        public UserDetails(string appId, string traId, string name, int userGroup)
        {
            AppId = appId;
            TraId = traId;
            Name = name;
            UserGroup = userGroup;
        }
    }

    public static class User
    {
        public static readonly UserDetails Publisher = new UserDetails("3fa85f64-5717-4562-b3fc-2c963f66afa6", "1050", "The publisher", 1);
        public static readonly UserDetails Consumer = new UserDetails("4f8b4a81-55ec-48cc-8ca2-2771c5490bd5", "", "The consumer", 2);
        public static readonly UserDetails Admin = new UserDetails("f553d1ec-a7ca-43d2-b714-60dacbb4d005", "", "Department for Transport", 3);
    }
}