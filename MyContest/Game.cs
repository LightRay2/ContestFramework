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

namespace MyContest
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
        public Vector2d? ballAim;
        public List<Vector2d> manAims;

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

        public int team;

        [XmlIgnore]
        public List<Man> manList = new List<Man>();
        internal int score;
        public string memoryFromPreviousTurn = null;

        public int possession;

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
        Possession
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


        enum ESprite
        {
            green,
            fieldPerfect,
            man01,
            man02,
            ball,
            man03,
            man04,
            explosion
        }



        List<Man> _manList = new List<Man>();

        int _ballOwner = -1;
        int _teamWithSpeedBonus = -1;
        Random _rand;

        bool _useDefendersSpeedIncrease = false, _useDefendersLimit = false;

        Animator<Vector2d> _ballAnimator;
        List<Animator<Vector2d>> _manAnimators = new List<Animator<Vector2d>>();
        private double _ballRadius = 1;
        double _manRadius = 2;
        private int _ballIgnoreCollisionsWith;

        const double BALL_SPEED = 6;
        double MAN_SPEED = 2;


        double MAN_SPEED_IN_ATTACK = 2.5;
        double BALL_MAX_TIME = (int)(30.0 / BALL_SPEED);
        double _ballTimeLeft = 0;
        int _currentPlayerIndex;

        int _lastBallOwnerTeam = -1;

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

            for (int i = 0; i < 2; i++)
            {
                players[i].team = i;

            }

            #endregion

            var positions = new List<Vector2d>
            {
                 new Vector2d (20, 10),
                 new Vector2d(20, 30),
                 new Vector2d(20,50),
                 new Vector2d (40, 20),
                 new Vector2d(40,40),


                 new Vector2d (80, 10),
                 new Vector2d(80, 30),
                 new Vector2d(80,50),
                 new Vector2d (60, 20),
                 new Vector2d(60,40)
            };

            _manAnimators = new List<Animator<Vector2d>>();
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

                _manAnimators.Add(new Animator<Vector2d>(Animator.Linear, man.position, man.position, 1));
            }

            AddBallToGame();

            #endregion
        }

        private void AddBallToGame()
        {
            if (_rand.Next(2) == 1)
            {
                _ballSpeedNormalized = Vector2d.UnitY;
                _ballPosition = new Vector2d(50, 1);
            }
            else
            {
                _ballSpeedNormalized = -Vector2d.UnitY;
                _ballPosition = new Vector2d(50, 49);
            }

            _ballTimeLeft = BALL_MAX_TIME * _rand.NextDouble();
            _ballAnimator = new Animator<Vector2d>(Animator.Linear, _ballPosition, _ballPosition, 1);
            _ballOwner = -1;
            _ballIgnoreCollisionsWith = -1;

        }

        public static void SetFrameworkSettings()
        {
            FrameworkSettings.GameNameEnglish = "SoccerAI";
            FrameworkSettings.RunGameImmediately = false;
            FrameworkSettings.AllowFastGameInBackgroundThread = true;
            FrameworkSettings.FramesPerTurn = 15;
            FrameworkSettings.ExecutionTimeLimitSeconds = 0.7;

            FrameworkSettings.PlayersPerGameMin = 2;
            FrameworkSettings.PlayersPerGameMax = 2;
            FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create("..//Players//Easy.exe", true));
            FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create("..//Players//Easy.exe", true));
            // FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create( "..//Players//Normal.exe", false));
            //  FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create( "..//Players//Hard.exe", false));
            //  FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create("..//Players//VeryHard.exe", true));
            //  FrameworkSettings.DefaultProgramAddresses.Add(Tuple.Create( "..//Players//Extreme.exe", false));

            FrameworkSettings.Timeline.Enabled = true;
            FrameworkSettings.Timeline.Position = TimelinePositions.right;
            FrameworkSettings.Timeline.TileLength = 4;
            FrameworkSettings.Timeline.TileWidth = 4;
            //  FrameworkSettings.Timeline.FontNormalTurn = EFont.timelineNormal;
            //  FrameworkSettings.Timeline.FontErrorTurn = EFont.timelineError;
            FrameworkSettings.Timeline.TurnScrollSpeedByMouseOrArrow = 4;
            FrameworkSettings.Timeline.TurnScrollSpeedByPageUpDown = 100;
            FrameworkSettings.Timeline.FollowAnimationTimeMs = 600;
        }

        public void LoadSpritesAndFonts()
        {
            if (FontList.All.Count == 0 && SpriteList.All.Count == 0)
            {
                FontList.Load(EFont.timelineNormal, "Times New Roman", 1.6, Color.FromArgb(200, 200, 200));
                FontList.Load(EFont.timelineError, "Times New Roman", 1.9, Color.Red, FontStyle.Bold);
                FontList.Load(EFont.playerNumbers, "Times New Roman", 1.5, Color.White);
                FontList.Load(EFont.Time, "Times New Roman", 2.0, Color.FromArgb(200, 200, 200), FontStyle.Bold);
                FontList.Load(EFont.TeamOne, "Times New Roman", 2.0, Color.FromArgb(200, 0, 180, 230), FontStyle.Bold);
                FontList.Load(EFont.TeamTwo, "Times New Roman", 2.0, Color.FromArgb(200, 180, 140, 0), FontStyle.Bold);
                FontList.Load(EFont.ScoreOne, "Times New Roman", 4.0, Color.FromArgb(255, 0, 180, 230), FontStyle.Bold);
                FontList.Load(EFont.ScoreTwo, "Times New Roman", 4.0, Color.FromArgb(255, 180, 140, 0), FontStyle.Bold);
                //  SpriteList.Load(ESprite.green);
                FontList.Load(EFont.CoordsOnField, "Times New Roman", 1.5, Color.FromArgb(200, 200, 200), FontStyle.Bold);
                FontList.Load(EFont.Possession, "Times New Roman", 1.5, Color.FromArgb(150, 150, 150), FontStyle.Bold);
                FontList.Load(EFont.Goal, "Lucida Console", 20, Color.FromArgb(150, 220, 0, 0), FontStyle.Bold);

                SpriteList.Load(ESprite.explosion, defaultSizeExact: new Vector2d(50), defaultDepth: 10000, frameCountHorizontal: 6, frameCountVertical: 5);
            }
        }
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
        Rect2d _arena = new Rect2d(0, 0, 100, 60);
        int _turnLimit = 500;
        public string GetInputFile(Player player)
        {

            _currentPlayerIndex = player.team;
            var sb = new StringBuilder(); //todo turn number
            sb.AppendLine(string.Format("{0} {1} {2}", roundNumber, players[_currentPlayerIndex].score, players[(_currentPlayerIndex + 1) % 2].score));
            sb.AppendLine(string.Format("{0} {1} {2} {3}",
                Reflected(_ballPosition).X.Rounded(3), Reflected(_ballPosition).Y.Rounded(3),
                Reflected(_ballPosition + _ballSpeedNormalized * BALL_SPEED * _ballTimeLeft).X.Rounded(3),
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
            sb.AppendLine(player.memoryFromPreviousTurn ?? "-1");

            return sb.ToString();
        }

        public Turn GetProgramTurn(Player player, string output, ExecuteResult executionResult, string executionResultRussianComment)
        {
            _currentPlayerIndex = player.team;
            var turn = new Turn
            {
                shortStatus = executionResultRussianComment,
                output = output,
                colorOnTimeLine = player.team == 0 ? Color.FromArgb(0, 100, 230) : Color.FromArgb(204, 140, 0),
                nameOnTimeLine = roundNumber.ToString()
            }; //todo now in interface just edit turn, no return

            turn.manAims = new List<Vector2d>();
            for (int i = 0; i < 5; i++)
                turn.manAims.Add(player.manList[i].position);


            if (executionResult == ExecuteResult.Ok)
            {
                var reader = new StringReader(output);

                string[] outputLines;
                try
                {
                    outputLines = reader.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                }
                catch
                {
                    turn.fontOnTimeLine = EFont.timelineError;
                    turn.shortStatus = "Неправильный формат вывода";
                    return turn;
                }
                var playerAims = new List<Vector2d>();
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var s = outputLines[i].Split(' ');
                        playerAims.Add(Reflected(new Vector2d(double.Parse(s[0].Replace(",", ".")), double.Parse(s[1].Replace(",", ".")))));
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

                bool memoryUsed = false;
                try
                {
                    var nextString = outputLines[5];
                    if (nextString.StartsWith("memory "))
                    {
                        if (nextString.Length < 1100)
                        {
                            player.memoryFromPreviousTurn = nextString.Substring(7);
                            memoryUsed = true;
                        }
                    }
                    else
                    {

                        try
                        {
                            var s = nextString.Split(' ');
                            var ballAim = Reflected(new Vector2d(double.Parse(s[0].Replace(",", ".")), double.Parse(s[1].Replace(",", "."))));
                            CheckDoubleWithException(ballAim.X);
                            CheckDoubleWithException(ballAim.Y);
                            turn.ballAim = ballAim;
                            if (Debugger.IsAttached)
                            {
                                turn.shortStatus += turn.ballAim.ToString();
                            }
                        }
                        catch
                        {

                        }
                        nextString = outputLines[6];
                        if (nextString.StartsWith("memory "))
                        {
                            if (nextString.Length < 1100)
                            {
                                player.memoryFromPreviousTurn = nextString.Substring(7);
                                memoryUsed = true;
                            }
                        }
                    }

                    if (turn.ballAim != null)
                        turn.shortStatus += string.Format(". Команда удара: {0} {1}", turn.ballAim.Value.X.Rounded(3), turn.ballAim.Value.Y.Rounded(3));
                    if (memoryUsed)
                        turn.shortStatus += string.Format(". Использовано запоминание");
                    else
                        player.memoryFromPreviousTurn = null;

                    return turn;
                }
                catch
                {
                    player.memoryFromPreviousTurn = null;
                    return turn;
                }
            }
            else
            {
                turn.fontOnTimeLine = EFont.timelineError;
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


        void KickBall(Vector2d where)
        {

            _ballIgnoreCollisionsWith = _ballOwner;
            _ballOwner = -1;

            if ((where - _ballPosition).Length.DoubleLessOrEqual(0))
                _ballSpeedNormalized = Vector2d.UnitX * 0.00000001;
            else
                _ballSpeedNormalized = (where - _ballPosition).Normalized();
            _ballTimeLeft = Math.Min(BALL_MAX_TIME, (where - _ballPosition).Length / BALL_SPEED);
        }


        List<Vector2d> manSpeedsNormalized;
        List<double> explosionEffectCoeffBetween0and1;
        bool _drawBall = true;
        public void ProcessRoundAndSetTotalStage(Round round)
        {
            try
            {
                _drawBall = true;
                int partCount = 100;

                if (roundNumber - _lastGoalRoundNumber > 100 && roundNumber - _explosionStartedRound > 100)
                {
                    explosionPosition = _ballPosition;
                    _explosionStartedRound = roundNumber;
                }

                if (roundNumber == _explosionStartedRound + 5)
                    AddBallToGame();

                var interpolationFunctionBall = new InterpolationFunction(_ballPosition, 1.0 / partCount);
                _ballAnimator = new Animator<Vector2d>(interpolationFunctionBall.GetInterpolated, Vector2d.Zero, Vector2d.Zero, 1); //todo govnokod

                var interpolateFunctionMan = new List<InterpolationFunction>();
                _manAnimators = new List<Animator<Vector2d>>();
                _manList.ForEach(x =>
                {
                    interpolateFunctionMan.Add(new InterpolationFunction(x.position, 1.0 / partCount));
                    _manAnimators.Add(new Animator<Vector2d>(interpolateFunctionMan.Last().GetInterpolated, Vector2d.Zero, Vector2d.Zero, 1));
                });

                var manAims = round.turns.SelectMany(x => x.manAims).ToList();

                bool setSpeedNormalized = true;
                var manSpeedExplosionCoeff = Enumerable.Repeat(1.0, 10).ToList();
                var goBackSpeedBonus = Enumerable.Repeat(1.0, 10).ToList();
                if (_lastGoalRoundNumber != -1 && roundNumber - _lastGoalRoundNumber <= _PARTY_AFTER_GOAL_TIME)
                {
                    for (int i = 0; i < _manList.Count; i++)
                    {
                        if (i < 5)
                            manAims[i] = _manList[i].position - Vector2d.UnitX * 10000;
                        else
                            manAims[i] = _manList[i].position + Vector2d.UnitX * 10000;
                    }

                    if (_lastGoalTeam == 0)
                    {
                        for (int i = 0; i < 5; i++) goBackSpeedBonus[i] = 1.6;

                    }
                    else
                    {
                        for (int i = 5; i < 10; i++) goBackSpeedBonus[i] = 1.6;

                    }
                    _drawBall = false;
                }
                else if (roundNumber >= _explosionStartedRound && roundNumber - _explosionStartedRound < 5)
                {
                    if (_explosionStartedRound == roundNumber)
                    {
                        manAims = _manList.Select(man =>
                        {
                            if ((man.position - explosionPosition).Length < 0.00001)
                                return man.position; //stays
                            else
                                return man.position + (man.position - explosionPosition).Normalized() * 1000;
                        }).ToList();
                        explosionEffectCoeffBetween0and1 = _manList.Select(man => Math.Min(1.0, 200 / (man.position - explosionPosition).LengthSquared)).ToList();

                        _ballPosition = new Vector2d(50, 30);
                        _ballTimeLeft = 0;
                    }
                    else
                    {
                        setSpeedNormalized = false;
                    }

                    var coefficients = new List<double> { 10, 8, 6, 4, 2 };
                    manSpeedExplosionCoeff = explosionEffectCoeffBetween0and1.Select(x => x * coefficients[roundNumber - _explosionStartedRound]).ToList();
                    _drawBall = false;
                }



                if (setSpeedNormalized)
                {
                    manSpeedsNormalized = new List<Vector2d>();
                    for (int i = 0; i < _manList.Count; i++)
                    {


                        if ((manAims[i] - _manList[i].position).Length.DoubleLessOrEqual(0))
                            manSpeedsNormalized.Add(Vector2d.UnitX * 0.00000001);
                        else
                            manSpeedsNormalized.Add((manAims[i] - _manList[i].position).Normalized());

                    }
                }



                if (_lastGoalRoundNumber != -1 && roundNumber - _lastGoalRoundNumber <= _PARTY_AFTER_GOAL_TIME + 1)
                {
                    //празднование после гола

                    if (roundNumber - _lastGoalRoundNumber == _PARTY_AFTER_GOAL_TIME + 1)
                    {
                        //выбиваем мяч ближнему
                        var list = players[(_lastGoalTeam + 1) % 2].manList;
                        var manToGetBall = list.OrderBy(x => (x.position - _ballPosition).Length).First();
                        if (_ballOwner != -1 && Debugger.IsAttached)
                            throw new Exception(); //мало ли 
                        KickBall(manToGetBall.position);

                        _lastBallOwnerTeam = (_lastGoalTeam + 1) % 2; //даем бонус скорости защитникам
                    }
                    else if (roundNumber - _lastGoalRoundNumber == 2)
                    {
                        //смещаем мяч в центр
                        KickBall(new Vector2d(_ballPosition.X, 30)); //!! тут надо быть внимательным , если что то будем менять !! чтобы мяч успел долететь 
                    }
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

                    bool playersCanTouchBall = _lastGoalRoundNumber == -1 || roundNumber - _lastGoalRoundNumber > _PARTY_AFTER_GOAL_TIME;

                    //todo ball with wall
                    var changePosition = Enumerable.Repeat(true, 10).ToList();
                    var shiffledIndices = Enumerable.Range(0, 10).OrderBy(x => _rand.Next()).ToList();
                    var alreadyExchangedSpeed = new List<Tuple<int, int>>();
                    for (int shuffledIndex = 0; shuffledIndex < shiffledIndices.Count; shuffledIndex++)
                    {
                        int i = shiffledIndices[shuffledIndex];
                        double manSpeed;
                        if (i < 5)
                            manSpeed = _lastBallOwnerTeam == -1 || _lastBallOwnerTeam == 1 ? MAN_SPEED : MAN_SPEED_IN_ATTACK;
                        else
                            manSpeed = _lastBallOwnerTeam == -1 || _lastBallOwnerTeam == 0 ? MAN_SPEED : MAN_SPEED_IN_ATTACK;

                        //  bool _useDefendersSpeedIncrease, _useDefendersLimit;

                        var man = _manList[i];
                        if (thereWasCollisionWithMan[i] == false && (man.position - manAims[i]).Length <= MAN_SPEED * part)
                            changePosition[i] = false; //уже в пункте назначения, и никто его не двигает

                        if (changePosition[i])
                        {
                            #region перемещение игрока
                            double speedIncreaseCoeff = 1;
                            if (_useDefendersSpeedIncrease)
                            {
                                if ((i < 5 && _ballPosition.X < _arena.size.X / 2) || (i >= 5 && _ballPosition.X > _arena.size.X / 2))
                                    speedIncreaseCoeff = 1.3;
                            }
                            var manWants = man.position + manSpeedsNormalized[i] * manSpeed * part * speedIncreaseCoeff * manSpeedExplosionCoeff[i] * goBackSpeedBonus[i];

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

                                        //if(explosionEffectCoeffBetween0and1 != null && explosionEffectCoeffBetween0and1.Count > 0)
                                        //{
                                        //    var p = explosionEffectCoeffBetween0and1[i];
                                        //    explosionEffectCoeffBetween0and1[i] = explosionEffectCoeffBetween0and1[j];
                                        //    explosionEffectCoeffBetween0and1[j] = p;
                                        //}

                                        if (_ballOwner == i)
                                        {
                                            _ballOwner = j;
                                            _lastBallOwnerTeam = _ballOwner / 5;
                                        }
                                        else if (_ballOwner == j)
                                        {
                                            _ballOwner = i;
                                            _lastBallOwnerTeam = _ballOwner / 5;
                                        }


                                    }


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
                                        horizontalLimitLeft = _arena.size.X / 2;
                                    }
                                    else if (i == 8 || i == 9)
                                    {
                                        horizontalLimitRight = _arena.size.X / 2;
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
                            #endregion

                        }


                        interpolateFunctionMan[i].Add(man.position);


                        //interactions with ball
                        if (_ballOwner == -1 && _ballIgnoreCollisionsWith != i && playersCanTouchBall)
                        {
                            if ((man.position - _ballPosition).Length <= _manRadius + _ballRadius)
                            {
                                _ballOwner = i;
                                _lastBallOwnerTeam = _ballOwner / 5;
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
                                changePositionBall = false;
                                _ballIgnoreCollisionsWith = -1;
                                if (ballWants.X < _ballRadius)
                                    Goal(1);
                                else
                                    Goal(0);
                                playersCanTouchBall = false;
                                _ballTimeLeft = 0; //останавливается
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

                    if (_ballTimeLeft.DoubleLessOrEqual(0))
                        _ballIgnoreCollisionsWith = -1;
                    //stop when goal

                    //if(_ballWasTouchedFirst)


                    //players

                    //if ballowner != -1

                    if (_lastBallOwnerTeam != -1)
                        players[_lastBallOwnerTeam].possession++;
                }


                if (roundNumber == 299)
                {
                    GameFinished = true;
                    if (_gameInstancePurpose == GamePurpose.fastGameInBackgroundWithoutVisualization)
                    {
                        var pos = players[0].possession * 1.0 / players.Sum(x => x.possession);

                        var one = (int)Math.Round(pos * 10000);
                        var two = 10000 - one;

                        var result = new GameResult
                        {
                            BotName = players[1].name,
                            OurScore = players[0].score,
                            BotScore = players[1].score,
                            possession = one
                        };
                        _formState.LastGameResult = result;
                    }
                }
                round.totalStage = 1;
            }
            catch { if (Debugger.IsAttached) throw; }
        }

        int _lastGoalRoundNumber = -1;
        int _lastGoalTeam = -1;
        private int _PARTY_AFTER_GOAL_TIME = 10;

        void Goal(int team)
        {
            players[team].score++;
            _lastGoalRoundNumber = roundNumber;
            _lastGoalTeam = team;
            // if (team == 0)
            //     _ballOwner = 5+players[(team+1)%2].manList.IndexOf(players[(team + 1) % 2].manList.OrderByDescending(x => x.position.X).First());
            // else
            //     _ballOwner = players[(team + 1) % 2].manList.IndexOf(players[(team + 1) % 2].manList.OrderBy(x => x.position.X).First());
            // _ballPosition = _manList[_ballOwner].position;
        }


        Color RandomColor()
        {
            var rand = new Random();
            return Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }

        Color currentFieldColor = Color.White;
        double currentFieldOpacity = 0.5;
        List<Color> topColorList = new List<Color>
        {
            Color.FromArgb(65,120,104),
            Color.FromArgb(13,72,77),
            Color.FromArgb(81,143,211),
            Color.FromArgb(148,134,82),
            Color.FromArgb(40,127,194),
            Color.FromArgb(138,217,250),
            Color.FromArgb(161,218,244),
            Color.FromArgb(78,114,124)
        };
        int topColorIndex = 0;
        int blackOpacity = 120;

        List<Color> allColors = new List<Color>();
        int allColorIndex = -1;
        public void DrawAll(Frame frame, double stage, double totalStage, bool humanMove, GlInput input) //todo human move??
        {
            if (allColors.Count == 0)
            {
                for (int i = 0; i <= 3; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            allColors.Add(Color.FromArgb(i * 85, j * 85, k * 85));
                        }
                    }
            }
            if (Debugger.IsAttached)
            {
                if (input.KeyTime(Key.Z) == 1)
                    currentFieldColor = RandomColor();
                if (input.KeyTime(Key.X) == 1)
                {
                    currentFieldOpacity += 0.1;
                    if (currentFieldOpacity > 1)
                        currentFieldOpacity = 0.5;
                }
                if (input.KeyTime(Key.C) == 1)
                {
                    topColorIndex++;
                    if (topColorIndex >= topColorList.Count)
                        topColorIndex = 0;
                    currentFieldColor = topColorList[topColorIndex];
                }
                if (input.KeyTime(Key.V) == 1)
                {
                    blackOpacity += 10;
                    if (blackOpacity >= 250)
                        blackOpacity = 40;
                }
                if (input.KeyTime(Key.B) == 1)
                {
                    blackOpacity = 0;
                    allColorIndex++;
                    if (allColorIndex >= allColors.Count)
                        allColorIndex = 0;
                    currentFieldColor = allColors[allColorIndex];
                }
            }

            // topColorIndex = 6;
            // currentFieldColor = topColorList[topColorIndex];
            blackOpacity = 170;
            currentFieldOpacity = 0.90;

            //!!! будьте внимательны (ранний drawall перед любыми методами)
            int frameWidth = 112, frameHeight = 84;
            frame.CameraViewport(frameWidth, frameHeight);

            frame.Polygon(currentFieldColor, new Rect2d(0, 0, frameWidth, frameHeight)); //todo line around polygon

            var fieldCorner = new Vector2d((frameWidth - _arena.size.X - FrameworkSettings.Timeline.TileWidth) / 2, (frameHeight - _arena.size.Y) / 2);
            var lineWidth = 0.4;
            //frame.Path(Color.Black, lineWidth, _arena + fieldCorner);
            //frame.Path(Color.Black, lineWidth, fieldCorner + new Vector2d(_arena.size.X / 2, 0), fieldCorner + new Vector2d(_arena.size.X / 2, _arena.size.Y));


            frame.SpriteCorner(ESprite.fieldPerfect, fieldCorner, sizeExact: _arena.size, opacity: currentFieldOpacity, depth: 1);
            // if (_manAnimators.Count != 0) //т е еще не было process turn
            //  {


            var blackColor = Color.FromArgb(blackOpacity, 0, 0, 0);
            frame.Polygon(blackColor, new Rect2d(0, 0, 1000, fieldCorner.Y));
            frame.Polygon(blackColor, new Rect2d(0, fieldCorner.Y, fieldCorner.X, _arena.size.Y));
            frame.Polygon(blackColor, new Rect2d(fieldCorner.X + _arena.size.X, fieldCorner.Y, 1000, _arena.size.Y));
            frame.Polygon(blackColor, new Rect2d(0, fieldCorner.Y + _arena.size.Y, 1000, 1000));


            for (int i = 0; i < _manList.Count; i++)
            {
                var man = _manList[i];
                if (i < 5)
                {
                    frame.SpriteCenter(ESprite.man03, _manAnimators[i].Get(stage) + fieldCorner, sizeOnlyWidth: 4, depth: 2, opacity: 1.0);
                }
                else
                {
                    frame.SpriteCenter(ESprite.man04, _manAnimators[i].Get(stage) + fieldCorner, sizeOnlyWidth: 4, depth: 2);

                }
                //frame.Circle(man.Color, _manAnimators[i].Get(stage) + fieldCorner, _manRadius);

                frame.TextCenter(EFont.playerNumbers, (i % 5).ToString(), _manAnimators[i].Get(stage) + fieldCorner, depth: 3);
            }

            //   var curMan = _manAnimators[6].Get(stage);



            //frame.Circle(Color.Gray, _ballAnimator.Get(stage) + fieldCorner, _ballRadius);
            if (_drawBall)
                frame.SpriteCenter(ESprite.ball, _ballAnimator.Get(stage) + fieldCorner, sizeOnlyWidth: 2, depth: 2);

            // }

            frame.TextBottomLeft(EFont.TeamOne, players[0].name, fieldCorner.X, fieldCorner.Y - 3); //todo framework text without declaration?
            frame.TextCustomAnchor(EFont.TeamTwo, players[1].name, 1, 1, fieldCorner.X + _arena.size.X, fieldCorner.Y - 3);

            frame.TextCustomAnchor(EFont.ScoreOne, players[0].score.ToString(), 1, 1, fieldCorner.X + _arena.size.X / 2 - 5, fieldCorner.Y - 3);
            frame.TextBottomLeft(EFont.ScoreTwo, players[1].score.ToString(), fieldCorner.X + _arena.size.X / 2 + 5, fieldCorner.Y - 3); //todo framework text without declaration?


            //frame.TextCustomAnchor(EFont.Time, roundNumber.ToString(), 0.5, 1, fieldCorner.X + _arena.size.X / 2, fieldCorner.Y - 3);


            if (GeomHelper.PointInSimpleRect(input.Mouse, _arena + fieldCorner))
            {
                var coord = input.Mouse - fieldCorner;
                var str = string.Format("{0}  {1}", coord.X.Rounded(3), coord.Y.Rounded(3));
                frame.TextCustomAnchor(EFont.CoordsOnField, str, 0.5, 1, input.Mouse - Vector2d.UnitY * 1.5, depth: 101);
            }

            //  frame.SpriteCenter(ESprite.green, 100, 80, depth:1, sizeOnlyWidth:4);

            if (Debugger.IsAttached)
            {
                if (input.LeftMouseUp)
                {
                    var position = input.Mouse - fieldCorner;
                    if (GeomHelper.PointInSimpleRect(position, _arena))
                    {
                        _explosionStartedRound = roundNumber + 1;
                        explosionPosition = position;
                    }
                }
            }

            if (roundNumber - _explosionStartedRound <= 2 && roundNumber >= _explosionStartedRound)
            {
                try
                {
                    frame.SpriteCenter(ESprite.explosion, explosionPosition + fieldCorner, sizeExact: new Vector2d(30), frameNumber:
                        (int)((roundNumber - _explosionStartedRound) * 10 + stage * 10).ToRange(0, 30));

                }


                catch
                {

                }
            }

            if (_lastGoalRoundNumber != -1 && roundNumber - _lastGoalRoundNumber < _PARTY_AFTER_GOAL_TIME && roundNumber != _lastGoalRoundNumber)
            {
                frame.TextCenter(EFont.Goal, "ГОЛ !!!", fieldCorner + _arena.center, depth: 100000);
            }

            if (players.Sum(x => x.possession) > 0)
            {
                var pos = players[0].possession * 1.0 / players.Sum(x => x.possession);

                var one = (int)Math.Round(pos * 10000);
                var two = 10000 - one;

                double horsz = 0.6;
                var rect = new Rect2d(fieldCorner.X - horsz / 2, fieldCorner.Y + _arena.size.Y + 5.7, horsz, 2);

                frame.PolygonWithDepth(Color.FromArgb(150, 150, 150), 10, new Rect2d(fieldCorner.X, fieldCorner.Y + _arena.size.Y + 6.4, _arena.size.X, 0.6));
                frame.PolygonWithDepth(Color.FromArgb(150, 150, 150), 10, rect + Vector2d.UnitX * _arena.size.X * pos);

                //  if (this.GameFinished)
                {
                    frame.TextTopLeft(EFont.Possession, one.ToString(), fieldCorner.X, fieldCorner.Y + _arena.size.Y + 8, depth: 21);
                    frame.TextCustomAnchor(EFont.Possession, two.ToString(), 1, 0, fieldCorner.X + _arena.size.X, fieldCorner.Y + _arena.size.Y + 8, depth: 21);
                }
            }
        }


        int _explosionStartedRound = -1000;
        Vector2d explosionPosition;

        public Turn TryGetHumanTurn(Player player, GlInput input)
        {
            return new Turn();
        }

    }
}
