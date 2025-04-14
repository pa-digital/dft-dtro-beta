#nullable enable

using System.Linq;
using Newtonsoft.Json.Linq;
using DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Helpers
{
    public static class EnvVariables
    {
        private static Dictionary<string, string>? _envVariables;

        public static string? GetEnvValue(string key)
        {
            // First, try to get the value from environment variables
            var value = Environment.GetEnvironmentVariable(key);

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            // If not found, try to read from .env file
            if (_envVariables == null)
            {
                LoadEnvFile();
            }

            if (_envVariables != null && _envVariables.TryGetValue(key, out var envValue))
            {
                return envValue;
            }
            else
            {
                throw new InvalidOperationException($"Environment variable '{key}' not found. When running the tests against dev / test / integration, make sure you've supplied '{key}' as an argument passed to run_api_tests.sh or in docker/dev/.env");
            }
        }

        private static void LoadEnvFile()
        {
            _envVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            string envFilePath = $"{PathToProjectDirectory}/docker/dev/.env";

            if (!File.Exists(envFilePath)) return;

            foreach (var line in File.ReadAllLines(envFilePath))
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#")) continue;

                var parts = trimmedLine.Split('=', 2);
                if (parts.Length == 2)
                {
                    _envVariables[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }
    }
}