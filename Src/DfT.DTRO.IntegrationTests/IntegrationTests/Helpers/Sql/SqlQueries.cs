using Npgsql;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.DataEntities
{
    public static class SqlQueries
    {
        public static void TruncateTable(string tableName)
        {
            string connectionString = TestConfig.DatabaseConnectionString;

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
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}