using System;

using frontend.Component;

namespace frontend.Model
{
    public class CharacterModel
    {
        public int X { get; set; }

        public int Y { get; set; }

        public string CharacterImage
        {
            get
            {

                return BaseCharacterImage + Orientation + ".png";
            }
        }

        public string BaseCharacterImage { get; set; }

        public int MouvementPoint { get; set; }

        public Orientation Orientation { get; set; }

        public bool IsInMovingMode { get; set; }

        public bool IsInAttackingMode { get; set; }

        public void MoveTo(TileModel tile)
        {

            int tileDistance = Math.Abs(X - tile.X) + Math.Abs(Y - tile.Y);
            if (tileDistance <= MouvementPoint && tile.IsNavigable)
            {
                X = tile.X;
                Y = tile.Y;
                MouvementPoint = MouvementPoint - tileDistance;
            }
        }
    }
}
