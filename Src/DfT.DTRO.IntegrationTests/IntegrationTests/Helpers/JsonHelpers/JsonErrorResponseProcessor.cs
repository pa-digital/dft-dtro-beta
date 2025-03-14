using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers.JsonHelpers
{
    public static class JsonErrorResponseProcessor
    {
        public static async Task<ErrorJson> GetErrorJson(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            string jsonContent = await response.Content.ReadAsStringAsync();

            try
            {
                using (JsonDocument document = JsonDocument.Parse(jsonContent))
                {
                    JsonElement root = document.RootElement;

                    string error = root.TryGetProperty("error", out JsonElement errorElement) ? errorElement.GetString() : null;
                    string message = root.TryGetProperty("message", out JsonElement messageElement) ? messageElement.GetString() : null;

                    return new ErrorJson { Error = error, Message = message };
                }
            }
            catch (Newtonsoft.Json.JsonException)
            {
                // Handle JSON parsing errors (e.g., invalid JSON)
                return new ErrorJson { Error = null, Message = $"Error parsing JSON from response: {jsonContent}" };
            }
        }

        public class ErrorJson
        {
            public string Error { get; set; }
            public string Message { get; set; }
        }
    }
}