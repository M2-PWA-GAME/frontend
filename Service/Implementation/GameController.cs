using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;

using frontend.Common;
using frontend.Component;
using frontend.Model;
using frontend.Model.Game;
using frontend.Service.Declaration;
using frontend.Utils.Interop;

using Microsoft.JSInterop;

namespace frontend.Service.Implementation
{
    public class GameController
    {
        private readonly GameModel _game;

        private readonly string _userId;

        private readonly IJSRuntime _jsRuntime;

        private readonly IGameService _gameService;

        private bool _isMoving = false;

        private bool _isAttacking = false;

        public bool IsPlayerTurn { get; set; }

        public bool HasAttacked { get; private set; } = false;

        public bool HasMoved { get; private set; } = false;

        private PlayerStateModel _userLastPlayerState;

        private WhosTurnModel _whosTurn;

        public List<PlayerStateModel> LastPlayerStates { get; set; }

        public bool IsInActions { get; set; } = false;

        public PlayerStateModel LastSelectedPlayer { get; private set; }

        public void SetSelectedPlayer(PlayerStateModel playerStateModel)
        {
            LastSelectedPlayer = playerStateModel;
        }

        public bool IsUserTurn => _whosTurn.WhoseTurn == _userId;

        public GameController(GameModel game, string userId, IJSRuntime jsRuntime, IGameService gameService)
        {
            _game = game;
            _userId = userId;
            _jsRuntime = jsRuntime;
            _gameService = gameService;
        }

        public async Task EndTurn()
        {
            await LocalStorageInterop.SetItem(_jsRuntime, "game_" + _game.Id, JsonSerializer.Serialize(new GameSaveModel { RoundIndex = GetLastRound().Index, TurnIndex = GetLastRound().Turns.IndexOf(GetLastTurn()) }));
            IsPlayerTurn = false;

            if (!HasAttacked || !HasMoved)
            {
                ActionModel actionModel = new ActionModel
                                              {
                                                  ActionType = ActionTypes.PASS,
                                                  From = new Vector2Model
                                                             {
                                                                 X = _userLastPlayerState.Position.X,
                                                                 Y = _userLastPlayerState.Position.Y
                                                             },
                                                  To = new Vector2Model
                                                           {
                                                               X = _userLastPlayerState.Position.X,
                                                               Y = _userLastPlayerState.Position.Y
                                                  }
                                              };

                await _gameService.SendAction(_game.Id, actionModel);
            }

            _whosTurn = await _gameService.WhosTurn(_game.Id);
        }

        private PlayerStateModel GetLastPlayerStateForUser(string userId)
        {
            return _game.Rounds?.OrderBy(r => r.Index).LastOrDefault()?.Turns.LastOrDefault()?.PlayerStates.Single(ps => ps.Id == userId);
        }

        private TurnModel GetLastTurn()
        {
            return _game.Rounds?.OrderBy(r => r.Index).LastOrDefault()?.Turns.LastOrDefault();
        }

        private RoundModel GetLastRound()
        {
            return _game.Rounds?.OrderBy(r => r.Index).LastOrDefault();
        }

        private List<PlayerStateModel> GetLastPlayerState()
        {
            return _game.Rounds?.OrderBy(r => r.Index).LastOrDefault()?.Turns.LastOrDefault()?.PlayerStates;
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
                if (tileModel.IsNavigable && Distance(tileModel.Position, positionToCompare) <= tilesFrom && !tileModel.Position.Equals(positionToCompare))
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

        public async Task TileClick(TileModel tile)
        {
            int playerDistance = Distance(tile.Position, _userLastPlayerState.Position);
            if (_isMoving && playerDistance <= _userLastPlayerState.MouvementPointLeft && tile.IsNavigable)
            {
                _userLastPlayerState.MouvementPointLeft = _userLastPlayerState.MouvementPointLeft - playerDistance;
                ActionModel actionModel = new ActionModel
                                              {
                                                  ActionType = ActionTypes.MOVE,
                                                  From = new Vector2Model
                                                             {
                                                                 X = _userLastPlayerState.Position.X,
                                                                 Y = _userLastPlayerState.Position.Y
                                                  },
                                                  To = new Vector2Model
                                                           {

                                                               X = tile.Position.X,
                                                               Y = tile.Position.Y
                                                  }
                                              };

                await _gameService.SendAction(_game.Id, actionModel);

                _userLastPlayerState.Position.X = tile.Position.X;
                _userLastPlayerState.Position.Y = tile.Position.Y;


                HasMoved = true;
            }

            if (_isAttacking && playerDistance <= _userLastPlayerState.Weapon.Range)
            {
                _userLastPlayerState.ObjectImage = "/img/Characters/Sword_16_LEFT.png";
                Timer timer = new Timer(500);
                timer.Elapsed += (sender, args) =>
                    {
                        _userLastPlayerState.ObjectImage = string.Empty;
                        RefreshRequested?.Invoke();
                    };
                timer.AutoReset = true;
                timer.Enabled = true;
                ActionModel actionModel = new ActionModel
                                              {
                                                  ActionType = ActionTypes.HIT,
                                                  From = new Vector2Model
                                                             {
                                                                 X = _userLastPlayerState.Position.X,
                                                                 Y = _userLastPlayerState.Position.Y
                                                             },
                                                  To = new Vector2Model
                                                           {

                                                               X = tile.Position.X,
                                                               Y = tile.Position.Y
                                                           }
                                              };

                await _gameService.SendAction(_game.Id, actionModel);
                HasAttacked = true;
            }

            CancelActions();

            if (HasAttacked && HasMoved)
            {
                await EndTurn();
            }
        }

        public async Task OnPlayerClick(PlayerStateModel playerStateModel)
        {
            int playerDistance = Distance(playerStateModel.Position, _userLastPlayerState.Position);
            if (_isAttacking && playerDistance <= _userLastPlayerState.Weapon.Range)
            {
                _userLastPlayerState.ObjectImage = "/img/Characters/Sword_16_LEFT.png";
                Timer timer = new Timer(500);
                timer.Elapsed += (sender, args) =>
                    {
                        _userLastPlayerState.ObjectImage = string.Empty;
                        RefreshRequested?.Invoke();
                    };
                timer.AutoReset = true;
                timer.Enabled = true;
                ActionModel actionModel = new ActionModel
                                              {
                                                  ActionType = ActionTypes.HIT,
                                                  From = new Vector2Model
                                                             {
                                                                 X = _userLastPlayerState.Position.X,
                                                                 Y = _userLastPlayerState.Position.Y
                                                             },
                                                  To = new Vector2Model
                                                           {

                                                               X = playerStateModel.Position.X,
                                                               Y = playerStateModel.Position.Y
                                                           }
                                              };

                await _gameService.SendAction(_game.Id, actionModel);
                HasAttacked = true;
            }

            CancelActions();

            if (HasAttacked && HasMoved)
            {
                await EndTurn();
            }
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

        public async Task InitGameAsync() {
            InitGame(_game);

            _userLastPlayerState = GetLastPlayerStateForUser(_userId);
            LastPlayerStates = GetLastPlayerState();

            if (LastPlayerStates != null)
            {
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

            if (_game.Status == GameStatus.PLAYING)
            {
                _whosTurn = await _gameService.WhosTurn(_game.Id);
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
                            tileModel.IsNavigable = false;
                            break;
                        }

                    case "WATER":
                        {
                            tileModel.MapImage = "/img/water_16.png";
                            tileModel.IsNavigable = false;
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