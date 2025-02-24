using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class JsonHelper
    {
        public static async Task<string> GetIdFromResponseJsonAsync(HttpResponseMessage httpResponse)
        {
            string createDtroResponseJson = await httpResponse.Content.ReadAsStringAsync();
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(createDtroResponseJson)!;
            string id = jsonDeserialised.id.ToString();
            return id;
        }

        public static void CompareJson(string expectedJson, string actualJson)
        {
            // Until files are set to camel case, convert files to lower case before comparison
            var expectedJsonToLowerCase = expectedJson.ToLower();
            var actualJsonToLowerCase = actualJson.ToLower();

            string orderedJson1 = SortJsonKeys(expectedJsonToLowerCase);
            string orderedJson2 = SortJsonKeys(actualJsonToLowerCase);

            string[] expectedLines = orderedJson1.Split('\n');
            string[] actualLines = orderedJson2.Split('\n');

            Console.WriteLine("Comparing JSON...\n");
            int maxLines = Math.Max(expectedLines.Length, actualLines.Length);

            var jsonsMatch = true;

            for (int i = 0; i < maxLines; i++)
            {
                string line1 = i < expectedLines.Length ? expectedLines[i] : "";
                string line2 = i < actualLines.Length ? actualLines[i] : "";

                if (line1 == line2)
                {
                    Console.WriteLine(line1);
                }
                else
                {
                    jsonsMatch = false;
                    if (!string.IsNullOrWhiteSpace(line1))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"- {line1.TrimEnd()}");
                        Console.ResetColor();
                    }
                    if (!string.IsNullOrWhiteSpace(line2))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"+ {line2.TrimEnd()}");
                        Console.ResetColor();
                    }
                }
            }

            if (!jsonsMatch)
            {
                throw new Exception("Actual JSON doesn't match expected JSON!");
            }
        }

        private static string SortJsonKeys(string jsonString)
        {
            try
            {
                JToken token = JToken.Parse(jsonString);
                return SortToken(token).ToString(Formatting.Indented);
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                return null;
            }
            catch (Exception generalException)
            {
                Console.WriteLine($"General error: {generalException.Message}");
                return null;
            }
        }

        private static JToken SortToken(JToken token)
        {
            if (token is JObject obj)
            {
                var sortedProperties = obj.Properties().OrderBy(p => p.Name).ToList();
                var sortedObject = new JObject();
                foreach (var prop in sortedProperties)
                {
                    sortedObject[prop.Name] = SortToken(prop.Value);
                }
                return sortedObject;
            }
            else if (token is JArray arr)
            {
                var sortedArray = new JArray(arr.Select(SortToken));
                return sortedArray;
            }
            else
            {
                return token; // Leave primitive values unchanged
            }
        }
    }
}