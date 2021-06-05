using System.Collections.Generic;
using System.Threading.Tasks;

using frontend.Model;
using frontend.Model.Game;

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
        Task<List<GameListModel>> ListUserActiveGame();

        void InitGame(GameModel game);

        Task<GameModel> GenerateRandomMap(GameGeneratorModel generator);
        Task<GameModel> GenerateRandomMapWithSeed(GameGeneratorModel generator);
    }
}
