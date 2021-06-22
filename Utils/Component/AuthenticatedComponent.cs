using System.Threading.Tasks;

using frontend.Model.User;
using frontend.Service.Declaration;

using Microsoft.AspNetCore.Components;

namespace frontend.Utils.Component
{
    /// <summary>
    /// Composant Authentifie.
    /// </summary>
    public class AuthenticatedComponent : ComponentBase, IComponent
    {
        /// <summary>
        /// Obtient ou définit le service de gestion des utilisateur.
        /// </summary>
        [Inject]
        public IUserService UserService { get; set; }

        public UserConnectionModel CurrentUser { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// Override this method if you will perform an asynchronous operation and
        /// want the component to refresh when that operation is completed.
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            CurrentUser = await UserService.GetCurrentUser();
            if (CurrentUser == null)
            {
                NavManager.NavigateTo("/user/signin");
            }
            await base.OnInitializedAsync();
        }
    }
}
