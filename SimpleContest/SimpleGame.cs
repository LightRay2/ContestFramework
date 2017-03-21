﻿using Framework;
using OpenTK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;




namespace SimpleContest
{
    #region small classes
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
        public string shortStatus { get; set; }

        public Color colorOnTimeLine { get; set; }
        public Color colorStatusOnTimeLine { get; set; }
        public Enum fontOnTimeLine { get; set; }
        public string nameOnTimeLine { get; set; }
        public Turn()
        {
            fontOnTimeLine = SimpleGame.EFont.timelineNormal;
        }
    }

    public class Player : IPlayer
    {
        public string programAddress { get; set; }
        public bool controlledByHuman { get; set; }
        public string name { get; set; }

        public int team;

        public List<Man> manList = new List<Man>();
        internal int score;
        public string memoryFromPreviousTurn = null;
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
    #endregion

    /// <summary>
    /// Везде point.X соответствует номеру строки
    /// </summary>
    public class SimpleGame : IGame<FormState, Turn, Round, Player>
    {
        public int roundNumber { get; set; }
        public int frameNumber { get; set; }
        public List<Player> players { get; set; }
        public List<Round> rounds { get; set; }
        public bool GameFinished { get; set; }
        public int clickedRound { get; set; }

        public enum EFont
        {
            timelineNormal,
            timelineError
        }
        enum ESprite { }



        List<Man> _manList = new List<Man>();
        List<Vector2d> _ballList = new List<Vector2d>();
        List<Tuple<Vector2d, double>> _ballFantom = new List<Tuple<Vector2d, double>>();
        Random _rand;


        List<Animator<Vector2d>> _manAnimators = new List<Animator<Vector2d>>();
        private double _ballRadius = 1;
        double _manRadius = 2;

        double MAN_SPEED = 5;

        GamePurpose _gameInstancePurpose;
        public SimpleGame(FormState settings, GamePurpose purpose)
        {
            #region constructor
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
                 new Vector2d (10, 10),
                 new Vector2d(90, 90)
            };

            _manAnimators = new List<Animator<Vector2d>>();
            for (int i = 0; i < 2; i++)
            {
                var man = new Man
                {
                    team = i,
                    Color = i == 0 ? Color.Blue : Color.Green,
                    position = positions[i]
                };
                _manList.Add(man);

                if (i < 1)
                    players[0].manList.Add(man);
                else
                    players[1].manList.Add(man);

                _manAnimators.Add(new Animator<Vector2d>(Animator.Linear, man.position, man.position, 1));
            }

            #endregion
        }


        public static void SetFrameworkSettings()
        {
            FrameworkSettings.GameNameEnglish = "SimpleGame";
            FrameworkSettings.RunGameImmediately = false;
            FrameworkSettings.AllowFastGameInBackgroundThread = true;
            FrameworkSettings.FramesPerTurn = 20;

            FrameworkSettings.PlayersPerGameMin = 2;
            FrameworkSettings.PlayersPerGameMax = 2;
            FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create(Path.GetDirectoryName(Application.StartupPath) + "//..//Players//Easy.exe", true));
            FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create(Path.GetDirectoryName(Application.StartupPath) + "//..//Players//Normal.exe", true));
            //  FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create(Path.GetDirectoryName(Application.StartupPath) + "//..//Players//Hard.exe", false));

            FrameworkSettings.Timeline.Enabled = true;
            FrameworkSettings.Timeline.Position = TimelinePositions.right;
            FrameworkSettings.Timeline.TileLength = 7;
            FrameworkSettings.Timeline.TileWidth = 7;
            FrameworkSettings.Timeline.FontNormalTurn = EFont.timelineNormal;
            FrameworkSettings.Timeline.FontErrorTurn = EFont.timelineError;
            FrameworkSettings.Timeline.TurnScrollSpeedByMouseOrArrow = 8;
            FrameworkSettings.Timeline.TurnScrollSpeedByPageUpDown = 100;
            FrameworkSettings.Timeline.FollowAnimationTimeMs = 600;
        }

        public void LoadSpritesAndFonts()
        {
            if (FontList.All.Count == 0 && SpriteList.All.Count == 0)
            {
                FontList.Load(EFont.timelineNormal, "Times New Roman", 2.0);
                FontList.Load(EFont.timelineError, "Times New Roman", 2.0, Color.Red, FontStyle.Bold);
            }
        }
        public string GetCurrentSituation()
        {
            return null;

        }


        Rect2d _arena = new Rect2d(0, 0, 100, 100);
        int _turnLimit = 100;
        public string GetInputFile(Player player)
        {

            var sb = new StringBuilder(); //todo turn number
            sb.AppendLine(string.Format("{0} {1} {2}", roundNumber, players[player.team].score, players[(player.team + 1) % 2].score));

            sb.AppendLine(string.Format("{0} {1}", _manList[player.team].position.X.Rounded(3), _manList[player.team].position.Y.Rounded(3)));
            sb.AppendLine(string.Format("{0} {1}", _manList[(player.team + 1) % 2].position.X.Rounded(3), _manList[(player.team + 1) % 2].position.Y.Rounded(3)));

            sb.AppendLine(_ballList.Count.ToString());
            _ballList.ForEach(ball =>
            {
                sb.AppendLine(string.Format("{0} {1}", ball.X.Rounded(3), ball.Y.Rounded(3)));

            });

            sb.AppendLine(player.memoryFromPreviousTurn ?? "-1");

            return sb.ToString();
        }

        public Turn GetProgramTurn(Player player, string output, ExecuteResult executionResult, string executionResultRussianComment)
        {
            var turn = new Turn
            {
                shortStatus = executionResultRussianComment,
                output = output,
                colorOnTimeLine = player.team == 0 ? Color.Blue : Color.Green,
                nameOnTimeLine = roundNumber.ToString(),
                colorStatusOnTimeLine = Color.Gold
            }; //todo now in interface just edit turn, no return

            turn.manAims = new List<Vector2d>();
            for (int i = 0; i < 1; i++)
                turn.manAims.Add(player.manList[i].position);


            if (executionResult == ExecuteResult.Ok)
            {
                var reader = new StringReader(output);



                var playerAims = new List<Vector2d>();
                try
                {
                    for (int i = 0; i < 1; i++)
                    {
                        var s = reader.ReadLine().Split(' ');
                        playerAims.Add(new Vector2d(double.Parse(s[0].Replace(",", ".")), double.Parse(s[1].Replace(",", "."))));
                        CheckDoubleWithException(playerAims.Last().X);
                        CheckDoubleWithException(playerAims.Last().Y);
                    }

                    turn.manAims = playerAims;
                }
                catch
                {
                    turn.fontOnTimeLine = EFont.timelineError;
                    turn.shortStatus = "Неправильный формат вывода";
                    return turn;
                }

                try
                {
                    var nextString = reader.ReadLine();
                    if (nextString.StartsWith("memory "))
                        player.memoryFromPreviousTurn = nextString.Substring(7);


                    return turn;
                }
                catch
                {
                    return turn;
                }
            }
            return turn;
        }

        void CheckDoubleWithException(double x)
        {
            if (double.IsInfinity(x) || double.IsNaN(x))
                throw new Exception();
        }

        public List<Player> GetTurnOrderForNextRound()
        {
            return players;
        }


        public void PreparationsBeforeRound()
        {

        }


        public void ProcessRoundAndSetTotalStage(Round round)
        {
            _ballFantom = new List<Tuple<Vector2d, double>>();
            if (roundNumber % 2 == 0)
                _ballList.Add(new Vector2d(_rand.NextDouble() * 98 + 1, _rand.NextDouble() * 98 + 1));
            int partCount = 100;

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
                if ((manAims[i] - _manList[i].position).Length.DoubleLessOrEqual(0))
                    manSpeedsNormalized.Add(Vector2d.UnitX * 0.00000001);
                else
                    manSpeedsNormalized.Add((manAims[i] - _manList[i].position).Normalized());

            }

            var part = 1.0 / partCount;
            var thereWasCollisionWithMan = Enumerable.Repeat(false, 2).ToList();



            for (int partIndex = 1; partIndex <= partCount; partIndex++)
            {



                //todo ball with wall
                var changePosition = Enumerable.Repeat(true, 2).ToList();
                var shiffledIndices = Enumerable.Range(0, 2).OrderBy(x => _rand.Next()).ToList();
                var alreadyExchangedSpeed = new List<Tuple<int, int>>();
                for (int shuffledIndex = 0; shuffledIndex < shiffledIndices.Count; shuffledIndex++)
                {
                    int i = shiffledIndices[shuffledIndex];
                    double manSpeed = MAN_SPEED;
                    var man = _manList[i];
                    if (thereWasCollisionWithMan[i] == false && (man.position - manAims[i]).Length <= MAN_SPEED * part)
                        changePosition[i] = false; //уже в пункте назначения, и никто его не двигает

                    if (changePosition[i])
                    {
                        #region перемещение игрока
                        var manWants = man.position + manSpeedsNormalized[i] * manSpeed * part;

                        //collided with man
                        for (int j = 0; j < _manList.Count; j++)
                        {
                            if (i == j)
                                continue;

                            if ((manWants - _manList[j].position).Length < _manRadius * 2)
                            {
                                thereWasCollisionWithMan[i] = thereWasCollisionWithMan[j] = true;
                                changePosition[i] = false;

                                if (alreadyExchangedSpeed.Contains(Tuple.Create(i, j)) == false)
                                {
                                    var newSpeeds = Engine.ProcessManCollision(man.position, _manList[j].position, manSpeedsNormalized[i], manSpeedsNormalized[j]);
                                    manSpeedsNormalized[i] = newSpeeds.Item1;
                                    manSpeedsNormalized[j] = newSpeeds.Item2;
                                    alreadyExchangedSpeed.Add(Tuple.Create(i, j));
                                    alreadyExchangedSpeed.Add(Tuple.Create(j, i));

                                }


                            }
                        }

                        //collided with wall
                        if (changePosition[i])
                        {
                            double horizontalLimitLeft = 0, horizontalLimitRight = _arena.size.X;

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
                        #endregion

                    }


                    interpolateFunctionMan[i].Add(man.position);


                    //interactions with ball
                    for (int j = 0; j < _ballList.Count; j++)
                    {
                        if ((man.position - _ballList[j]).Length <= _manRadius + _ballRadius)
                        {
                            _ballFantom.Add(Tuple.Create(_ballList[j], partIndex * part));
                            _ballList.RemoveAt(j--);
                            players[i].score++;
                        }
                    }


                }



                //stop when goal

                //if(_ballWasTouchedFirst)


                //players

                //if ballowner != -1

            }


            if (roundNumber == 99)
                GameFinished = true;
            round.totalStage = 1;
        }


        public void DrawAll(Frame frame, double stage, double totalStage, bool humanMove, GlInput input) //todo human move??
        {
            //!!! будьте внимательны (ранний drawall перед любыми методами)
            int frameWidth = 160, frameHeight = 120;
            frame.CameraViewport(frameWidth, frameHeight);

            frame.Polygon(Color.Wheat, new Rect2d(0, 0, frameWidth, frameHeight)); //todo line around polygon

            var fieldCorner = new Vector2d(10, 10);
            var lineWidth = 0.4;
            frame.Path(Color.Black, lineWidth, _arena + fieldCorner);
            //  frame.Path(Color.Black, lineWidth, fieldCorner + new Vector2d(_arena.size.X / 2, 0), fieldCorner + new Vector2d(_arena.size.X / 2, _arena.size.Y));

            // if (_manAnimators.Count != 0) //т е еще не было process turn
            //  {



            for (int i = 0; i < _manList.Count; i++)
            {
                var man = _manList[i];

                frame.Circle(man.Color, _manAnimators[i].Get(stage) + fieldCorner, _manRadius);
            }

            _ballList.ForEach(ball => frame.Circle(Color.Gray, ball + fieldCorner, _ballRadius));

            _ballFantom.ForEach(ball =>
            {
                if (ball.Item2 >= stage)
                    frame.Circle(Color.Gray, ball.Item1 + fieldCorner, _ballRadius);
            });


            // }

            frame.TextTopLeft(EFont.timelineNormal, players[0].name+": "+ players[0].score.ToString(), 3, 3);
            frame.TextCustomAnchor(EFont.timelineNormal, players[1].name + ": " + players[1].score.ToString(), 1, 0, 107, 3);


        }

        public Turn TryGetHumanTurn(Player player, GlInput input)
        {
            return new Turn();
        }

    }
}
