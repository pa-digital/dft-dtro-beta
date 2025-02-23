using Npgsql;
using static DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.TestConfig;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class SqlQueries
    {
        public static void TruncateTable(string tableName)
        {
            string connectionString = DatabaseConnectionString;

            string truncateQuery = $"TRUNCATE TABLE public.\"{tableName}\" RESTART IDENTITY CASCADE;";

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using var command = new NpgsqlCommand(truncateQuery, connection);
                    command.ExecuteNonQuery();
                    Console.WriteLine($"{tableName} table truncated successfully!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }
    }
}