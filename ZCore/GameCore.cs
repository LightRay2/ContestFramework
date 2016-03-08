using Framework.Opengl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Framework
{
    /// <summary>
    /// Все, что должен делать главный класс - изменить состояние и вернуть новый кадр назад контроллера
    /// </summary>
    public class GameCore<TState, TTurn, TRound, TPlayer>
        where TState : IState<TPlayer>, new()
        where TTurn : ITurn<TPlayer>
        where TPlayer : IPlayer
        where TRound : IRound<TTurn,TPlayer>, new()
    {
        public static Random  Rand= new Random();
        public static bool IsWorking = false;
        public static bool TryRunAsSingleton(IGame<TState, TTurn, TRound, TPlayer> game, List<FormMainSettings> settings)
        {
            if (IsWorking)
                return false;
            IsWorking = true;

            _instance = new GameCore<TState, TTurn, TRound, TPlayer>();

            _instance._game = game;
            _instance._settings = settings;

            if (!_instance.TryRunNextGame())
                return false;

            new Form1(_instance.Process).ShowDialog();
            return true;
        }

        static GameCore<TState, TTurn, TRound, TPlayer> _instance;
        static GameCore()
        {
        }

        TState _currentState;
        IGame<TState, TTurn, TRound, TPlayer> _game;

        int _currentGameNumber = -1;
        bool PauseButtonPressed = false;
        List<FormMainSettings> _settings;
        TRound _currentRound;

        ConcurrentDictionary<int, TRound> allRounds;

        enum EGameMode { localWithHuman, localWithoutHuman, fromServer, replayFile };
        EGameMode _gameMode;
        bool TryRunNextGame()
        {
            _currentGameNumber++;
            if (_currentGameNumber >= _settings.Count)
                return false;
            var settings = _settings[_currentGameNumber];

            _currentState = new TState();
            _currentState.roundNumber = -1;
            _currentState.Init(settings);

            if (_currentState.players.Count == 0)
                throw new Exception("В State.Init() должны быть созданы players");

            _gameMode = _currentState.players.Any(x => x.controlledByHuman) ?
                EGameMode.localWithHuman :
                EGameMode.localWithoutHuman; //todo replay server

            allRounds = new ConcurrentDictionary<int, TRound>(); //todo
            //_turnReceiver = new TurnReceiver(_currentState, _engine, settings.javaPath, settings.ThisIsReplayGame);
            //if (settings.ThisIsReplayGame)
            //{
            //    bool success = _turnReceiver.TryLoadReplay(settings.replayPath);
            //    if (!success)
            //    {
            //        _currentState.Message = "Не удалось загрузить повтор. Нажмите Enter";
            //        _currentState.IsFinished = true;
            //    }
            //}


            //  AnimationGoNextTurn();
            //  DrawAll();

            GoToPhase(EProcessPhase.beforeRound);
            return true;
        }

        enum EProcessPhase { beforeRound, getTurnsOfNextRound, processRound, waitUntilAnimationFinished, gameFinished }
        EProcessPhase _processPhase;
        double animationStage = 0;
        private List<TPlayer> _currentPlayerOrder;

        bool animationFinished { get { return animationStage.Equal(1); } }
        
        void GoToPhase(EProcessPhase phase)
        {
            switch (phase)
            {
                case EProcessPhase.getTurnsOfNextRound:
                    bool nextRoundExists = allRounds.TryGetValue(_currentState.roundNumber, out _currentRound);
                    if (!nextRoundExists)
                    {
                        _currentRound = new TRound();
                        _currentRound.Random = new System.Random(Rand.Next());
                        _currentRound.turns = new List<TTurn>();
                        _currentPlayerOrder = _game.GetTurnOrderForNextRound(_currentState);
                    }
                    else
                    {
                        GoToPhase(EProcessPhase.processRound);
                        return;
                    }
                    break;
                case EProcessPhase.processRound:
                    _game.ProcessRound(ref _currentState, _currentRound);
                    GoToPhase(EProcessPhase.waitUntilAnimationFinished);
                    return;
                case EProcessPhase.waitUntilAnimationFinished:
                    animationStage = 0;
                    break;
            }

            _processPhase = phase;
        }

        public Frame Process(IGetKeyboardState keyboard)
        {
            if (keyboard.GetActionTime(EKeyboardAction.Esc) == 1) return null;//todo завершить второй процесс корректно
            if (keyboard.GetActionTime(EKeyboardAction.Fire) == 1)
                PauseButtonPressed = !PauseButtonPressed;

            if (!PauseButtonPressed)
            {
                //фазы лесенкой, чтобы можно было хоть по всем пройтись

                if (_processPhase == EProcessPhase.beforeRound)
                {
                    _currentState.roundNumber++;
                    if (_currentState.GameFinished)
                        GoToPhase(EProcessPhase.gameFinished);
                    else
                        GoToPhase(EProcessPhase.getTurnsOfNextRound);
                }

                if (_processPhase == EProcessPhase.getTurnsOfNextRound)
                {
                    bool humanTurnWasAdded = false;
                    while (_currentRound.turns.Count < _currentPlayerOrder.Count)
                    {
                        var player = _currentPlayerOrder[_currentRound.turns.Count];
                        if (player.controlledByHuman)
                        {
                            if (humanTurnWasAdded)
                                break;

                            var turn = _game.TryGetHumanTurn(_currentState, player, keyboard);
                            if (turn != null)
                            {
                                _currentRound.turns.Add(turn);
                                humanTurnWasAdded = true;
                            }
                            else
                                break;
                        }
                        else
                        {
                            ExternalProgramExecuter epe =
                                new ExternalProgramExecuter(player.programAddress, "input.txt", "output.txt", _settings[_currentGameNumber].JavaPath);

                            string input = _game.GetInputFile(_currentState, player);
                            string output;
                            string comment;

                            ExecuteResult res = epe.Execute(input, 2, out output, out comment);
                            var turn = _game.GetProgramTurn(_currentState, player,output, res, comment);
                            _currentRound.turns.Add(turn);
                        }
                    }

                    if (_currentRound.turns.Count == _currentPlayerOrder.Count)
                        GoToPhase(EProcessPhase.processRound);
                }

                if (_processPhase == EProcessPhase.waitUntilAnimationFinished)
                {
                    UpdateAnimationStage();
                    if (animationFinished)
                        GoToPhase(EProcessPhase.beforeRound);
                }

                if (_processPhase == EProcessPhase.gameFinished)
                {
                    if (keyboard.GetActionTime(EKeyboardAction.Enter) == 1)
                    {
                        bool success = TryRunNextGame();
                        if (!success)
                        {
                            return null;
                        }
                    }
                }
            }

            Frame frame = new Frame();
            _game.DrawAll(ref frame, _currentState, animationStage, keyboard);
            _currentState.frameNumber++;
            return frame;
        }


       
        private void UpdateAnimationStage()
        {
            animationStage += 1.0 / Vars.FramesPerTurn;
            if (animationStage > 1)
                animationStage = 1;
        }
    }
}
