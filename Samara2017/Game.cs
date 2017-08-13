using Framework;
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
using System.Xml.Serialization;

namespace Samara2017
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
        public Vector2d manAim;

        public string input { get; set; }
        public string output { get; set; }
        public Player player { get; set; }
        public string shortStatus { get; set; }


        [XmlIgnore]
        public Color colorOnTimeLine { get; set; }
        public int ColorArgb { get { return colorOnTimeLine.ToArgb(); } set { colorOnTimeLine = Color.FromArgb(value); } }
        [XmlIgnore]
        public Enum fontOnTimeLine { get; set; }
        public string FontOnTimeLineString { get { return fontOnTimeLine.ToString(); } set { fontOnTimeLine = (EFont)Enum.Parse(typeof(EFont), value); } }
        public string nameOnTimeLine { get; set; }
        public bool WantsShoot { get; internal set; }
        public int GoType { get; internal set; }

        public Turn()
        {
            fontOnTimeLine = EFont.timelineNormal;
        }
    }

    public class Player : IPlayer
    {
        public string programAddress { get; set; }
        public bool controlledByHuman { get; set; }
        public string name { get; set; }
        public bool Alive { get; internal set; }

        public int team;
        public string lastInput;

        [XmlIgnore]
        public Man man;
        internal int score;
        
        internal int lastShootingRound;
        internal Color color;
        public Player() { Alive = true; }
    }
    public class GameParams
    {

    }

    public class Man
    {
        public Vector2d position;
        public int team;
        internal int lastFreezed=0;
        internal int hp=100;
        internal Vector2d speedVector;

        public Color Color { get; internal set; }
    }

    public class Shell
    {
        public Vector2d position;
        internal Vector2d vectorSpeed;

        public Player Owner;
    }
    #endregion
    public enum EFont
    {
        timelineNormal,
        timelineError,
        playerNumbers,
        TeamOne,
        TeamTwo,
        Time,
        CoordsOnField,
        Goal,
        ScoreOne,
        ScoreTwo,
        Possession,
        black
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

        public int MAN_RADIUS = 30;

        public double SHELL_RADIUS = 5;

        Rect2d _arena = new Rect2d(0, 0, 1000, 750);

        enum ESprite
        {
            
        }


        

        GamePurpose _gameInstancePurpose;
        FormState _formState;
        public Game(FormState settings, GamePurpose purpose)
        {
            _formState = settings;
            #region constructor
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

            var startPositions = (new Rect2d(0,0,900, 650) + new Vector2d(50)).points;
            var colors = new List<Color> { Color.Red, Color.Gray, Color.Violet, Color.SkyBlue };
            for (int i = 0; i < 4; i++)
            {
                players[i].team = i;
                players[i].man = new Man { position = startPositions[i], lastFreezed = -10000, hp = 100, speedVector =  - Vector2d.UnitY*1000 /*_arena.center*/ };
                players[i].color = colors[i];
                players[i].lastShootingRound = -10000;
                _manAnimators.Add(new Animator<Vector2d>(Animator.Linear, players[i].man.position, players[i].man.position, 1));

            }
            _manList = players.Select(x => x.man).ToList();

            #endregion

            _walls = new List<Rect2d>
            {
                new Rect2d (100,100, 30, 30),
                new Rect2d (130,100, 30, 30),
                new Rect2d (160,100, 30, 30),
                new Rect2d (190,100, 30, 30),
                new Rect2d (220,100, 30, 30),
                new Rect2d (250,100, 30, 30),
                new Rect2d (250,130, 30, 30),
            };


            #endregion
        }
        

        public static void SetFrameworkSettings()
        {
            FrameworkSettings.GameNameEnglish = "Samara2017";
            FrameworkSettings.RunGameImmediately = true;
            FrameworkSettings.AllowFastGameInBackgroundThread = false;
            FrameworkSettings.FramesPerTurn = 7;
            FrameworkSettings.ExecutionTimeLimitSeconds = 1;
            FrameworkSettings.ForInnerUse.TimerInterval  = 8;

            FrameworkSettings.PlayersPerGameMin = 4;
            FrameworkSettings.PlayersPerGameMax = 4;
            //FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create("..//Players//Easy.exe", true));
            //FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create("..//Players//Easy.exe", true));
            // FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create( "..//Players//Normal.exe", false));
            //  FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create( "..//Players//Hard.exe", false));
            //  FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create("..//Players//VeryHard.exe", true));
            //  FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create( "..//Players//Extreme.exe", false));

            FrameworkSettings.Timeline.Enabled = true;
            FrameworkSettings.Timeline.Position = TimelinePositions.right;
            FrameworkSettings.Timeline.TileLength = 40;
            FrameworkSettings.Timeline.TileWidth = 40;
            //  FrameworkSettings.Timeline.FontNormalTurn = EFont.timelineNormal;
           //  FrameworkSettings.Timeline.FontErrorTurn = EFont.timelineError;
            FrameworkSettings.Timeline.TurnScrollSpeedByMouseOrArrow = 4;
            FrameworkSettings.Timeline.TurnScrollSpeedByPageUpDown = 100;
            FrameworkSettings.Timeline.FollowAnimationTimeMs = 600;

            FrameworkSettings.ForInnerUse.GetAllUserTurnsImmediately = true;
        }

        public void LoadSpritesAndFonts()
        {
            if (FontList.All.Count == 0 && SpriteList.All.Count == 0)
            {
                FontList.Load(EFont.ScoreOne, "Times New Roman", 20.0, Color.FromArgb(150, 0, 180, 230), FontStyle.Bold);



                FontList.Load(EFont.timelineNormal, "Times New Roman", 12, Color.FromArgb(40, 40, 40));
                FontList.Load(EFont.timelineError, "Times New Roman", 14, Color.Red, FontStyle.Bold);
                FontList.Load(EFont.black, "Times New Roman", 15, Color.Black);
                //FontList.Load(EFont.Time, "Times New Roman", 2.0, Color.FromArgb(200, 200, 200), FontStyle.Bold);
                //FontList.Load(EFont.TeamOne, "Times New Roman", 2.0, Color.FromArgb(200, 0, 180, 230), FontStyle.Bold);
                //FontList.Load(EFont.TeamTwo, "Times New Roman", 2.0, Color.FromArgb(200, 180, 140, 0), FontStyle.Bold);
                //FontList.Load(EFont.ScoreTwo, "Times New Roman", 4.0, Color.FromArgb(255, 180, 140, 0), FontStyle.Bold);
                ////  SpriteList.Load(ESprite.green);
                //FontList.Load(EFont.CoordsOnField, "Times New Roman", 1.5, Color.FromArgb(200, 200, 200), FontStyle.Bold);
                //FontList.Load(EFont.Possession, "Times New Roman", 1.5, Color.FromArgb(150, 150, 150), FontStyle.Bold);
                //FontList.Load(EFont.Goal, "Lucida Console", 20, Color.FromArgb(150, 220, 0, 0), FontStyle.Bold);

            }
        }

        public void PreparationsBeforeRound()
        {
            players.ForEach(p => p.lastInput = GetInputFile(p));
        }


        public List<Player> GetTurnOrderForNextRound()
        {
            return players;
        }


        public string GetCurrentSituation()
        {
            return null;
        }
        
        public string GetInputFile(Player player)
        {
            //round number
            //player count 
            //player positions, player looks at, hp, time to go,       base position, time to end defence, 0-1 if has bomb
            //wall count
            //wall center positions
            //shell count
            //shell positions, finishPositions
           
            //bomb count
            //bomb positions
            //bonus bomb count
            //bonus bomb positions
            //bonus defence count
            //bonus defence positions


            var sb = new StringBuilder();
            sb.AppendLine(roundNumber.ToString());
            sb.AppendLine(players.Count(x => x.Alive).ToString());
            var our = players.First(x => x.team == player.team);
            AppendPlayer(sb, our);
            foreach (var p in players.Where(x => x.Alive && x.team != player.team))
                AppendPlayer(sb, p);

            sb.AppendLine(_walls.Count.ToString());
            _walls.ForEach(x => sb.AppendLine(string.Format("{0} {1}", x.center.X.Rounded(0), x.center.Y.Rounded(0))));

            sb.AppendLine(_shellList.Count.ToString());
            _shellList.ForEach(x => sb.AppendLine(string.Format("{0} {1} {2} {3}", x.position.X.Rounded(0), x.position.Y.Rounded(0)
                , (x.vectorSpeed ).Normalized().X, (x.vectorSpeed ).Normalized().Y)));


            return sb.ToString();
        }

        private void AppendPlayer(StringBuilder sb, Player our)
        {
            sb.AppendLine(string.Format("{0} {1} {2} {3} {4} {5}", our.man.position.X.Rounded(0), our.man.position.Y.Rounded(0),
                (our.man.speedVector).Normalized().X.Rounded(0), (our.man.speedVector).Normalized().Y.Rounded(0),
                our.man.hp, Math.Max(0, 300 / SPEED_FASTER - (roundNumber - our.man.lastFreezed))));
        }

        public Turn GetProgramTurn(Player player, string output, ExecuteResult executionResult, string executionResultRussianComment)
        {
            var turn = new Turn
            {
                shortStatus = executionResultRussianComment,
                output = output,
                colorOnTimeLine = player.color,
                nameOnTimeLine = roundNumber.ToString(),
                player = player,
                fontOnTimeLine = EFont.timelineNormal
            }; //todo now in interface just edit turn, no return

            if (executionResult == ExecuteResult.Ok)
            {
                try
                {
                    var reader = new StringReader(output);
                    string[] outputLines;
                    outputLines = reader.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var line = outputLines[0].Split();
                    var goType = int.Parse(line[0])
                        ;
                    var angleTo = new Vector2d(double.Parse(line[1].Replace(",", ".")), double.Parse(line[2].Replace(",", ".")));
                    var shoot = int.Parse(line[3]) == 1;

                    turn.WantsShoot = shoot;
                    turn.manAim = angleTo;
                    turn.GoType = goType;
                }
                catch
                {
                    turn.shortStatus = "Неправильный формат вывода";
                    turn.fontOnTimeLine = EFont.timelineError;
                }
            }
            else
                turn.fontOnTimeLine = EFont.timelineError;


            return turn;
        }

        void CheckDoubleWithException(double x)
        {
            if (double.IsInfinity(x) || double.IsNaN(x))
                throw new Exception();
        }




        Random _rand;
        List<Man> _manList = new List<Man>();
        List<Animator<Vector2d>> _manAnimators = new List<Animator<Vector2d>>();
        List<Shell> _shellList = new List<Shell>();
        List<Animator<Vector2d>> _shellAnimators = new List<Animator<Vector2d>>();
        List<Rect2d> _walls = new List<Rect2d>();

        
        int SPEED_FASTER = 2;

        public void ProcessRoundAndSetTotalStage(Round round)
        {
            int partCount = 5;



            //shooting
            var createShells = round.turns.Select(x => x.WantsShoot && roundNumber - x.player.lastShootingRound >= 40 / SPEED_FASTER &&
                (roundNumber - x.player.man.lastFreezed >= 300 / SPEED_FASTER)).ToList();//
            for (int i = 0; i < players.Count; i++)
            {
                if (createShells[i])
                {
                    var vector = players[i].man.speedVector ; //
                    var shellPos = players[i].man.position + vector.Normalized() * (5 + MAN_RADIUS);
                    _shellList.Add(new Shell
                    {
                        vectorSpeed = vector * 1000,
                        position = shellPos,
                        Owner = players[i]
                    });
                    players[i].lastShootingRound = roundNumber;
                }
            }


            //interpolations
            var interpolateFunctionMan = new List<InterpolationFunction>();
            _manAnimators = new List<Animator<Vector2d>>();
            _manList.ForEach(x =>
            {
                interpolateFunctionMan.Add(new InterpolationFunction(x.position, 1.0 / partCount));
                _manAnimators.Add(new Animator<Vector2d>(interpolateFunctionMan.Last().GetInterpolated, Vector2d.Zero, Vector2d.Zero, 1));
            });

            var interpolateFunctionShell = new List<InterpolationFunction>();
            _shellAnimators = new List<Animator<Vector2d>>();
            _shellList.ForEach(x =>
            {
                interpolateFunctionShell.Add(new InterpolationFunction(x.position, 1.0 / partCount));
                _shellAnimators.Add(new Animator<Vector2d>(interpolateFunctionShell.Last().GetInterpolated, Vector2d.Zero, Vector2d.Zero, 1));
            });


            

            //aims for man and shells
            var manSpeedsNormalized = new List<Vector2d>();
            var manAngleChange = new List<double>();
            for(int i = 0; i < 4; i++)
            {
                var man = _manList[i];
                var pos = man.position;
                var wantsPointRelative = round.turns[i].manAim - pos;
                var nowRelative = man.speedVector;
                var angle = wantsPointRelative.AngleDeg(nowRelative);

                manSpeedsNormalized.Add(nowRelative.Normalized());
                var goType = round.turns[i].GoType;
                if (goType == 1)
                    manSpeedsNormalized[i] = manSpeedsNormalized[i] * (-1);
                else if (goType == 2)
                    manSpeedsNormalized[i] = manSpeedsNormalized[i].PerpendicularRight;
                else if(goType == 3)
                    manSpeedsNormalized[i] = manSpeedsNormalized[i].PerpendicularLeft;

                if (angle > 180)
                    angle = angle-360;
                manAngleChange.Add(Math.Min(0.1 / Math.PI * 180,  angle)); 

                if ((roundNumber - man.lastFreezed < 300 / SPEED_FASTER))
                {
                    manAngleChange[i] = 0.0001;
                    manSpeedsNormalized[i] = new Vector2d(0.000001);
                }
            }
            //var manTurnAim = round.turns.Select(turn =>
            //{

            //});
            //for (int i = 0; i < _manList.Count; i++)
            //{
            //    if ((manAims[i] - _manList[i].position).Length.DoubleLessOrEqual(0))
            //        manSpeedsNormalized.Add(Vector2d.UnitX * 0.00000001);
            //    else
            //        manSpeedsNormalized.Add((manAims[i] - _manList[i].position).Normalized());
            //}

            var shellSpeedNormalized = _shellList.Select(x => (x.vectorSpeed).Normalized()).ToList();



            var thereWasCollisionWithMan = Enumerable.Repeat(false, 10).ToList();
            //animate
            for (int partIndex = 1; partIndex <= partCount; partIndex++)
            {
                for (int i = 0; i < _shellList.Count; i++)
                {
                    _shellList[i].position += shellSpeedNormalized[i]*10*SPEED_FASTER / partCount;
                    interpolateFunctionShell[i].Add(_shellList[i].position);
                    

                    var manAimed = _manList.FirstOrDefault(man => (_shellList[i].position - man.position).Length < SHELL_RADIUS + MAN_RADIUS);
                    if(manAimed != null)
                    {
                        
                        if (roundNumber  -manAimed.lastFreezed >= 300 / SPEED_FASTER)
                        {
                            manAimed.hp -= 10;
                            _shellList[i].Owner.score += 10;
                            if (manAimed.hp <= 0)
                            {
                                manAimed.hp = 50;
                                manAimed.lastFreezed = roundNumber;
                            }
                        }

                        _shellAnimators.RemoveAt(i);
                        interpolateFunctionShell.RemoveAt(i);
                        _shellList.RemoveAt(i);
                        shellSpeedNormalized.RemoveAt(i);
                        i--;
                        continue;
                    }

                    if (_walls.Any(wall => GeomHelper.IsCircleIntersectsRect(_shellList[i].position, SHELL_RADIUS, wall))
                        || GeomHelper.PointInSimpleRect(_shellList[i].position, _arena) == false)
                    {
                        _shellList.RemoveAt(i);
                        _shellAnimators.RemoveAt(i);
                        interpolateFunctionShell.RemoveAt(i);
                        shellSpeedNormalized.RemoveAt(i);
                        i--;
                    }
                }

                //todo ball with wall
                var changePosition = Enumerable.Repeat(true, 10).ToList();
                var alreadyExchangedSpeed = new List<Tuple<int, int>>();
                for (int i = 0; i < 4; i++)
                {
                   
                    var man = _manList[i];
                    double manSpeed = 2.0* SPEED_FASTER / partCount;
                    if (roundNumber - man.lastFreezed < 300 / SPEED_FASTER)
                        manSpeed = 0.000001;
                    //  bool _useDefendersSpeedIncrease, _useDefendersLimit;

                    manSpeedsNormalized[i] = manSpeedsNormalized[i].RotateDeg(manAngleChange[i] / partCount);
                    man.speedVector =  man.speedVector.RotateDeg(manAngleChange[i] / partCount);

                    if (changePosition[i])
                    {
                        #region перемещение игрока
                        var manWants = man.position + manSpeedsNormalized[i] * manSpeed;

                        //collided with man
                        for (int j = 0; j < _manList.Count; j++)
                        {
                            if (i == j)
                                continue;

                            if ((manWants - _manList[j].position).Length < MAN_RADIUS * 2)
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

                            if (manWants.X < horizontalLimitLeft + MAN_RADIUS || manWants.X > horizontalLimitRight - MAN_RADIUS)
                            {
                                manSpeedsNormalized[i] = new Vector2d(-manSpeedsNormalized[i].X, manSpeedsNormalized[i].Y);
                                changePosition[i] = false;
                            }
                            if (manWants.Y < MAN_RADIUS || manWants.Y > _arena.size.Y - MAN_RADIUS)
                            {
                                manSpeedsNormalized[i] = new Vector2d(manSpeedsNormalized[i].X, -manSpeedsNormalized[i].Y);
                                changePosition[i] = false;
                            }

                            if (_walls.Any(wall => GeomHelper.IsCircleIntersectsRect(manWants, MAN_RADIUS, wall)))
                            {
                                changePosition[i] = false;
                                manSpeedsNormalized[i] = new Vector2d(-manSpeedsNormalized[i].X, -manSpeedsNormalized[i].Y);
                            }
                        }




                        if (changePosition[i])
                        {
                            man.position = manWants;
                        }
                        #endregion

                    }


                    interpolateFunctionMan[i].Add(man.position);
                    


                }

                
            }

            round.totalStage = 1;
        }



        public void DrawAll(Frame frame, double stage, double totalStage, bool humanMove, GlInput input) //todo human move??
        {
            frame.CameraViewport(Vector2d.Zero, _arena.size*1.1);
            frame.Polygon(Color.Gray, new Rect2d(0,0,10000,10000));
            frame.Polygon(Color.LightGray, _arena);
            for (int i = 0; i < 4; i++)
            {
                var color = roundNumber - players[i].man.lastFreezed < 300 / SPEED_FASTER ? Color.Black : players[i].color;
                frame.Circle(color, _manAnimators[i].Get(stage), MAN_RADIUS);
                frame.Circle(Color.Black , _manAnimators[i].Get(stage)+( _manList[i].speedVector).Normalized()*MAN_RADIUS, MAN_RADIUS/10.0);
                frame.TextCenter(EFont.black, players[i].man.hp.ToString(), _manAnimators[i].Get(stage));
            }
            for(int i = 0; i <_shellList.Count; i++)
            {
                frame.Circle(Color.DarkGreen, _shellAnimators[i].Get(stage), SHELL_RADIUS);
            }
            _walls.ForEach(wall => frame.Polygon(Color.Brown, wall));

            frame.TextTopLeft(EFont.ScoreOne, string.Join("   ", players.Select(x => x.score)),10,10);


        }
        
        public Turn TryGetHumanTurn(Player player, GlInput input)
        {
            string result="";
            if (player.team == 0)
                result = SolverA.Run(player.lastInput);
            else if (player.team == 1)
                result = SolverB.Run(player.lastInput);
            else if (player.team == 2)
                result = SolverC.Run(player.lastInput);
            else
                result = SolverD.Run(player.lastInput);

            return GetProgramTurn(player, result, ExecuteResult.Ok, "ok");
        }

    }
}
