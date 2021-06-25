using System.Collections.Generic;
using System.Threading.Tasks;

using frontend.Model;
using frontend.Model.Game;
using frontend.Model.User;

namespace frontend.Service.Declaration
{
    /// <summary>
    /// Déclaration des méthode de gestion du jeu.
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Récupère la partie avec le code.
        /// </summary>
        /// <param name="id">Identifiant de la partie.</param>
        /// <returns>Partie cible.</returns>
        Task<GameModel> GetGame(string id);

        /// <summary>
        /// Liste les games en cours.
        /// </summary>
        /// <returns>Liste des parties.</returns>
        Task<UserGamesModel> ListUserActiveGame();

        Task CreateGame(GameCreateModel gameCreate);

        Task<MapModel> GenerateRandomMap(GameGeneratorModel generator);
        Task<MapModel> GenerateRandomMapWithSeed(GameGeneratorModel generator);

        Task JoinGame(string code);

        Task<GameModel> SendAction(string code, ActionModel action);

        Task<WhosTurnModel> WhosTurn(string code);
    }
}
