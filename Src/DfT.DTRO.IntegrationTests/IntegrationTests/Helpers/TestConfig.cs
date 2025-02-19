namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class TestConfig
    {
        public static string EnvironmentName { get; }
        public static string BaseUri { get; }
        public static string AbsolutePathToExamplesFolder { get; }

        // TODO: Retrieve values from .env file
        public static readonly string DatabaseConnectionString = "Host=localhost;Username=postgres;Password=admin;Database=DTRO";
        public static readonly string SchemaVersionUnderTest = "3.3.1";

        private static string GetAbsolutePathToExamplesFolder()
        {
            DirectoryInfo currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            var projectDirectory = "dft-dtro-beta";

            while (currentDirectory != null && currentDirectory.Name != projectDirectory)
            {
                currentDirectory = currentDirectory.Parent;
            }

            if (currentDirectory != null && currentDirectory.Name == projectDirectory)
            {
                return $"{currentDirectory.FullName}/examples";
            }
            else
            {
                throw new Exception($"Directory '{projectDirectory}' not found in the current path hierarchy.");
            }
        }

        static TestConfig()
        {
            EnvironmentName = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "local";

            BaseUri = EnvironmentName switch
            {
                "local" => "https://localhost:5001",
                _ => throw new Exception($"Environment {EnvironmentName} not recognised!")
            };

            AbsolutePathToExamplesFolder = GetAbsolutePathToExamplesFolder();
        }
    }
}