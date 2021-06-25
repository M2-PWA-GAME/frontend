using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using frontend.Model.Game;
using frontend.Pages.Game;

namespace frontend.Model.User
{
    public class UserGamesModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("games")]
        public List<GameListModel> Games { get; set; }
    }
}
