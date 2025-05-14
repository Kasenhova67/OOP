using Lab3OOp.Domain.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3OOp.Infrastrucrura.Adapters
{
    public class QuoteApiAdapter : IQuoteApiAdapter
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://api.quotable.io/random";
        private const int TimeoutSeconds = 5;

        public QuoteApiAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(TimeoutSeconds);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<QuoteDTO> GetMotivationalQuote()
        {
            try
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(TimeoutSeconds)))
                {
                    var response = await _httpClient.GetAsync(ApiUrl, cts.Token);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var quote = JsonConvert.DeserializeObject<QuoteDTO>(content);
                    return quote;
                }
            }
            catch (Exception ex) when (
                ex is HttpRequestException ||
                ex is TaskCanceledException ||
                ex is JsonException)
            {
                // Return null to indicate failure
                return null;
            }
        }
    }
}