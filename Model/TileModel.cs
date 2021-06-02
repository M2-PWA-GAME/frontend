using System.Text.Json.Serialization;

namespace frontend.Model
{
    public class TileModel
    {
        public int X { get; set; }

        public int Y { get; set; }

        public string MapImage { get; set; }

        public string ObjectImage { get; set; }

        public bool IsNavigable { get; set; }

        public string HoverColor { get; set; }

        [JsonIgnore]
        public GameModel GameModel { get; set; }

        public void OnTileClick()
        {
            if (GameModel.PlayerCharacter.IsInMovingMode)
            {
                GameModel.PlayerCharacter.MoveTo(this);
                GameModel.ToggleMoving();
            }
        }
    }
}