using System.Linq;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class TestConfig
    {
        public static EnvironmentType EnvironmentName { get; }
        public static string BaseUri { get; }
        public static string PathToProjectDirectory { get; }
        public static string PathToExamplesDirectory { get; }
        public static string PathToDtroExamplesDirectory { get; }
        public static string PathToDtroExamplesTempDirectory { get; }
        public static string PathToSchemaExamplesDirectory { get; }
        public static string PathToRuleExamplesDirectory { get; }
        public static string RulesJsonFile { get; }
        public static string SchemaJsonFile { get; }
        public static string DatabaseHostName { get; }
        public static string DatabaseConnectionString { get; }

        private static string GetAbsolutePathToProjectDirectory()
        {
            DirectoryInfo currentDirectory = new(Directory.GetCurrentDirectory());

            string projectDirectory = "dft-dtro-beta";

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
            string envFilePath = $"{PathToProjectDirectory}/docker/dev/.env";
            if (File.Exists(envFilePath))
            {
                string[] lines = File.ReadAllLines(envFilePath);
                string postgresUserLine = lines.First(line => line.StartsWith("POSTGRES_USER="));
                string postgresUser = postgresUserLine.Split('=')[1];
                string postgresPasswordLine = lines.First(line => line.StartsWith("POSTGRES_PASSWORD="));
                string postgresPassword = postgresPasswordLine.Split('=')[1];
                string postgresDbLine = lines.First(line => line.StartsWith("POSTGRES_DB="));
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
            string environmentFromBashCommand = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "local";

            switch (environmentFromBashCommand)
            {
                case "local":
                    EnvironmentName = EnvironmentType.Local;
                    BaseUri = "https://localhost:5001";
                    DatabaseHostName = "localhost";
                    break;
                default:
                    throw new Exception($"Environment {EnvironmentName} not recognised!");
            }

            PathToProjectDirectory = GetAbsolutePathToProjectDirectory();
            PathToExamplesDirectory = $"{GetAbsolutePathToProjectDirectory()}/examples";
            PathToDtroExamplesDirectory = $"{PathToExamplesDirectory}/D-TROs";
            PathToDtroExamplesTempDirectory = $"{PathToExamplesDirectory}/temp_integration_tests";
            PathToSchemaExamplesDirectory = $"{PathToExamplesDirectory}/Schemas";
            PathToRuleExamplesDirectory = $"{PathToExamplesDirectory}/Rules";
            DatabaseConnectionString = GetDatabaseConnectionString();
        }
    }
}