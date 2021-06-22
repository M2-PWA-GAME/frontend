using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

using frontend.Common;
using frontend.Component;
using frontend.Model;
using frontend.Model.Game;

namespace frontend.Service.Implementation
{
    public class GameController
    {
        private readonly GameModel _game;

        private readonly string _userId;

        private bool _isMoving = false;

        private bool _isAttacking = false;

        public bool HasAttacked { get; private set; } = false;
        public bool HasMoved { get; private set; } = false;

        private PlayerStateModel _userLastPlayerState;

        public List<PlayerStateModel> LastPlayerStates { get; set; }

        public bool IsInActions { get; set; } = false;

        public PlayerStateModel LastSelectedPlayer { get; private set; }

        public void SetSelectedPlayer(PlayerStateModel playerStateModel)
        {
            LastSelectedPlayer = playerStateModel;
        }

        public GameController(GameModel game, string userId)
        {
            _game = game;
            _userId = userId;
            InitGame();
        }

        private PlayerStateModel GetLastPlayerStateForUser(string userId)
        {
            return _game.Rounds.OrderBy(r => r.Index).LastOrDefault()?.Turns.LastOrDefault()?.PlayerStates.Single(ps => ps.Id == userId);
        }

        private List<PlayerStateModel> GetLastPlayerState()
        {
            return _game.Rounds.OrderBy(r => r.Index).LastOrDefault()?.Turns.LastOrDefault()?.PlayerStates;
        }

        public void ToogleMoveActions()
        {
            _isMoving = true;
            _isAttacking = false;
            IsInActions = true;

            SetTilesColor(_game.Map.Tiles, _userLastPlayerState.Position, _userLastPlayerState.MouvementPointLeft, "#2980b9");
        }

        private void SetTilesColor(List<TileModel> tiles, Vector2Model positionToCompare, int tilesFrom, string hexColor)
        {
            foreach (var tileModel in tiles)
            {
                if (Distance(tileModel.Position, positionToCompare) <= tilesFrom && !tileModel.Position.Equals(positionToCompare))
                {
                    tileModel.HoverColor = hexColor;
                }
            }
        }

        public void DisplayMouvementAndAttackForPlayerState(PlayerStateModel playerStateModel)
        {
            if (!IsInActions)
            {
                DisplayMouvementForPlayerState(playerStateModel);
                DisplayAttackForPlayerState(playerStateModel);
            }
        }
        public void DisplayMouvementForPlayerState(PlayerStateModel playerStateModel)
        {
            if (!IsInActions)
            {
                SetTilesColor(_game.Map.Tiles, playerStateModel.Position, playerStateModel.MouvementPointLeft, "#2980b9");
            }
        }
        public void DisplayAttackForPlayerState(PlayerStateModel playerStateModel)
        {
            if (!IsInActions)
            {
                SetTilesColor(_game.Map.Tiles, playerStateModel.Position, playerStateModel.Weapon.Range, "#e74c3c");
            }
        }

        public void TileClick(TileModel tile)
        {
            int playerDistance = Distance(tile.Position, _userLastPlayerState.Position);
            if (_isMoving && playerDistance <= _userLastPlayerState.MouvementPointLeft)
            {
                _userLastPlayerState.MouvementPointLeft = _userLastPlayerState.MouvementPointLeft - playerDistance;
                _userLastPlayerState.Position.X = tile.Position.X;
                _userLastPlayerState.Position.Y = tile.Position.Y;
                HasMoved = true;
            }

            if (_isAttacking && playerDistance <= _userLastPlayerState.Weapon.Range)
            {
                _userLastPlayerState.ObjectImage = "/img/Characters/Sword_16_LEFT.png";
                Timer timer = new Timer(500);
                timer.Elapsed += (sender, args) => { _userLastPlayerState.ObjectImage = string.Empty;RefreshRequested?.Invoke(); };
                timer.AutoReset = true;
                timer.Enabled = true;
                HasAttacked = true;
            }
            CancelActions();
        }

        public void ToogleAttackingActions()
        {
            _isMoving = false;
            _isAttacking = true;
            IsInActions = true;

            SetTilesColor(_game.Map.Tiles, _userLastPlayerState.Position, _userLastPlayerState.Weapon.Range, "#e74c3c");
        }

        public void CancelActions()
        {
            _isMoving = false;
            _isAttacking = false;
            IsInActions = false;

            CleanDisplay();
        }

        public void CleanDisplay()
        {
            if (!IsInActions)
            {
                foreach (var tileModel in _game.Map.Tiles)
                {
                    tileModel.HoverColor = string.Empty;
                }

                RefreshRequested?.Invoke();
            }
        }


        private void InitGame()
        {
            InitGame(_game);

            _userLastPlayerState = GetLastPlayerStateForUser(_userId);
            LastPlayerStates = GetLastPlayerState();

            foreach (var playerStateModel in LastPlayerStates)
            {
                if (playerStateModel.Id == _userId)
                {
                    playerStateModel.CharacterImage = "/img/Characters/Character_16_DOWN.png";
                }
                else
                {
                    playerStateModel.CharacterImage = "/img/Characters/CharacterEnemy_16_DOWN.png";
                }

                playerStateModel.MouvementPointLeft = GameInfos.PLAYER_MOUVEMENT;
            }
        }

        public static void InitGame(GameModel game)
        {
            foreach (var tileModel in game.Map.Tiles)
            {
                tileModel.GameModel = game;
                switch (tileModel.Type)
                {
                    case "GRASS":
                        {
                            tileModel.MapImage = "/img/grass_16.png";
                            break;
                        }

                    case "ROCK":
                        {
                            tileModel.MapImage = "/img/grass_16.png";
                            tileModel.ObjectImage = "/img/wood_16.png";
                            break;
                        }

                    case "WATER":
                        {
                            tileModel.MapImage = "/img/water_16.png";
                            break;
                        }
                }
            }
        }

        private int Distance(Vector2Model vector1, Vector2Model vector2)
        {
            return Math.Abs(vector2.X - vector1.X) + Math.Abs(vector2.Y - vector1.Y);
        }

        public event Action RefreshRequested;

        public void CallRequestRefresh()
        {
            RefreshRequested?.Invoke();
        }
    }
}
