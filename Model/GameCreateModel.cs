using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace frontend.Model
{
    public class GameCreateOldModel
    {

        [JsonPropertyName("xmax")]
        public int XMax { get; set; }

        [JsonPropertyName("ymax")]
        public int YMax { get; set; }

        [JsonPropertyName("seed")]
        public int Seed { get; set; }

        [JsonPropertyName("maxPlayers")]
        public int MaxPlayers { get; set; }

        [JsonPropertyName("tiles")]
        public List<TileModel> Tiles { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public CharacterModel PlayerCharacter { get; set; }

        public event Action RefreshRequested;

        public void CallRequestRefresh()
        {
            RefreshRequested?.Invoke();
        }

        public void ToggleMoving()
        {
            PlayerCharacter.IsInMovingMode = !PlayerCharacter.IsInMovingMode;
            if (PlayerCharacter.IsInMovingMode)
            {
                DisplayMovingRange();
            }
            else
            {
                HideMovingRange();
            }
        }

        public void ToggleAttacking()
        {
            PlayerCharacter.IsInAttackingMode = !PlayerCharacter.IsInAttackingMode;
        }

        public void DisplayMovingRange()
        {
            foreach (var tileModel in Tiles.Where(t => t.IsNavigable))
            {
                int tileDistance = Math.Abs(tileModel.Position.X - PlayerCharacter.X) + Math.Abs(tileModel.Position.Y - PlayerCharacter.Y);
                if (tileDistance <= PlayerCharacter.MouvementPoint)
                {
                    tileModel.HoverColor = "#2980b9";
                }
            }

            CallRequestRefresh();
        }

        public void HideMovingRange()
        {
            foreach (var tileModel in Tiles.Where(t => !string.IsNullOrWhiteSpace(t.HoverColor)))
            {
                tileModel.HoverColor = string.Empty;
            }

            CallRequestRefresh();
        }

        public void EndTurn()
        {
            PlayerCharacter.MouvementPoint = 5;
        }
    }
}
