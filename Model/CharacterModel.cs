using System;

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

        public void MoveTo(int x, int y)
        {

            int tileDistance = Math.Abs(X - x) + Math.Abs(Y - y);
            if (tileDistance <= MouvementPoint)
            {
                X = x;
                Y = y;
                MouvementPoint = MouvementPoint - tileDistance;
            }
        }
    }
}
