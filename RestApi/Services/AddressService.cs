using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RestApi.Services
{
    public class AddressService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AddressService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        /// <summary>
        /// Determine the Lat and Lon of a given Address
        /// </summary>
        /// <param name="address">Body which contains the Address details</param>
        /// <returns>Array with Lat and Lon values</returns>
        public async Task<double[]> GetGeometryAsync(Address address)
        {
            IList<double> result = new List<double>();

            var addressQuery = $"{address.Street} {address.Number} {address.City}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://photon.komoot.io/api/?q={addressQuery}");

            var client = _httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<JObject>(jsonString);
                var coords = json["features"][0]["geometry"]["coordinates"].ToArray();

                foreach (var coord in coords)
                {
                    result.Add(coord.ToObject<double>());
                }
            }

            return result.ToArray();
        }
    }
}