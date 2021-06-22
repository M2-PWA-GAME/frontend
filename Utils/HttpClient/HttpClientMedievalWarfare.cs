using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using frontend.Model;
using frontend.Service.Implementation;

namespace frontend.Utils.HttpClient
{
    /// <summary>
    /// Client Http spécifique.
    /// </summary>
    public class HttpClientMedievalWarfare : System.Net.Http.HttpClient
    {
        private static HttpClientMedievalWarfare _instance;

        public static HttpClientMedievalWarfare GetInstance()
        {
            if (_instance == null)
            {
                _instance = new HttpClientMedievalWarfare();
            }

            return _instance;
        }

        private UserService _userService = null;

        private HttpClientMedievalWarfare()
        {
            BaseAddress = new Uri("https://medieval-warfare.herokuapp.com/");
        }

        public async Task<T> GetJsonAsync<T>(string url)
        {
            try
            {
                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                var response = await GetAsync(url);
                string entityJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("not connected");
                }

                return JsonSerializer.Deserialize<T>(entityJson);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<TResponse> PostJsonAsync<TBody, TResponse>(string url, TBody body)
        {
            try
            {
                string jwtToken = await GetToken();
                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                string bodyJson = JsonSerializer.Serialize(body);
                var response = await PostAsync(url, new StringContent(bodyJson, Encoding.UTF8, "application/json"));
                string entityJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("not connected");
                }

                return JsonSerializer.Deserialize<TResponse>(entityJson);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task<string> GetToken()
        {
            if (_userService == null)
            {
                _userService = UserService.GetInstance();
            }

            string test = await _userService?.GetCurrentToken();
            return test;
        }
    }
}