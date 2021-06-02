using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace frontend.Model.User
{
    /// <summary>
    /// Modèle d'utilisateur.
    /// </summary>
    public class UserConnectionModel
    {
        /// <summary>
        /// Obtient ou définit l'email.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// Obtient ou définit le pseudo.
        /// </summary>
        [JsonPropertyName("username")]
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Obtient ou définit le mot de passe.
        /// </summary>
        [JsonPropertyName("password")]
        [Required]
        public string Password { get; set; }
    }
}