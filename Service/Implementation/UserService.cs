using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using frontend.Model.User;
using frontend.Service.Declaration;
using frontend.Utils.Helper;
using frontend.Utils.Interop;

using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace frontend.Service.Implementation
{
    /// <summary>
    /// Service de gestion des utilisateurs.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Obtient ou définit l'utilisateur actuel.
        /// </summary>
        public UserConnectionModel UserConnection { get; set; }

        /// <summary>
        /// Obtient ou définit le token actuel.
        /// </summary>
        public string CurrentToken { get; set; }

        /// <summary>
        /// Obtient ou définit le token actuel.
        /// </summary>
        public DateTime? TokenEndDate { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur.
        /// </summary>
        public string CurrentUserId { get; set; }

        public event Action UserChanged;

        private static UserService _instance = null;

        public static UserService GetInstance()
        {
            return _instance;
        }

        public UserService(IJSRuntime jsRuntime)
        {
            if (_instance != null)
            {
                throw new Exception($"Une instance {nameof(UserService)} déjà existante");
            }
            _jsRuntime = jsRuntime;
            _instance = this;
        }
        
        /// <summary>
        /// Inscrit un utilisateur.
        /// </summary>
        /// <param name="userConnection">Utilisateur a incrire.</param>
        /// <returns>Resultat de la <see cref="Task"/> asynchrone.</returns>
        public async Task SignUp(UserConnectionModel userConnection)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    string json = JsonSerializer.Serialize(userConnection);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await http.PostAsync("https://medieval-warfare.herokuapp.com/users/signup", content);

                    string token = await response.Content.ReadAsStringAsync();
                    await HandleJwtTokenAsync(token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Connecte un utilisateur.
        /// </summary>
        /// <param name="userConnection">Utilisateur a connecter.</param>
        /// <returns>Resultat de la <see cref="Task"/> asynchrone.</returns>
        public async Task SignIn(UserConnectionModel userConnection)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    string json = JsonSerializer.Serialize(userConnection);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await http.PostAsync("https://medieval-warfare.herokuapp.com/users/signin", content);
                    string token = await response.Content.ReadAsStringAsync();
                    await HandleJwtTokenAsync(token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Rafraichit un token.
        /// </summary>
        /// <returns>Resultat de la <see cref="Task"/> asynchrone.</returns>
        public async Task ResfreshToken()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(CurrentToken))
                {
                    using (HttpClient http = new HttpClient())
                    {
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentToken);
                        var response = await http.GetAsync("https://medieval-warfare.herokuapp.com/users/refresh");
                        string token = await response.Content.ReadAsStringAsync();
                        await HandleJwtTokenAsync(token);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Récupère l'utilisateur connecté.
        /// </summary>
        /// <returns>Utilisateur connecté.</returns>
        public async Task<string> GetCurrentToken()
        {
            if (CurrentToken == null)
            {
                CurrentToken = await LocalStorageInterop.GetItem(_jsRuntime, "jwt");
            }
            if (TokenEndDate == null)
            {
                string dateJson = await LocalStorageInterop.GetItem(_jsRuntime, "jwtEndDate");
                TokenEndDate = !string.IsNullOrWhiteSpace(dateJson) ? JsonSerializer.Deserialize<DateTime?>(dateJson) : null;
            }

            if (TokenEndDate < DateTime.Now || (TokenEndDate == null && CurrentToken != null))
            {
                await ResfreshToken();
            }

            if (CurrentToken == null)
            {
                await ResetJwtAsync();
            }

            return CurrentToken;
        }

        /// <summary>
        /// Récupère l'utilisateur connecté.
        /// </summary>
        /// <returns>Utilisateur connecté.</returns>
        public async Task<UserConnectionModel> GetCurrentUser()
        {
            if (CurrentToken == null)
            {
                CurrentToken = await LocalStorageInterop.GetItem(_jsRuntime, "jwt");
            }

            if (TokenEndDate == null)
            {
                string dateJson = await LocalStorageInterop.GetItem(_jsRuntime, "jwtEndDate");
                TokenEndDate = !string.IsNullOrWhiteSpace(dateJson) ? JsonSerializer.Deserialize<DateTime?>(dateJson) : null;
            }

            if (TokenEndDate < DateTime.Now || (TokenEndDate == null && CurrentToken != null))
            {
                await ResfreshToken();
            }

            if (UserConnection == null)
            {
                string userJson = await LocalStorageInterop.GetItem(_jsRuntime, "user");
                UserConnection = string.IsNullOrWhiteSpace(userJson) ? null : JsonSerializer.Deserialize<UserConnectionModel>(userJson);
            }

            if (UserConnection == null && CurrentToken != null)
            {
                await UpdateUser();
            }

            if (CurrentToken == null)
            {
                await ResetJwtAsync();
            }

            return CurrentToken == null ? null : UserConnection;
        }

        /// <summary>
        /// Récupère l'utilisateur courrant.
        /// </summary>
        /// <returns>Utilisateur courrant.</returns>
        public async Task<UserConnectionModel> MeAsync()
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentToken);
                    var response = await http.GetAsync("https://medieval-warfare.herokuapp.com/users/me");
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserConnectionModel>(json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<UserConnectionModel> GetUser(string id)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetCurrentToken());
                    var response = await http.GetAsync($"https://medieval-warfare.herokuapp.com/users/{id}");
                    return JsonSerializer.Deserialize<UserConnectionModel>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetCurrentUserId()
        {
            return CurrentUserId;
        }

        private async Task UpdateUser()
        {
            UserConnection = await MeAsync();
            UserChanged?.Invoke();
        }

        /// <summary>
        /// Gère le token JWT actuel.
        /// </summary>
        /// <param name="token">Token JWT.</param>
        /// <returns>Resultat de la <see cref="Task"/> asynchrone.</returns>
        private async Task HandleJwtTokenAsync(string token)
        {
            CurrentToken = token; 
            await LocalStorageInterop.SetItem(_jsRuntime, "jwt", CurrentToken);

            if (!string.IsNullOrWhiteSpace(CurrentToken))
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                SecurityToken jsonToken = handler.ReadToken(CurrentToken);
                JwtSecurityToken tokenS = jsonToken as JwtSecurityToken;

                CurrentUserId = tokenS.Claims.FirstOrDefault(c => c.Type == "user_id").Value;
                TokenEndDate = DateTimeHelper.UnixTimeStampToDateTime(int.Parse(tokenS.Claims.FirstOrDefault(c => c.Type == "exp").Value));
                await LocalStorageInterop.SetItem(_jsRuntime, "jwtEndDate", JsonSerializer.Serialize(TokenEndDate));

                if (UserConnection == null)
                {
                    await UpdateUser();
                    UserConnection.Password = null;
                    await LocalStorageInterop.SetItem(_jsRuntime, "user", JsonSerializer.Serialize(UserConnection));
                }
            }
            else
            {
                await ResetJwtAsync();
            }
        }

        private async Task ResetJwtAsync()
        {
            var localStorageToken = await LocalStorageInterop.GetItem(_jsRuntime, "jwt");
            Console.WriteLine($"ResetJwtAsync | CurrentToken: {CurrentToken}, LocalStorage: {localStorageToken}");
            if (string.IsNullOrWhiteSpace(CurrentToken) && string.IsNullOrWhiteSpace(localStorageToken))
            {
                Console.WriteLine($"ResetJwtAsync | CLEANED");
                CurrentToken = null;
                CurrentUserId = null;
                UserConnection = null;
                await LocalStorageInterop.SetItem(_jsRuntime, "user", null);
                await LocalStorageInterop.SetItem(_jsRuntime, "jwtEndDate", null);
                await LocalStorageInterop.SetItem(_jsRuntime, "jwt", null);
            }
        }
    }
}