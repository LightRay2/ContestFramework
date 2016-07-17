﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Framework
{
    /// <summary>
    /// Все, что должен делать главный класс - изменить состояние и вернуть новый кадр назад контроллера
    /// </summary>
    public class GameCore<TState, TTurn, TRound, TPlayer>
        where TState : IState<TPlayer, TRound, TTurn>, new()
        where TTurn : ITurn<TPlayer>
        where TPlayer : IPlayer
        where TRound : IRound<TTurn,TPlayer>, new()
    {
        public static Random  Rand= new Random();
        public static bool IsWorking = false;
        static ConcurrentDictionary<int, object> _roundsFromServer;
        public static bool TryRunAsSingleton(IGame<TState, TTurn, TRound, TPlayer> game, List<object> settings, ConcurrentDictionary<int, object> roundsFromServer=null)
        {
            if (IsWorking)
                return false;
            IsWorking = true;
            _roundsFromServer = roundsFromServer;

            _instance = new GameCore<TState, TTurn, TRound, TPlayer>();

            _instance._game = game;
            _instance._settings = settings;

            if (!_instance.TryRunNextGame())
                return false;

            var gameForm = new  GameForm(_instance.Process);
            game.LoadSpritesAndFonts();

            //todo keyboard
            gameForm.FormClosed += new FormClosedEventHandler((o,e)=>IsWorking = false);
            gameForm.ShowDialog();
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
        List<object> _settings;
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
            _currentState.rounds = new List<TRound>();
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
        double animationFinishStage = 1;
        private List<TPlayer> _currentPlayerOrder;

        bool animationFinished { get { return animationStage.Equal(animationFinishStage); } }
        
        void GoToPhase(EProcessPhase phase)
        {
            switch (phase)
            {
                case EProcessPhase.getTurnsOfNextRound:
                    bool nextRoundExists=false;
                    if (_roundsFromServer == null)
                    {
                        nextRoundExists = allRounds.TryGetValue(_currentState.roundNumber, out _currentRound);
                    }
                    else
                    {
                        object tmp;
                        if (_roundsFromServer.TryGetValue(_currentState.roundNumber, out tmp))
                        {
                            nextRoundExists = true;
                        }
                        _currentRound = JsonConvert.DeserializeObject<TRound> (tmp.ToString());
                    }
                    
                        
                    if (!nextRoundExists)
                    {
                        _currentRound = new TRound();
                        _currentRound.random = new System.Random(Rand.Next());
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
                    _currentState.rounds.Add(_currentRound);
                    _game.ProcessRoundAndSetTotalStage(_currentState, _currentRound);
                    animationFinishStage = _currentRound.totalStage;
                    GoToPhase(EProcessPhase.waitUntilAnimationFinished);
                    return;
                    break;
            }

            _processPhase = phase;
        }

        public Frame Process(GlInput input)
        {
            if (input.KeyTime ( System.Windows.Input.Key.Escape) == 1) return null;//todo завершить второй процесс корректно
            if (input.KeyTime(System.Windows.Input.Key.Space) == 1)
                PauseButtonPressed = !PauseButtonPressed;

            if (!PauseButtonPressed)
            {
                //фазы лесенкой, чтобы можно было хоть по всем пройтись

                if (_processPhase == EProcessPhase.beforeRound)
                {
                    _currentState.roundNumber++;
                    animationStage = 0;
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

                            var turn = _game.TryGetHumanTurn(_currentState, player, input);
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
                                new ExternalProgramExecuter(player.programAddress, "input.txt", "output.txt", null);//todo java path

                            string inputFile = _game.GetInputFile(_currentState, player);
                            string output;
                            string comment; //exitCode, нигде не используется

                            ExecuteResult res = epe.Execute(inputFile, 2, out output, out comment);
                            var turn = _game.GetProgramTurn(_currentState, player,output, res, ExternalProgramExecuter.ExecuteResultToString(res));
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
                    if (input.KeyTime(System.Windows.Input.Key.Enter) == 1)
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
            _game.DrawAll(frame, _currentState, animationStage, animationFinishStage, _processPhase == EProcessPhase.getTurnsOfNextRound, input);
            _currentState.frameNumber++;
            return frame;
        }


       
        private void UpdateAnimationStage()
        {
            animationStage += 1.0 / FrameworkSettings.FramesPerTurn;
            if (animationStage > animationFinishStage)
                animationStage = animationFinishStage;
        }

        public static string RunOnServerOrGetError(IGame<TState, TTurn, TRound, TPlayer> game, FormMainSettings settings, Action<object, string> RoundPlayed )
        {
            TState state = new TState();
            state.roundNumber = -1;
            state.Init(settings);

            if (state.players.Count == 0)
                return "Игра без игроков невозможна";

            while(!state.GameFinished)
            {
                state.roundNumber++;
                var round = new TRound();
                round.random = new System.Random(Rand.Next());
                round.turns = new List<TTurn>();

                List<TPlayer> order = game.GetTurnOrderForNextRound(state);
                while(round.turns.Count < order.Count){
                    TPlayer player = order[round.turns.Count];
                    ExternalProgramExecuter epe =
                                new ExternalProgramExecuter(player.programAddress, "input.txt", "output.txt", settings.JavaPath);

                            string input = game.GetInputFile(state, player);
                            string output;
                            string comment;

                            ExecuteResult res = epe.Execute(input, 2, out output, out comment);
                            var turn = game.GetProgramTurn(state, player,output, res,  ExternalProgramExecuter.ExecuteResultToString(res));
                            round.turns.Add(turn);
                }
                state.rounds.Add(round);
                game.ProcessRoundAndSetTotalStage(state, round);

                RoundPlayed(round, game.GetCurrentSituation(state));
            }

            return "Ok";
        }

        
    }
}
