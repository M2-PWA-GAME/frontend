using System;
using System.Collections.Generic;
using System.Linq;

namespace frontend.Model
{
    public class GameModel
    {

        public int XMax { get; set; }

        public int YMax { get; set; }

        public List<TileModel> Tiles { get; set; }

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
            foreach (var tileModel in Tiles)
            {
                int tileDistance = Math.Abs(tileModel.X - PlayerCharacter.X) + Math.Abs(tileModel.Y - PlayerCharacter.Y);
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
