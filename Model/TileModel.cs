using System.Text.Json.Serialization;

using frontend.Model.Game;

namespace frontend.Model
{
    public class TileModel
    {
        [JsonPropertyName("position")]
        public Vector2Model Position { get; set; }

        public string MapImage { get; set; }

        public string ObjectImage { get; set; }

        [JsonPropertyName("isNavigable")]
        public bool IsNavigable { get; set; }

        public string HoverColor { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonIgnore]
        public GameModel GameModel { get; set; }
    }
}