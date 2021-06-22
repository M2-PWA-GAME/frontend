using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using frontend.Model.Game;

namespace frontend.Service.Implementation
{
    public class GameController
    {
        private readonly GameModel _game;

        public GameController(GameModel game)
        {
            _game = game;
        }

        public void MoveTo(Vector2Model vector2)
        {

        }
    }
}
