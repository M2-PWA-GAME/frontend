using System.Threading.Tasks;

using frontend.Model.User;

namespace frontend.Service.Declaration
{
    /// <summary>
    /// Déclaration des méthodes de gestion des utilisateur.
    /// </summary>
    public interface IUserService
    {
        Task SignUp(UserConnectionModel userConnection);

        Task SignIn(UserConnectionModel userConnection);

        Task<UserConnectionModel> GetCurrentUser();
    }
}
