using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using frontend.Model;
using frontend.Model.Game;
using frontend.Service.Declaration;
using frontend.Utils.HttpClient;

namespace frontend.Service.Implementation
{
    /// <summary>
    /// Service de gestion des partie.
    /// </summary>
    public class GameService : IGameService
    {
        private readonly UserService _userService;
        private readonly HttpClientMedievalWarfare _httpClientMedievalWarfare;

        public GameService(UserService userService)
        {
            _userService = userService;

            _httpClientMedievalWarfare = HttpClientMedievalWarfare.GetInstance();
        }

        /// <summary>
        /// Récupère la partie avec le code.
        /// </summary>
        /// <param name="id">Identifiant de la partie.</param>
        /// <returns>Partie cible.</returns>
        public async Task<GameModel> GetGame(string id)
        {
            using (HttpClient http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userService.CurrentToken);
                var response = await http.GetAsync($"https://medieval-warfare.herokuapp.com/game/{id}");
                string entityJson = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<GameModel>(entityJson);
            }
        }

        public void InitGame(GameModel game)
        {
            foreach (var tileModel in game.Tiles)
            {
                tileModel.GameModel = game;
                switch (tileModel.Type)
                {
                    case "GRASS":
                        {
                            tileModel.MapImage = "/img/grass_16.png";
                            break;
                        }
                    case "ROCK":
                        {
                            tileModel.MapImage = "/img/grass_16.png";
                            tileModel.ObjectImage = "/img/wood_16.png";
                            break;
                        }
                    case "WATER":
                        {
                            tileModel.MapImage = "/img/water_16.png";
                            break;
                        }
                }
            }
        }

        public async Task<GameModel> GenerateRandomMap(GameGeneratorModel generator)
        {
            return await _httpClientMedievalWarfare.PostJsonAsync<GameGeneratorModel, GameModel>("/maps/random", generator);
        }

        public async Task<GameModel> GenerateRandomMapWithSeed(GameGeneratorModel generator)
        {
            return await _httpClientMedievalWarfare.PostJsonAsync<GameGeneratorModel, GameModel>("/maps/seed", generator);
        }

        /// <summary>
        /// Liste les games en cours.
        /// </summary>
        /// <returns>Liste des parties.</returns>
        public async Task<List<GameListModel>> ListUserActiveGame()
        {
            using (HttpClient http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userService.CurrentToken);
                var response = await http.GetAsync($"https://medieval-warfare.herokuapp.com/users/games");
                string entityJson = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<GameListModel>>(entityJson);
            }
        }
    }
}
