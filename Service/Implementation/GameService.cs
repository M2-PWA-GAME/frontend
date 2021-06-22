using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using frontend.Model;
using frontend.Model.Game;
using frontend.Model.User;
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
            return await _httpClientMedievalWarfare.GetJsonAsync<GameModel>($"/games/{id}");
        }

        public void InitGame(GameModel gameCreate)
        {
            foreach (var tileModel in gameCreate.Map.Tiles)
            {
                tileModel.GameModel = gameCreate;
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

        public async Task CreateGame(GameCreateModel gameCreate)
        {
            await _httpClientMedievalWarfare.PostJsonAsync<GameCreateModel, GameJoinModel>("/games", gameCreate);
        }

        public async Task<MapModel> GenerateRandomMap(GameGeneratorModel generator)
        {
            return await _httpClientMedievalWarfare.PostJsonAsync<GameGeneratorModel, MapModel>("/maps/random", generator);
        }

        public async Task<MapModel> GenerateRandomMapWithSeed(GameGeneratorModel generator)
        {
            return await _httpClientMedievalWarfare.PostJsonAsync<GameGeneratorModel, MapModel>("/maps/seed", generator);
        }

        /// <summary>
        /// Liste les games en cours.
        /// </summary>
        /// <returns>Liste des parties.</returns>
        public async Task<UserGamesModel> ListUserActiveGame()
        {
            return await _httpClientMedievalWarfare.GetJsonAsync<UserGamesModel>("/users/games");
        }
    }
}
