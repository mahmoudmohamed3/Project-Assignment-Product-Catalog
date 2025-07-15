using System.Text.Json;
using System.Text;

namespace Product_Catalog.Services
{
    public class MicrosoftTranslatorService : ITranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;

        public MicrosoftTranslatorService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TranslationServices:MicrosoftTranslator:ApiKey"] ?? throw new ArgumentNullException("Microsoft Translator API Key not configured.");
            _endpoint = configuration["TranslationServices:MicrosoftTranslator:Endpoint"] ?? throw new ArgumentNullException("Microsoft Translator Endpoint not configured.");
        }

        public async Task<string> TranslateAsync(string text, string targetLanguage)
        {
           
            var route = $"/translate?api-version=3.0&to={targetLanguage}";
            object[] body = new object[] { new { Text = text } };
            var requestBody = JsonSerializer.Serialize(body);

            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(_endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", _apiKey);

                HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(result))
                {
                    JsonElement root = doc.RootElement;
                    if (root.GetArrayLength() > 0)
                    {
                        JsonElement translationElement = root[0].GetProperty("translations")[0];
                        return translationElement.GetProperty("text").GetString() ?? text;
                    }
                }
            }
            return text; 
        }
    }
}
