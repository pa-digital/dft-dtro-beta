using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static DfT.DTRO.ApiTests.ApiTests.Helpers.TestConfig;

namespace DfT.DTRO.ApiTests.ApiTests.Helpers.JsonHelpers
{
    public static class JsonMethods
    {
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
                throw new Exception($"Actual JSON doesn't match expected JSON! Actual JSON was:\n\n{actualJson}");
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

        public static JObject ConvertKeysToCamelCase(JObject original)
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

        public static string CloneFirstItemInArrayAndAppend(string jsonString, string arrayPath)
        {
            try
            {
                JObject jsonObject = JObject.Parse(jsonString);
                JToken arrayToken = jsonObject.SelectToken(arrayPath);

                if (arrayToken == null || arrayToken.Type != JTokenType.Array)
                {
                    throw new ArgumentException("Invalid array path or array not found.");
                }

                JArray jsonArray = (JArray)arrayToken;

                if (jsonArray.Count == 0)
                {
                    return jsonString; // No items to copy, return original JSON
                }

                JToken firstItem = jsonArray[0].DeepClone(); // Deep clone to avoid reference issues
                jsonArray.Add(firstItem);

                return jsonObject.ToString();
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                throw new ArgumentException("Invalid JSON string.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing JSON: {ex.Message}", ex);
            }
        }

        public static async Task<string> GetJsonFromHttpResponseMessageAsync(HttpResponseMessage httpResponseMessage)
        {
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return json;
        }

        public static object GetValueAtJsonPath(string jsonString, string jsonPath)
        {
            try
            {
                JObject jsonObject = JObject.Parse(jsonString);
                JToken token = jsonObject.SelectToken(jsonPath);

                return token?.ToString() ?? null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error accessing JSON path: {jsonPath}. Exception: {ex.Message}");
            }
        }

        public static string SetValueAtJsonPath(string jsonString, string jsonPath, object newValue)
        {
            try
            {
                JObject jsonObject = JObject.Parse(jsonString);
                JToken token = jsonObject.SelectToken(jsonPath);

                if (token != null)
                {
                    if (token.Type == JTokenType.Array && newValue is IEnumerable<object> arrayValue)
                    {
                        token.Replace(JArray.FromObject(arrayValue));
                    }
                    else
                    {
                        token.Replace(JToken.FromObject(newValue));
                    }

                    return jsonObject.ToString(Formatting.Indented);
                }
                else
                {
                    Console.WriteLine($"Path '{jsonPath}' not found in JSON.");
                    return jsonString;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error modifying JSON at path: {jsonPath}. Exception: {ex.Message}");
                return jsonString;
            }
        }
    }
}