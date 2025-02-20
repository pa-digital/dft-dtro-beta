using System.Linq;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class TestConfig
    {
        public static readonly string SchemaVersionUnderTest = "3.3.1";
        public static string EnvironmentName { get; }
        public static string BaseUri { get; }
        public static string AbsolutePathToProjectFolder { get; }
        public static string AbsolutePathToExamplesFolder { get; }
        public static string AbsolutePathToDtroExamplesFolder { get; }
        public static string AbsolutePathToDtroExamplesTempFolder { get; }
        public static string RulesJsonFile { get; }
        public static string SchemaJsonFile { get; }
        public static string DatabaseHostName { get; }
        public static string DatabaseConnectionString { get; }

        private static string GetAbsolutePathToProjectFolder()
        {
            DirectoryInfo currentDirectory = new(Directory.GetCurrentDirectory());

            var projectDirectory = "dft-dtro-beta";

            while (currentDirectory != null && currentDirectory.Name != projectDirectory)
            {
                currentDirectory = currentDirectory.Parent;
            }

            if (currentDirectory != null && currentDirectory.Name == projectDirectory)
            {
                return $"{currentDirectory.FullName}";
            }
            else
            {
                throw new Exception($"Directory '{projectDirectory}' not found in the current path hierarchy.");
            }
        }

        private static string GetDatabaseConnectionString()
        {
            string envFilePath = $"{AbsolutePathToProjectFolder}/docker/dev/.env";
            if (File.Exists(envFilePath))
            {
                var lines = File.ReadAllLines(envFilePath);
                var postgresUserLine = lines.First(line => line.StartsWith("POSTGRES_USER="));
                string postgresUser = postgresUserLine.Split('=')[1];
                var postgresPasswordLine = lines.First(line => line.StartsWith("POSTGRES_PASSWORD="));
                string postgresPassword = postgresPasswordLine.Split('=')[1];
                var postgresDbLine = lines.First(line => line.StartsWith("POSTGRES_DB="));
                string postgresDb = postgresDbLine.Split('=')[1];

                return $"Host={DatabaseHostName};Username={postgresUser};Password={postgresPassword};Database={postgresDb}";
            }
            else
            {
                return "Host=localhost;Username=postgres;Password=admin;Database=DTRO";
            }
        }

        static TestConfig()
        {
            EnvironmentName = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "local";

            switch (EnvironmentName)
            {
                case "local":
                    // BaseUri = "https://localhost:5001";
                    BaseUri = "https://127.0.0.1:5001";
                    DatabaseHostName = "localhost";
                    break;
                default:
                    throw new Exception($"Environment {EnvironmentName} not recognised!");
            }

            AbsolutePathToProjectFolder = GetAbsolutePathToProjectFolder();
            AbsolutePathToExamplesFolder = $"{GetAbsolutePathToProjectFolder()}/examples";
            AbsolutePathToDtroExamplesFolder = $"{AbsolutePathToExamplesFolder}/D-TROs/{SchemaVersionUnderTest}";
            AbsolutePathToDtroExamplesTempFolder = $"{AbsolutePathToExamplesFolder}/temp";
            RulesJsonFile = $"{AbsolutePathToExamplesFolder}/Rules/rules-{SchemaVersionUnderTest}.json";
            SchemaJsonFile = $"{AbsolutePathToExamplesFolder}/Schemas/schemas-{SchemaVersionUnderTest}.json";
            DatabaseConnectionString = GetDatabaseConnectionString();
        }
    }
}