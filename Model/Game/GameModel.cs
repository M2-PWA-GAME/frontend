using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace frontend.Model.Game
{
    public class GameModel
    {
        [JsonPropertyName("map")]
        public MapModel Map { get; set; }

        [JsonPropertyName("rounds")]
        public List<RoundModel> Rounds { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("maxPlayers")]
        public int MaxPlayers { get; set; }

        [JsonPropertyName("users")]
        public List<string> Users { get; set; }

        [JsonPropertyName("weapons")]
        public List<WeaponModel> Weapons { get; set; }
        
    }

    public class RoundModel
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("turns")]
        public List<TurnModel> Turns { get; set; }
    }

    public class TurnModel
    {
        [JsonPropertyName("actions")]
        public List<ActionModel> Actions { get; set; }

        [JsonPropertyName("playerId")]
        public string PlayerId { get; set; }

        [JsonPropertyName("playersStates")]
        public List<PlayerStateModel> PlayerStates { get; set; }
    }

    public class PlayerStateModel
    {
        [JsonPropertyName("armor")]
        public int Armor { get; set; }

        [JsonPropertyName("health")]
        public int Health { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("position")]
        public Vector2Model Position { get; set; }

        [JsonPropertyName("weapon")]
        public WeaponModel Weapon { get; set; }

        [JsonIgnore]
        public int MouvementPointLeft { get; set; }

        [JsonIgnore]
        public string CharacterImage { get; set; }

        [JsonIgnore]
        public string ObjectImage { get; set; }
    }

    public class WeaponModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("damages")]
        public int Damages { get; set; }

        [JsonPropertyName("range")]
        public int Range { get; set; }

    }

    public class ActionModel
    {
        [JsonPropertyName("actionType")]
        public string ActionType { get; set; }

        [JsonPropertyName("from")]
        public Vector2Model From { get; set; }

        [JsonPropertyName("to")]
        public Vector2Model To { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
