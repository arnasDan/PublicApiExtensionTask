using PublicApiExtension.Clients.NagerDate.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApiExtension.Clients.NagerDate.Client
{
    public class NagerDateClient : INagerDateClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public NagerDateClient(HttpClient httpClient, string apiUrl)
        {
            _httpClient = httpClient;
            _apiUrl = apiUrl;
        }

        public async Task<List<NagerDateHoliday>> GetHolidays(string countryCode, int year, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{year}/{countryCode}", cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<List<NagerDateHoliday>>(content, new JsonSerializerOptions
            { 
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            });
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
