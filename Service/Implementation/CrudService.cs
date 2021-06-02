using System;
using System.Collections.Generic;

using frontend.Service.Declaration;

namespace frontend.Service.Implementation
{
    /// <summary>
    /// Implémentation des méthodes du <see cref="CrudService<T>"/>
    /// </summary>
    public abstract class CrudService<T> : ICrudService<T>
    {
        /// <summary>
        /// Création de l'entite.
        /// </summary>
        /// <param name="entity">Entite a créer.</param>
        /// <returns>Entite créé.</returns>
        public T Create(T entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Modification de l'entite.
        /// </summary>
        /// <param name="entity">Entite a modifier.</param>
        /// <returns>Entite modifié.</returns>
        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Récupère l'entite.
        /// </summary>
        /// <param name="id">Identifiant de l'entite.</param>
        /// <returns>Entite cible.</returns>
        public T Get(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Récupère la liste des entites.
        /// </summary>
        /// <returns>Liste des entités.</returns>
        public List<T> List()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Supprime l'entite cible.
        /// </summary>
        /// <param name="id">Identifiant de l'entite.</param>
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
