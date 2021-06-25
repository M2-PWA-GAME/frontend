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
                string token = await GetToken();
                if (string.IsNullOrWhiteSpace(token))
                {
                    return default(T);
                }

                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await GetAsync(url);
                string entityJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("not connected");
                }

                return JsonSerializer.Deserialize<T>(entityJson);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetJsonAsync(string url)
        {
            try
            {
                string token = await GetToken();
                if (string.IsNullOrWhiteSpace(token))
                {
                    return;
                }

                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await GetAsync(url);
                string entityJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("not connected");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TResponse> PostJsonAsync<TBody, TResponse>(string url, TBody body)
        {
            try
            {
                string jwtToken = await GetToken();
                if (string.IsNullOrWhiteSpace(jwtToken))
                {
                    return default(TResponse);
                }
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
            catch (Exception)
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