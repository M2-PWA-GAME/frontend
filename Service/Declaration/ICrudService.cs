using System.Collections.Generic;

namespace frontend.Service.Declaration
{
    /// <summary>
    /// Déclaration des méthode crud des entités.
    /// </summary>
    public interface ICrudService<T>
    {
        /// <summary>
        /// Création de l'entite.
        /// </summary>
        /// <param name="entity">Entite a créer.</param>
        /// <returns>Entite créé.</returns>
        T Create(T entity);

        /// <summary>
        /// Modification de l'entite.
        /// </summary>
        /// <param name="entity">Entite a modifier.</param>
        /// <returns>Entite modifié.</returns>
        T Update(T entity);

        /// <summary>
        /// Récupère l'entite.
        /// </summary>
        /// <param name="id">Identifiant de l'entite.</param>
        /// <returns>Entite cible.</returns>
        T Get(string id);

        /// <summary>
        /// Récupère la liste des entites.
        /// </summary>
        /// <returns>Liste des entités.</returns>
        List<T> List();

        /// <summary>
        /// Supprime l'entite cible.
        /// </summary>
        /// <param name="id">Identifiant de l'entite.</param>
        void Delete(string id);
    }
}
