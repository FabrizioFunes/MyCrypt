using mycrypt.Servicios;
using System.Text.Json;

namespace mycrypt.Servicios
{
    public class CriptoYaService : ICriptoYaService
    {
        private readonly HttpClient _httpClient;

        public CriptoYaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal?> ObtenerPrecioAsync(string codigoCripto, string exchange)
        {
            string url = $"https://criptoya.com/api/{exchange}/{codigoCripto}/ars";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("totalAsk", out var precio))
                    return precio.GetDecimal();

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
