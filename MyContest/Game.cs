﻿using Framework;
using OpenTK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;




namespace MyContest
{

    public class Round : IRound<Turn, Player>
    {
        public List<Turn> turns { get; set; }
        public Random random { get; set; }
        public double totalStage { get; set; }
        public string nameForTimeLine { get; set; }

        //здесь переменные, описывающие конкретный раунд
    }

    public class Turn : ITurn<Player>, ITimelineCell
    {
        internal Vector2d? ballAim;
        internal List<Vector2d> manAims;

        public string input { get; set; }
        public string output { get; set; }
        public Player player { get; set; }

        public Color colorOnTimeLine { get; set; }
        public Color colorStatusOnTimeLine { get; set; }
        public string nameOnTimeLine { get; set; }
    }

    public class Player : IPlayer
    {
        public string programAddress { get; set; }
        public bool controlledByHuman { get; set; }
        public string name { get; set; }

        public int team;

        public List<Man> manList = new List<Man>();
        internal int score;
    }
    public class GameParams
    {

    }

    public class Man
    {
        public Vector2d position;
        public int team;

        public Color Color { get; internal set; }
    }
    /// <summary>
    /// Везде point.X соответствует номеру строки
    /// </summary>
    public class Game : IGame<FormState, Turn, Round, Player>
    {
        public int roundNumber { get; set; }
        public int frameNumber { get; set; }
        public List<Player> players { get; set; }
        public List<Round> rounds { get; set; }
        public bool GameFinished { get; set; }
        public int clickedRound { get; set; }

        enum EFont { regular }
        enum ESprite { }



        List<Man> _manList = new List<Man>();

        int _ballOwner = -1;
        int _teamWithSpeedBonus = -1;
        bool _ballWasTouchedFirstTime = false;
        Random _rand;

        bool _useDefendersSpeedIncrease = true, _useDefendersLimit = true;

        GamePurpose _gameInstancePurpose;
        public Game(FormState settings, GamePurpose purpose)
        {
            clickedRound = -1;
            _rand = new Random(settings.RandomSeed);
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            _gameInstancePurpose = purpose;
            if (_gameInstancePurpose == GamePurpose.LoadSpritesAndFonts)
                return;



            #region create players
            players = settings.ProgramAddressesInMatch
                    .Select(index => settings.ProgramAddressesAll[index])
                    .Select(address => new Player
                    {
                        name = address == null ? "Человек" : Path.GetFileNameWithoutExtension(address),
                        controlledByHuman = address == null,
                        programAddress = address
                    }).ToList();

            for (int i = 0; i < 2; i++)
            {
                players[i].team = i;

            }
            #endregion

            var positions = new List<Vector2d>
            {
                 new Vector2d (20, 10),
                 new Vector2d(20, 25),
                 new Vector2d(20,40),
                 new Vector2d (60, 20),
                 new Vector2d(60,40),


                 new Vector2d (80, 10),
                 new Vector2d(80, 25),
                 new Vector2d(80,40),
                 new Vector2d (40, 20),
                 new Vector2d(40,40)
            };

            for (int i = 0; i < 10; i++)
            {
                var man = new Man
                {
                    team = i / 5,
                    Color = i / 5 == 0 ? Color.Blue : Color.Green,
                    position = positions[i]
                };
                _manList.Add(man);

                if (i < 5)
                    players[0].manList.Add(man);
                else
                    players[1].manList.Add(man);
            }

            _ballPosition = new Vector2d(50, 50 * _rand.NextDouble());
            _ballSpeedNormalized = _rand.Next(2) == 1 ? new Vector2d(0, 1) : new Vector2d(0, -1);
            _ballTimeLeft = BALL_MAX_TIME;

            FrameworkSettings.FramesPerTurn = 10;
        }

        const double BALL_SPEED = 8;
        double MAN_SPEED = BALL_SPEED * 0.3;
        double BALL_MAX_TIME = (int)(32.0 / BALL_SPEED);
        double _ballTimeLeft = 0;
        int _currentPlayerIndex;


        public string GetCurrentSituation()
        {
            return null;

        }

        Vector2d Reflected(Vector2d p)
        {
            if (_currentPlayerIndex == 0)
                return p;
            return new Vector2d(_arena.right - p.X, p.Y);
        }

        Vector2d _ballPosition, _ballSpeedNormalized;
        Rect2d _arena = new Rect2d(0, 0, 100, 50);
        int _turnLimit = 500;
        public string GetInputFile(Player player)
        {

            _currentPlayerIndex = player.team;
            var sb = new StringBuilder(); //todo turn number
            sb.AppendLine(string.Format("{0} {1} {2} {3}",
                Reflected(_ballPosition).X.Rounded(3), Reflected(_ballPosition).Y.Rounded(3),
                Reflected(_ballPosition + _ballSpeedNormalized* BALL_SPEED * _ballTimeLeft).X.Rounded(3),
                Reflected(_ballPosition + _ballSpeedNormalized * BALL_SPEED * _ballTimeLeft).Y.Rounded(3)));

            if (_currentPlayerIndex == 0)
            {
                foreach (var man in _manList)
                {
                    sb.AppendLine(string.Format("{0} {1}", (Reflected(man.position).X).Rounded(3), (Reflected(man.position).Y).Rounded(3)));
                }
            }
            else
            {
                foreach (var man in _manList.Skip(5))
                {
                    sb.AppendLine(string.Format("{0} {1}", Reflected(man.position).X.Rounded(3), Reflected(man.position).Y.Rounded(3)));
                }
                foreach (var man in _manList.Take(5))
                {
                    sb.AppendLine(string.Format("{0} {1}", Reflected(man.position).X.Rounded(3), Reflected(man.position).Y.Rounded(3)));
                }
            }

            return sb.ToString();
        }

        public Turn GetProgramTurn(Player player, string output, ExecuteResult executionResult, string executionResultRussianComment)
        {
            _currentPlayerIndex = player.team;
            var reader = new StringReader(output);
            var turn = new Turn {
                output = output,
                colorOnTimeLine = player.team == 0? Color.Blue : Color.Green,
             nameOnTimeLine = roundNumber.ToString(),
             colorStatusOnTimeLine = Color.Gold}; //todo now in interface just edit turn, no return
            turn.manAims = new List<Vector2d>();
            for (int i = 0; i < 5; i++)
                turn.manAims.Add(player.manList[i].position);

            var playerAims = new List<Vector2d>();
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    var s = reader.ReadLine().Split(' ');
                    playerAims.Add(Reflected(new Vector2d(double.Parse(s[0].Replace(",", ".")), double.Parse(s[1].Replace(",", ".")))));
                }

                turn.manAims = playerAims;
            }
            catch
            {

                return turn;
            }

            try
            {
                var s = reader.ReadLine().Split();
                turn.ballAim = Reflected(new Vector2d(double.Parse(s[0].Replace(",", ".")), double.Parse(s[1].Replace(",", "."))));
                return turn;
            }
            catch
            {
                return turn;
            }
        }



        public List<Player> GetTurnOrderForNextRound()
        {
            return players;
        }

        public void LoadSpritesAndFonts()
        {
            FontList.Load(EFont.regular, "Times New Roman", 3);
        }

        public void PreparationsBeforeRound()
        {

        }


        void KickBall(Vector2d where)
        {

            _ballIgnoreCollisionsWith = _ballOwner;
            _ballOwner = -1;

            _ballSpeedNormalized = (where - _ballPosition).Normalized();
            _ballTimeLeft = Math.Min(BALL_MAX_TIME, (where - _ballPosition).Length / BALL_SPEED);
        }

        public void ProcessRoundAndSetTotalStage(Round round)
        {
            int partCount = 100;

            var interpolationFunctionBall = new InterpolationFunction(_ballPosition, 1.0 / partCount);
            _ballAnimator = new Animator<Vector2d>(interpolationFunctionBall.GetInterpolated, Vector2d.Zero, Vector2d.Zero, 1); //todo govnokod

            var interpolateFunctionMan = new List<InterpolationFunction>();
            _manAnimators = new List<Animator<Vector2d>>();
            _manList.ForEach(x =>
            {
                interpolateFunctionMan.Add(new InterpolationFunction(x.position, 1.0 / partCount));
                _manAnimators.Add(new Animator<Vector2d>(interpolateFunctionMan.Last().GetInterpolated, Vector2d.Zero, Vector2d.Zero, 1));
            });

            var manSpeedsNormalized = new List<Vector2d>();
            var manAims = round.turns.SelectMany(x => x.manAims).ToList();
            for (int i = 0; i < _manList.Count; i++)
            {
                manSpeedsNormalized.Add((manAims[i] - _manList[i].position).Normalized());

            }

            var part = 1.0 / partCount;
            var thereWasCollisionWithMan = Enumerable.Repeat(false, 10).ToList();

            if (round.turns[0].ballAim != null && _ballOwner < 5 && _ballOwner >= 0)
            {
                KickBall(round.turns[0].ballAim.Value);
            }
            if (round.turns[1].ballAim != null && _ballOwner >= 5)
            {
                KickBall(round.turns[1].ballAim.Value);
            }


            for (int partIndex = 1; partIndex <= partCount; partIndex++)
            {




                //very simple collision
                var changePosition = Enumerable.Repeat(true, 10).ToList();
                var shiffledIndices = Enumerable.Range(0, 10).OrderBy(x=>_rand.Next()).ToList();
                for (int shuffledIndex = 0; shuffledIndex < shiffledIndices.Count; shuffledIndex++)
                {
                    int i = shiffledIndices[shuffledIndex];
                    //  bool _useDefendersSpeedIncrease, _useDefendersLimit;

                    var man = _manList[i];
                    if (thereWasCollisionWithMan[i] == false && (man.position - manAims[i]).Length <= MAN_SPEED * part)
                        changePosition[i] = false; //уже в пункте назначения, и никто его не двигает

                    if (changePosition[i])
                    {
                        double speedIncreaseCoeff = 1;
                        if (_useDefendersSpeedIncrease)
                        {
                            if ((i < 5 && _ballPosition.X < _arena.size.X / 2) || (i >= 5 && _ballPosition.X > _arena.size.X / 2))
                                speedIncreaseCoeff = 1.3;
                        }
                        var manWants = man.position + manSpeedsNormalized[i] * MAN_SPEED * part * speedIncreaseCoeff;

                        //collided with man
                        for (int j = 0; j < _manList.Count; j++)
                        {
                            if (i == j)
                                continue;

                            if ((manWants - _manList[j].position).Length < _manRadius * 2)
                            {
                                changePosition[i] = false;
                                changePosition[j] = false;
                                var p = manSpeedsNormalized[i];
                                manSpeedsNormalized[i] = manSpeedsNormalized[j];
                                manSpeedsNormalized[j] = p;
                                thereWasCollisionWithMan[i] = thereWasCollisionWithMan[j] = true;
                                if (_ballOwner == i)
                                    _ballOwner = j;
                                else if (_ballOwner == j)
                                    _ballOwner = i;
                            }
                        }

                        //collided with wall
                        if (changePosition[i])
                        {
                            double horizontalLimitLeft = 0, horizontalLimitRight = _arena.size.X;
                            //prohibited to go there
                            if (_useDefendersLimit)
                            {
                                if (i == 3 || i == 4)
                                {
                                    horizontalLimitLeft = _arena.size.X/2;
                                }
                                else if (i == 8 || i == 9)
                                {
                                    horizontalLimitRight = _arena.size.X/2;
                                }
                            }

                            if (manWants.X < horizontalLimitLeft + _manRadius || manWants.X > horizontalLimitRight - _manRadius)
                            {
                                manSpeedsNormalized[i] = new Vector2d(-manSpeedsNormalized[i].X, manSpeedsNormalized[i].Y);
                                changePosition[i] = false;
                            }
                            if (manWants.Y < _manRadius || manWants.Y > _arena.size.Y - _manRadius)
                            {
                                manSpeedsNormalized[i] = new Vector2d(manSpeedsNormalized[i].X, -manSpeedsNormalized[i].Y);
                                changePosition[i] = false;
                            }
                        }




                        if (changePosition[i])
                        {
                            man.position = manWants;
                        }

                    }


                    interpolateFunctionMan[i].Add(man.position);


                    //interactions with ball
                    if (_ballOwner == -1 && _ballIgnoreCollisionsWith != i)
                    {
                        if ((man.position - _ballPosition).Length <= _manRadius + _ballRadius)
                        {
                            _ballOwner = i;
                            _ballWasTouchedFirstTime = true;
                            _ballIgnoreCollisionsWith = -1;
                            _ballPosition = man.position;
                            if (_ballOwner < 5 && round.turns[0].ballAim != null)
                                KickBall(round.turns[0].ballAim.Value);
                            else if (_ballOwner >= 5 && round.turns[1].ballAim != null)
                                KickBall(round.turns[1].ballAim.Value);
                        }
                    }


                }



                if (_ballOwner == -1)
                {
                    if (_ballTimeLeft > 0)
                    {
                        var ballPart = Math.Min(_ballTimeLeft, part);
                        var ballWants = _ballPosition + ballPart * BALL_SPEED * _ballSpeedNormalized;
                        bool changePositionBall = true;
                        if (ballWants.X < _ballRadius || ballWants.X > _arena.size.X - _ballRadius)
                        {
                            _ballSpeedNormalized.X = -_ballSpeedNormalized.X;
                            changePositionBall = false;
                            _ballIgnoreCollisionsWith = -1;
                            if (ballWants.X < _ballRadius)
                                Goal(1);
                            else
                                Goal(0);
                        }
                        if (ballWants.Y < _ballRadius || ballWants.Y > _arena.size.Y - _ballRadius)
                        {
                            _ballSpeedNormalized.Y = -_ballSpeedNormalized.Y;
                            changePositionBall = false;
                            _ballIgnoreCollisionsWith = -1;


                        }

                        if (changePositionBall)
                        {
                            _ballPosition = ballWants;
                        }
                    }




                }
                else
                {
                    _ballPosition = _manList[_ballOwner].position;
                }
                interpolationFunctionBall.Add(_ballPosition);

                if (_ballTimeLeft > 0)
                    _ballTimeLeft -= part;
                if (_ballOwner != -1)
                    _ballTimeLeft = 0;
                if (_ballTimeLeft.DoubleEqual(0))
                    _ballIgnoreCollisionsWith = -1;
                //stop when goal

                //if(_ballWasTouchedFirst)


                //players

                //if ballowner != -1

            }



            round.totalStage = 1;
        }

        void Goal(int team)
        {
            players[team].score++;
            if (team == 0)
                _ballOwner = players[(team+1)%2].manList.IndexOf(players[(team + 1) % 2].manList.OrderByDescending(x => x.position.X).First());
            else
                _ballOwner = players[(team + 1) % 2].manList.IndexOf(players[(team + 1) % 2].manList.OrderBy(x => x.position.X).First());
            _ballPosition = _manList[_ballOwner].position;
        }

        public void DrawAll(Frame frame, double stage, double totalStage, bool humanMove, GlInput input) //todo human move??
        {
            //!!! будьте внимательны (ранний drawall перед любыми методами)
            if (_manAnimators.Count == 0)
                return;//todo bad for manual
            int frameWidth = 120, frameHeight = 90;
            frame.CameraViewport(frameWidth, frameHeight);
            
            frame.Polygon(Color.Wheat, new Rect2d(0, 0, frameWidth, frameHeight)); //todo line around polygon

            var fieldCorner = new Vector2d(8, 10);
            var lineWidth = 0.4;
            frame.Path(Color.Black, lineWidth, _arena + fieldCorner);
            frame.Path(Color.Black, lineWidth, fieldCorner + new Vector2d(_arena.size.X / 2, 0), fieldCorner + new Vector2d(_arena.size.X / 2, _arena.size.Y));

            for (int i = 0; i < _manList.Count; i++)
            {
                var man = _manList[i];

                frame.Circle(man.Color, _manAnimators[i].Get(stage) + fieldCorner, _manRadius);
            }

            var curMan = _manAnimators[6].Get(stage);



            frame.Circle(Color.Gray, _ballAnimator.Get(stage) + fieldCorner, _ballRadius);


            frame.TextTopLeft(EFont.regular, players[0].score.ToString(), 2, 2); //todo framework text without declaration?
            frame.TextCustomAnchor(EFont.regular, players[1].score.ToString(), 1, 0, 110, 2);



            #region timeline2
            {
                double tileStatusWidth = 1.5;
                var fullTimeLineRect = new Rect2d(frameWidth - _timeline._tileWidth, 0, _timeline._tileWidth, frameHeight);
                //if(_indexOfLastViewedTile != -1){
                var turns = this.rounds.SelectMany(x => x.turns).ToList();
                if (turns.Count > 0)
                {
                    _timeline._indexOfLastViewedTile = turns.Count - 1;
                    var clickedTurn  = _timeline.ManageTimelineByInputAndGetClickedTurn(frame, input, fullTimeLineRect, turns.Count);
                    if(clickedTurn != -1)
                    {
                        Clipboard.SetText(turns[clickedTurn].input);
                        clickedRound = rounds.IndexOf( rounds.First(r => r.turns.Contains(turns[clickedTurn])));
                    }
                    _timeline.Draw(frame, input, turns.Cast<ITimelineCell>().ToList(), _timeline._firstTurnOffset, _timeline._indexOfLastViewedTile,
                        new Vector2d(frameWidth - _timeline._tileWidth, 0), frameHeight, _timeline._tileWidth, _timeline._tileHeight, tileStatusWidth, EDirections.down);
                }
                // }
            }
            #endregion
        }

        Timeline _timeline = new Timeline(EFont.regular, 5,5);
        public Turn TryGetHumanTurn(Player player, GlInput input)
        {
            return new Turn();
        }

        Animator<Vector2d> _ballAnimator;
        List<Animator<Vector2d>> _manAnimators = new List<Animator<Vector2d>>();
        private double _ballRadius = 1;
        double _manRadius = 2;
        private int _ballIgnoreCollisionsWith;
    }
}