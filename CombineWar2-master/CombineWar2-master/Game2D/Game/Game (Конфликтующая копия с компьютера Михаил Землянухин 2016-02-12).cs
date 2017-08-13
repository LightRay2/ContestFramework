using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game2D.Opengl;
using Game2D.Game.DataClasses;

namespace Game2D.Game
{
    class Game:IGame
    {
        Animator _animator;
        State _currentState ;
        Engine _engine ;
        TurnReceiver _turnReceiver ;

        Frame _previousFrame;

        int _currentGameNumber = -1;
        public static bool PauseButtonPressed=false;
        public Game()
        {
            TryRunNextGame();
        }

        public bool TryRunNextGame()
        {
            _currentGameNumber++;
            if(_currentGameNumber >= Const.ParamsFromMainFormToGame.Count )
                return false;
            var game = Const.ParamsFromMainFormToGame[_currentGameNumber];

            _animator = new Animator();
            _currentState = new State();
            _engine = new Engine();
            _previousFrame = null;

            string temp;
            if (!game.firstIsControlledByHuman)
            {
                temp = game.firstProgramAddress;
                if (temp != "")
                    _currentState.players[0].programAddress = temp;
            }
            if (!game.secondIsControlledByHuman)
            {
                temp = game.secondProgramAddress;
                if (temp != "")
                    _currentState.players[1].programAddress = temp;
            }
            Const.FramesPerTurn = Const.DefaultFramesPerTurn * game.AnimationSpeedInPercent / 100;

            _turnReceiver = new TurnReceiver(_currentState, _engine, game.javaPath, game.ThisIsReplayGame);
            if (game.ThisIsReplayGame)
            {
                bool success = _turnReceiver.TryLoadReplay(game.replayPath);
                if (!success)
                {
                    _currentState.Message = "Не удалось загрузить повтор. Нажмите Enter";
                    _currentState.IsFinished = true;
                }
            }
            _animator.Run(_currentState);

            return true;
        }

        public Frame Process(IGetKeyboardState keyboard)
        {
            if (keyboard.GetActionTime(EKeyboardAction.Esc) == 1) return null;
            if (keyboard.GetActionTime(EKeyboardAction.Fire) == 1)
                PauseButtonPressed = !PauseButtonPressed;
            if (_currentState.IsFinished && keyboard.GetActionTime(EKeyboardAction.Enter) == 1)
            {
                bool success = TryRunNextGame();
                if (!success)
                {
                    return null;
                }
            }


            Frame frame = new Frame();

            //todo в проекте неплохо бы дописать Vector2 и Point2 с перегрузками операторов
            // а то в текущей реализации приходится все действия с векторами каждый раз руками делать
            //например, отсюда можно взять реализацию https://docs.google.com/document/d/16MoZCrlhzYNvMA7woEYWId7pnEpIki3JONzpyk4YTLg/edit


            

            //если аниматор не работает, значит все уже отрисовано,
            //и надо получить и подкинуть ему новое состояние 
            if (_animator.IsFinished)
            {
                SimultaneousTurn nextTurn;
                if (!PauseButtonPressed && _turnReceiver.GetNextTurnAndComment(_currentState, keyboard,out nextTurn, ref frame))
                {
                    _engine.DoTurn(ref _currentState, nextTurn);

                    
                    //тут можно при желании добавить в состояние comment, чтобы он тоже было отрисован


                    //добавляем в аниматор обновленное состояние
                    _animator.Run(_currentState);

                    
                    
                }
            }
            

            _animator.DrawAll(ref frame, keyboard);

            return frame;
        }

        
    }
}
