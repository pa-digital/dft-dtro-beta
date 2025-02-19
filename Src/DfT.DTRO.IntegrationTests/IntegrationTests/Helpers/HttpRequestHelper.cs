using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class HttpRequestHelper
    {
        public static async Task<HttpResponseMessage> MakeHttpRequestAsync(HttpMethod method, string uri, Dictionary<string, string> headers = null, string body = null, string pathToJsonFile = null, bool printCurl = true)
        {
            // Throw exception if Content-Type exists but body or JSON file path doesn't exist, and vice versa
            if ((headers.ContainsKey("Content-Type") && body == null && pathToJsonFile == null) || (!headers.ContainsKey("Content-Type") && (body != null || pathToJsonFile != null)))
            {
                throw new Exception("Re-write test to send request with both the Content-Type header and a body (HttpClient doesn't allow one to exist and the other to be absent).");
            }

            var request = new HttpRequestMessage(method, uri);

            if (headers != null)
            {
                if (headers.TryGetValue("Content-Type", out string contentTypeValue))
                {
                    if (body != null)
                    {
                        var content = new StringContent(body);
                        content.Headers.ContentType = new MediaTypeHeaderValue(contentTypeValue);
                        request.Content = content;
                    }
                    else
                    {
                        var multipartContent = new MultipartFormDataContent();
                        var fileStream = new FileStream(pathToJsonFile, FileMode.Open, FileAccess.Read);
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

                        multipartContent.Add(fileContent, "file", Path.GetFileName(pathToJsonFile));
                        request.Content = multipartContent;
                    }
                    // Content-Type header can't be added to request like other headers, so it's removed here
                    headers.Remove("Content-Type");
                }

                foreach (var nonContentHeader in headers)
                {
                    request.Headers.Add(nonContentHeader.Key, nonContentHeader.Value);
                }
            }

            // Only print out cURL when the request does not contain sensitive data such as secrets
            if (printCurl)
            {
                PrintCurlCommand(request, pathToJsonFile);
            }

            HttpClient _httpClient;

            // Only expect and handle an untrusted certificate when running the application against localhost
            if (TestConfig.EnvironmentName == "local")
            {
                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                _httpClient = new HttpClient(handler);
            }
            else
            {
                _httpClient = new HttpClient();
            }

            var response = await _httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine("\n========= HTTP response =========");
            Console.WriteLine($"Response status code: {response.StatusCode}");
            Console.WriteLine("\nResponse headers:");
            foreach (var header in response.Headers)
            {
                Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
            }
            Console.WriteLine("\nResponse body:");
            Console.WriteLine(PrettyFormatJson(responseBody));
            Console.WriteLine("=================================\n");
            return response;
        }

        private static void PrintCurlCommand(HttpRequestMessage request, string pathToJsonFile)
        {
            var curl = new StringBuilder($"curl -k -X {request.Method} \"{request.RequestUri}\"");

            foreach (var header in request.Headers)
            {
                curl.Append($" -H \"{header.Key}: {string.Join(", ", header.Value)}\"");
            }

            if (request.Content?.Headers != null)
            {
                foreach (var contentHeader in request.Content.Headers)
                {
                    string pattern = @";\s*boundary=""[^""]+""";
                    string headerValue = string.Join(", ", contentHeader.Value);
                    // Strip out boundary, because cURL adds it automatically
                    string headerValueWithoutBoundard = Regex.Replace(headerValue, pattern, "");
                    curl.Append($" -H \"{contentHeader.Key}: {headerValueWithoutBoundard}\"");
                }
            }

            if (request.Content is StringContent)
            {
                var content = request.Content.ReadAsStringAsync().Result;
                var contentWithEscapedDoubleQuotes = JsonConvert.SerializeObject(content);
                curl.Append($" -d {contentWithEscapedDoubleQuotes}");
            }
            else if (request.Content is MultipartFormDataContent)
            {
                curl.Append($" --form 'file=@\"{pathToJsonFile}\"'");
            }

            Console.WriteLine("\n========= cURL request =========");
            Console.WriteLine(curl.ToString());
            Console.WriteLine("================================\n");
        }

        private static string PrettyFormatJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return "{}";
            try
            {
                using var jDoc = JsonDocument.Parse(json);
                return System.Text.Json.JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
            }
            catch
            {
                return json;
            }
        }
    }
}