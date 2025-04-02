using System.Linq;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using DfT.DTRO.IntegrationTests.IntegrationTests.Helpers;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.Enums;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class TestConfig
    {
        public static EnvironmentType EnvironmentName { get; }

        public static string AdminClientId { get; }
        public static string AdminClientSecret { get; }
        public static string ConsumerClientId { get; }
        public static string ConsumerClientSecret { get; }
        public static string PublisherClientId { get; }
        public static string PublisherClientSecret { get; }
        public static string PublisherTraId { get; }

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
                string postgresUser = EnvVariables.GetEnvValue("POSTGRES_USER");
                string postgresPassword = EnvVariables.GetEnvValue("POSTGRES_PASSWORD");
                string postgresDb = EnvVariables.GetEnvValue("POSTGRES_DB");

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
                case "dev":
                    EnvironmentName = EnvironmentType.Dev;
                    BaseUri = "https://dtro-dev.dft.gov.uk/v1";
                    break;
                case "test":
                    EnvironmentName = EnvironmentType.Test;
                    BaseUri = "https://dtro-test.dft.gov.uk/v1";
                    break;
                default:
                    throw new Exception($"Environment {environmentFromBashCommand} not recognised!");
            }

            PathToProjectDirectory = GetAbsolutePathToProjectDirectory();
            PathToExamplesDirectory = $"{GetAbsolutePathToProjectDirectory()}/examples";
            PathToDtroExamplesDirectory = $"{PathToExamplesDirectory}/D-TROs";
            PathToDtroExamplesTempDirectory = $"{PathToExamplesDirectory}/temp_integration_tests";
            PathToSchemaExamplesDirectory = $"{PathToExamplesDirectory}/Schemas";
            PathToRuleExamplesDirectory = $"{PathToExamplesDirectory}/Rules";
            DatabaseConnectionString = GetDatabaseConnectionString();

            AdminClientId = EnvVariables.GetEnvValue("ADMIN_CLIENT_ID");
            AdminClientSecret = EnvVariables.GetEnvValue("ADMIN_CLIENT_SECRET");
            ConsumerClientId = EnvVariables.GetEnvValue("CONSUMER_CLIENT_ID");
            ConsumerClientSecret = EnvVariables.GetEnvValue("CONSUMER_CLIENT_SECRET");
            PublisherClientId = EnvVariables.GetEnvValue("PUBLISHER_CLIENT_ID");
            PublisherClientSecret = EnvVariables.GetEnvValue("PUBLISHER_CLIENT_SECRET");
            PublisherTraId = EnvVariables.GetEnvValue("PUBLISHER_TRA_ID");
        }
    }
}