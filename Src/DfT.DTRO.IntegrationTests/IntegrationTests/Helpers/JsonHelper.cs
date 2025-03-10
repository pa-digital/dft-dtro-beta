using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class JsonHelper
    {
        public static async Task<string> GetIdFromResponseJsonAsync(HttpResponseMessage httpResponseMessage)
        {
            string createDtroResponseJson = await httpResponseMessage.Content.ReadAsStringAsync();
            dynamic jsonDeserialised = JsonConvert.DeserializeObject<dynamic>(createDtroResponseJson)!;
            string id = jsonDeserialised.id.ToString();
            return id;
        }

        public static void CompareJson(string expectedJson, string actualJson)
        {
            string orderedExpectedJson = SortJsonKeys(expectedJson);
            string orderedActualJson = SortJsonKeys(actualJson);

            string[] expectedLines = orderedExpectedJson.Split('\n');
            string[] actualLines = orderedActualJson.Split('\n');

            Console.WriteLine("Comparing JSON...\n");
            int maxLines = Math.Max(expectedLines.Length, actualLines.Length);

            // Assume JSONs match until a discrepancy is found
            bool jsonsMatch = true;

            for (int i = 0; i < maxLines; i++)
            {
                string expectedLine = i < expectedLines.Length ? expectedLines[i] : "";
                string actualLine = i < actualLines.Length ? actualLines[i] : "";
                int lineNumberToPrint = i + 1;

                if (expectedLine == actualLine)
                {
                    Console.WriteLine($"line {lineNumberToPrint}    {expectedLine}");
                }
                else
                {
                    jsonsMatch = false;
                    if (!string.IsNullOrWhiteSpace(expectedLine))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"line {lineNumberToPrint}    - {expectedLine.TrimEnd()}");
                        Console.ResetColor();
                    }
                    if (!string.IsNullOrWhiteSpace(actualLine))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"line {lineNumberToPrint}    + {actualLine.TrimEnd()}");
                        Console.ResetColor();
                    }
                }
            }

            if (jsonsMatch)
            {
                Console.WriteLine("JSONs match!");
            }
            else
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
                System.Collections.Generic.List<JProperty> sortedProperties = obj.Properties().OrderBy(p => p.Name).ToList();
                JObject sortedObject = new JObject();
                foreach (JProperty prop in sortedProperties)
                {
                    sortedObject[prop.Name] = SortToken(prop.Value);
                }
                return sortedObject;
            }
            else if (token is JArray arr)
            {
                JArray sortedArray = new JArray(arr.Select(SortToken));
                return sortedArray;
            }
            else
            {
                return token; // Leave primitive values unchanged
            }
        }

        public static string ConvertJsonKeysToCamelCase(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            JObject camelCasedObject = ConvertKeysToCamelCase(jsonObject);
            return JsonConvert.SerializeObject(camelCasedObject, Formatting.Indented);
        }

        private static JObject ConvertKeysToCamelCase(JObject original)
        {
            JObject newObject = new JObject();

            foreach (var property in original.Properties())
            {
                string camelCaseKey = Char.ToLowerInvariant(property.Name[0]) + property.Name.Substring(1);

                if (property.Value.Type == JTokenType.Object)
                {
                    newObject[camelCaseKey] = ConvertKeysToCamelCase((JObject)property.Value);
                }
                else if (property.Value.Type == JTokenType.Array)
                {
                    JArray array = (JArray)property.Value;
                    JArray newArray = new JArray();

                    foreach (var item in array)
                    {
                        if (item.Type == JTokenType.Object)
                        {
                            newArray.Add(ConvertKeysToCamelCase((JObject)item));
                        }
                        else
                        {
                            newArray.Add(item);
                        }

                    }

                    newObject[camelCaseKey] = newArray;
                }
                else
                {
                    newObject[camelCaseKey] = property.Value;
                }
            }

            return newObject;
        }

        public static string MinifyJson(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            return System.Text.Json.JsonSerializer.Serialize(doc.RootElement);
        }
    }
}