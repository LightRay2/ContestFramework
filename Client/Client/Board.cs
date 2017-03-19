using Framework;
using OpenTK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace Client
{

    public class Round : IRound<Turn, Player>
    {
        public List<Turn> turns { get; set; }
        public Random random { get; set; }
        public double totalStage { get; set; }
        public string nameForTimeLine { get; set; }
    }

    public class Turn : ITurn<Player>, ITimelineCell
    {
        public string input { get; set; }
        public string output { get; set; }
        public Player player { get; set; }
        public string shortStatus { get; set; }

        public int moveCount;
        public Tuple<Point, Point> firstValidCommand;
        public List<string> commandComments = new List<string>();


        public string totalComment { get; set; }

        public string shortTotalComment { get; set; }

        public Color colorOnTimeLine { get; set; }
        public Color colorStatusOnTimeLine { get; set; }
        public string nameOnTimeLine { get; set; }
    }

    public class Player : IPlayer
    {
        public int team;
        public bool humanSelectsDestination;
        public Point humanSource, humanDestination;
        public int movingOnRound = -1;
        public Point movingOnRoundTile;
        internal Color color;

        public string programAddress { get; set; }
        public bool controlledByHuman { get; set; }
        public string name { get; set; }
    }
    public class GameParams
    {

    }
    /// <summary>
    /// Везде point.X соответствует номеру строки
    /// </summary>
    public class Board : IGame<FormState, Turn, Round, Player>
    {
        #region game state
        public int roundNumber { get; set; }
        public int frameNumber { get; set; }
        public List<Player> players { get; set; }
        public List<Round> rounds { get; set; }
        public bool GameFinished { get; set; }

        /// <summary>
        /// 0,0 - левый верхний угол и база первого игрока (i=1), второй игрок против часовой стрелке
        /// </summary>
        Player[,] field = new Player[10, 10];
        int currentCorner = 0;
        int maxDistanceToWin;
        int teamTurn = 3;//и сразу переключится на 0
        List<Point> movePath;
        Animator<double> playerAnimator;
        int lastPlayerMadeTurns = -1;
        GamePurpose _purpose;
        public Board(FormState settings, GamePurpose purpose)
        {
            _purpose = purpose;
            if (_purpose == GamePurpose.LoadSpritesAndFonts)
                return;

            var colors = new Queue<Color>();
            colors.Enqueue(Color.Green); colors.Enqueue(Color.Red); colors.Enqueue(Color.Violet); colors.Enqueue(Color.Yellow );
            players = settings.ProgramAddressesInMatch
                .Select(index => settings.ProgramAddressesAll[index])
                .Select(address => new Player
                {
                    name = address == null ? "Человек" : Path.GetFileNameWithoutExtension(address),
                    controlledByHuman = address == null,
                    programAddress = address,
                    color = colors.Dequeue()
                }).ToList();
            for (int i = 0; i < 4; i++)
            {
                players[i].team = i;
                players[i].humanSource = new Point(1, 1);
                //стартовая позиция
                RotateField(i);
                field[1, 1] = players[i];
                field[1, 2] = players[i];
                field[2, 1] = players[i];
                field[1, 3] = players[i];
                field[2, 2] = players[i];
                field[3, 1] = players[i];
            }

            maxDistanceToWin = 10 + 9 + 9 + 8 + 8 + 8;
        }

        public static void SetFrameworkSettings()
        {
            FrameworkSettings.GameNameEnglish = "Board";
            FrameworkSettings.RunGameImmediately = false;
            FrameworkSettings.AllowFastGameInBackgroundThread = true;
            FrameworkSettings.PlayersPerGameMax = 4;
            FrameworkSettings.FramesPerTurn = 50;


            FrameworkSettings.Timeline.Enabled = true; //todo doesnt work
            FrameworkSettings.Timeline.Position = TimelinePositions.right;
            FrameworkSettings.Timeline.TileLength = 30;
            FrameworkSettings.Timeline.TileWidth = 30;
            FrameworkSettings.Timeline.FontNormalTurn = EFont.timelineNormal;
            FrameworkSettings.Timeline.FontErrorTurn = EFont.timelineError;
            FrameworkSettings.Timeline.TurnScrollSpeedByMouseOrArrow = 4;
            FrameworkSettings.Timeline.TurnScrollSpeedByPageUpDown = 16;
            FrameworkSettings.PlayersPerGameMin = 4;
            FrameworkSettings.PlayersPerGameMax = 4;

            FrameworkSettings.AdditionalHelpOnGameForm ="РУЧНОЕ УПРАВЛЕНИЕ: стрелками на клавиатуре выберите шашку, которой собираетесь ходить, нажмите Enter и аналогично выберите точку, в которую будет совершен ход.";
        }

        public Point RotatePoint(Point point, int from, int to)
        {
            for (int i = 0; i < (to - from + 4) % 4; i++)
                point = RotatePoint(point);
            return point;
        }

        Point RotatePoint(Point point)
        {
            var ret = new Point(point.Y, 9 - point.X);
            return ret;
        }

        public void RotateField(int corner)
        {
            for (int i = 0; i < (corner - currentCorner + 4) % 4; i++)
                Rotate();
            currentCorner = corner;
        }

        /// <summary>
        /// Вращает доску по часовой стрелке (получается, игроки ходят против часовой)
        /// </summary>
        void Rotate()
        {
            var rotated = new Player[10, 10];
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    rotated[row, col] = field[col, 9 - row];
                }
            }
            field = rotated;

        }
        #endregion



        enum EFont { regular, teamSmall3, teamSmall4, teamSmall1, teamSmall2, teamBig1, teamBig2, teamBig4, teamBig3, 
            timelineNormal,
            timelineError
        }
        enum ESprite { board10, Design, background, green, red, yellow, violet, humanSource, humanDestination, humanPointer, greenRect, redRect, yellowRect, violetRect }
        public void LoadSpritesAndFonts()
        {
            if (FontList.All.Count > 0)
                return;
            FontList.Load(EFont.regular, "Arial", 20, Color.Black,
                FontStyle.Regular);
            FontList.Load(EFont.teamSmall1, "Times New Roman", 15, Color.FromArgb(64, 178, 119), FontStyle.Bold);
            FontList.Load(EFont.teamSmall2, "Times New Roman", 15, Color.FromArgb(188, 64, 66), FontStyle.Bold);
            FontList.Load(EFont.teamSmall3, "Times New Roman", 15, Color.FromArgb(114, 91, 200), FontStyle.Bold);
            FontList.Load(EFont.teamSmall4, "Times New Roman", 15, Color.FromArgb(150, 147, 61), FontStyle.Bold);
            FontList.Load(EFont.teamBig1, "Times New Roman", 27, Color.FromArgb(64, 178, 119), FontStyle.Bold);
            FontList.Load(EFont.teamBig2, "Times New Roman", 27, Color.FromArgb(188, 64, 66), FontStyle.Bold);
            FontList.Load(EFont.teamBig3, "Times New Roman", 27, Color.FromArgb(114, 91, 200), FontStyle.Bold);
            FontList.Load(EFont.teamBig4, "Times New Roman", 27, Color.FromArgb(150, 147, 61), FontStyle.Bold);

            FontList.Load(EFont.timelineNormal, "Times New Roman", 10, Color.Black, FontStyle.Regular);
            FontList.Load(EFont.timelineError, "Times New Roman", 10, Color.Red, FontStyle.Bold);

            SpriteList.Load(ESprite.board10, defaultSizeExact: new Vector2d(431, 452));
            //   SpriteList.Load(ESprite.background);
            SpriteList.Load(ESprite.Design);

            SpriteList.Load(ESprite.red, defaultSizeExact: new Vector2d(40, 40));
            SpriteList.Load(ESprite.green, defaultSizeExact: new Vector2d(40, 40));
            SpriteList.Load(ESprite.violet, defaultSizeExact: new Vector2d(40, 40));
            SpriteList.Load(ESprite.yellow, defaultSizeExact: new Vector2d(40, 40));

            SpriteList.Load(ESprite.redRect);
            SpriteList.Load(ESprite.greenRect);
            SpriteList.Load(ESprite.violetRect);
            SpriteList.Load(ESprite.yellowRect);

            SpriteList.Load(ESprite.humanDestination, defaultSizeExact: new Vector2d(43, 43));
            SpriteList.Load(ESprite.humanSource, defaultSizeExact: new Vector2d(43, 43));
            SpriteList.Load(ESprite.humanPointer, defaultSizeExact: new Vector2d(43, 43));
        }

        Vector2d PointToVector(Point point) { return new Vector2d(point.X, point.Y); }

        public void DrawAll(Frame frame, double stage, double totalStage, bool humanMove, GlInput input)
        {
            //todo implement as out parameter
            //statusBarText = "";

            //you can preload sprites or not

            double frameWidth = 800;
            double frameHeight = 600;

            //total 509 509
            //49 49
            //8 10 (x y)
            frame.CameraViewport(frameWidth, frameHeight);
            #region board game draw
            //frame.TextCustom(EFont.small, "маленький текст o k очень много текста длинный тест", new Vector2d(0.0, 0), new Vector2d(200, 200), Align.justify, 200);
            //frame.TextCustom(EFont.small, "маленький текст o k очень много текста длинный тест", new Vector2d(0.5, 0.5), new Vector2d(200, 200), Align.justify, 200, 3);
            //frame.TextCustom(EFont.small, "маленький текст o k очень много текста длинный тест", new Vector2d(1.0, 0.2), new Vector2d(200, 500), Align.justify, 200);
            //  frame.SpriteCustom(ESprite.board10, 0, 0, 0, 0, 0, SpecialDraw.All(1, 1, 0, 2, 0.5));
            //frame.SpriteCustom(ESprite.Design, 0.5, 0.5, 400, 400, 30, SpecialDraw.All(0.6, 0.6, 1, -2, 1));




            var boardTopLeft = new Vector2d(66, 73);
            var corner = new Vector2d(73, 82);
            var oppositeCorner = new Vector2d(488, 517);
            var tileSize = (oppositeCorner - corner) / 10;

            double teamNamesHeight = 45;

            //доска и шашки
            frame.SpriteCorner(ESprite.background, 0, 0);



            frame.SpriteCorner(ESprite.board10, boardTopLeft);


            var firstTileCenter = corner + tileSize / 2;
            var spritePlayers = new List<ESprite>{
                ESprite.green,
                ESprite.red,
                ESprite.violet,
                ESprite.yellow
            };
            var spritePlayerRects = new List<ESprite>{
                ESprite.greenRect,
                ESprite.redRect,
                ESprite.violetRect,

                ESprite.yellowRect
            };

            //подсвечиваем противоположный от ходящего уголок
            var center = this.teamTurn == 0 ? new Vector2d(8) :
                this.teamTurn == 1 ? new Vector2d(2, 8) :
                this.teamTurn == 2 ? new Vector2d(2) :
                new Vector2d(8, 2);

            frame.SpriteCenter(spritePlayerRects[this.teamTurn], corner + tileSize.MultEach(center), sizeExact: tileSize * 4, opacity: 0.7);



            this.RotateField(0);
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    var player = this.field[row, col];
                    if (player == null)
                        continue;
                    var movingOnRoundTileInPlayer0 = this.RotatePoint(player.movingOnRoundTile, 0, this.teamTurn);
                    if (player.movingOnRound == this.roundNumber &&
                        movingOnRoundTileInPlayer0.X == row && movingOnRoundTileInPlayer0.Y == col)
                    {
                        //значит на этом ходу шашка перемещается, ее рисуем между клетками и выше всех
                        double curPartOfPath = this.playerAnimator.Get(stage);

                        int index = (int)curPartOfPath.ToRange(0, this.movePath.Count - 2);

                        double curPartOfSingleMove = curPartOfPath - index;//- index * (totalStage / (this.movePath.Count - 1));
                        var fromPoint = new Point(this.movePath[index].Y, this.movePath[index].X);
                        var toPoint = new Point(this.movePath[index + 1].Y, this.movePath[index + 1].X);
                        var from = firstTileCenter + tileSize.MultEach(PointToVector(this.RotatePoint(fromPoint, this.teamTurn, 0)));
                        var to = firstTileCenter + tileSize.MultEach(PointToVector(this.RotatePoint(toPoint, this.teamTurn, 0)));
                        var position = from + (to - from) * curPartOfSingleMove;
                        frame.SpriteCenter(spritePlayers[player.team],
                            position, depth: 1);//рисуем выше всех
                    }
                    else
                    {
                        frame.SpriteCenter(spritePlayers[player.team],
                            firstTileCenter + tileSize.MultEach(new Vector2d(col, row)));

                    }
                }
            }


            #region названия команд и счет, номер хода
            double fromBoardToNearest = 12;
            double fromBoardToFarthest = 62;
            double sideProtrusion = 40;

            var boardSize = SpriteList.All[ESprite.board10].DefaultDrawSettings.sizeExact.Value;

            Vector2d[] textPosition = new[]{
                boardTopLeft - new Vector2d(sideProtrusion, fromBoardToNearest),
                boardTopLeft - new Vector2d(sideProtrusion, fromBoardToNearest),
                boardTopLeft + new Vector2d(0,boardSize.X) + new Vector2d(sideProtrusion, fromBoardToNearest),
                boardTopLeft + new Vector2d(0,boardSize.X) + new Vector2d(sideProtrusion, fromBoardToNearest)
            };

            //top names
            frame.TextBottomLeft(EFont.teamSmall3, this.players[2].name,
                boardTopLeft - new Vector2d(sideProtrusion, fromBoardToNearest),
                  boardSize.X + sideProtrusion * 2, Align.left);
            frame.TextBottomLeft(EFont.teamSmall4, this.players[3].name,
                boardTopLeft - new Vector2d(sideProtrusion, fromBoardToNearest),
                  boardSize.X + sideProtrusion * 2, Align.right);
            //bottom names
            frame.TextBottomLeft(EFont.teamSmall2, this.players[1].name,
                boardTopLeft + new Vector2d(0, boardSize.Y) + new Vector2d(-sideProtrusion, fromBoardToFarthest),
                  boardSize.X + sideProtrusion * 2, Align.left);
            frame.TextBottomLeft(EFont.teamSmall1, this.players[0].name,
                boardTopLeft + new Vector2d(0, boardSize.Y) + new Vector2d(-sideProtrusion, fromBoardToFarthest),
                  boardSize.X + sideProtrusion * 2, Align.right);

            var scores = this.players.Select(x => { this.RotateField(x.team); return GetDistanceToWin(this.field, x); }).ToList();
            //top scores
            frame.TextTopLeft(EFont.teamBig3, scores[2].ToString(),
                boardTopLeft - new Vector2d(sideProtrusion, fromBoardToFarthest),
                  boardSize.X + sideProtrusion * 2, Align.left);
            frame.TextTopLeft(EFont.teamBig4, scores[3].ToString(),
                boardTopLeft - new Vector2d(sideProtrusion, fromBoardToFarthest),
                  boardSize.X + sideProtrusion * 2, Align.right);
            //bottom scores
            frame.TextTopLeft(EFont.teamBig2, scores[1].ToString(),
                boardTopLeft + new Vector2d(0, boardSize.Y) + new Vector2d(-sideProtrusion, fromBoardToNearest),
                  boardSize.X + sideProtrusion * 2, Align.left);
            frame.TextTopLeft(EFont.teamBig1, scores[0].ToString(),
                boardTopLeft + new Vector2d(0, boardSize.Y) + new Vector2d(-sideProtrusion, fromBoardToNearest),
                  boardSize.X + sideProtrusion * 2, Align.right);

            //turn number
            frame.TextCenter(EFont.regular, string.Format("- {0} -", this.lastPlayerMadeTurns),
                boardTopLeft.X + boardSize.X / 2, boardTopLeft.Y / 2);
            #endregion

            //выбор ходов и логирование







            //управление с клавиатуры
            if (humanMove)
            {
                double opacity = 0.7;
                var tileSource = this.RotatePoint(this.players[this.teamTurn].humanSource, 0, this.teamTurn);
                var tileDestination = this.RotatePoint(this.players[this.teamTurn].humanDestination, 0, this.teamTurn);
                if (this.players[this.teamTurn].humanSelectsDestination == true)
                {
                    frame.SpriteCenter(ESprite.humanSource, firstTileCenter + tileSize.MultEach(new Vector2d(tileSource.Y, tileSource.X)), opacity: opacity);
                    frame.SpriteCenter(ESprite.humanPointer, firstTileCenter + tileSize.MultEach(new Vector2d(tileDestination.Y, tileDestination.X)), opacity: opacity);
                }
                else
                {
                    frame.SpriteCenter(ESprite.humanPointer, firstTileCenter + tileSize.MultEach(new Vector2d(tileSource.Y, tileSource.X)), opacity: opacity);

                }


                this.RotateField(0);
                if (this.field[tileSource.X, tileSource.Y] != null && this.field[tileSource.X, tileSource.Y].team == this.teamTurn)
                {
                    var allPossible = GetAllPossibleDestinations(this.field, tileSource);
                    allPossible.ForEach(p => frame.SpriteCenter(ESprite.humanDestination, firstTileCenter + tileSize.MultEach(new Vector2d(p.Y, p.X)), opacity: opacity));
                }
            }

            #endregion


            #region timeline
            //{
            //    if (!initialized)
            //        InitTimeLine();

            //    double thinWidth = 2, thickWidth = 4;
            //    var colorLight = Color.FromArgb(80, 48, 66, 76);
            //    var colorDark = Color.FromArgb(120, 41, 58, 68);
            //    var colorLine = Color.Black;
            //    var colorCurrent = Color.Red;
            //    var colorMouse = Color.Orange;

            //    //текущий размер блока и привязка верха(привязка не может быть меньше 0)


            //    //отрисуем , что есть +текущую позицию + указатель мыши + раскрасим под мышью + найдем точную временную позицию мыши 

            //    #region old commented
            //    //double prevStart = 0;
            //    //var roundList = this.rounds
            //    //    .Select(r => 
            //    //    {
            //    //        var duration = Math.Max(0.1, r.totalStage);//чтобы можно было ход мышкой выбрать
            //    //        var res = new { name = r.nameForTimeLine, duration = duration, start = prevStart + duration };
            //    //        prevStart = res.start;
            //    //        return res;
            //    //    }).ToList();
            //    //double currentPos = roundList[ this.roundNumber].start + stage;
            //    #endregion

            //    #region прямоугольник и клетки
            //    var roundVisibleRects = new List<Tuple<int, Rect2d>>();
            //    frame.Path(colorLine, thickWidth, rect);
            //    if (currentTopTime < 0)
            //        currentTopTime = 0;

            //    double zeroY = rect.top - currentTopTime * currentHeightPerRound;
            //    for (int i = 0; i < this.rounds.Count; i++)
            //    {
            //        double top = zeroY + i * currentHeightPerRound;
            //        double bottom = top + currentHeightPerRound;
            //        var bottomInsideRect = bottom.IsInRange(rect.top, rect.bottom, true);
            //        var topIndiseRect = top.IsInRange(rect.top, rect.bottom, true);
            //        var topAboveBottomBelow = top <= rect.top && bottom >= rect.bottom;
            //        if (bottomInsideRect)
            //        {
            //            frame.Path(colorLine, thickWidth,
            //                rect.left, bottom, rect.right, bottom);

            //        }
            //        if (topIndiseRect || bottomInsideRect || topAboveBottomBelow)
            //        {
            //            var realTop = Math.Max(rect.top, top);
            //            var realBottom = Math.Min(rect.bottom, bottom);
            //            roundVisibleRects.Add(Tuple.Create(i, new Rect2d(rect.left, realTop, rect.size.X, realBottom - realTop)));
            //            if (realBottom - realTop >= minDistBetweenLines)
            //            {
            //                frame.TextCenter(EFont.regular, this.rounds[i].nameForTimeLine,
            //                    (rect.right + rect.left) / 2, (realTop + realBottom) / 2);
            //            }
            //        }
            //    }
            //    #endregion

            //    double currentPos = this.roundNumber + (stage / totalStage).ToRange(0, 1);
            //    var currentPosY = zeroY + currentPos * currentHeightPerRound;
            //    if (currentPosY.IsInRange(rect.top, rect.bottom, true))
            //    {
            //        frame.Path(colorCurrent, thickWidth, rect.left, currentPosY, rect.right, currentPosY);
            //    }

            //    Cursor.Show();
            //    foreach (var roundRect in roundVisibleRects)
            //    {
            //        if (GeomHelper.PointInSimpleRect(input.Mouse, roundRect.Item2))
            //        {
            //            Cursor.Hide();
            //            var top = zeroY + roundRect.Item1 * currentHeightPerRound;
            //            var distanceToTop = input.Mouse.Y - top;
            //            var mouseY = (distanceToTop <= mouseDropMaxDistance) ? top : input.Mouse.Y;
            //            frame.Path(colorMouse, thickWidth, rect.left, mouseY, rect.right, mouseY);


            //            if (input.LeftMouseUp)
            //            {
            //                frame.Polygon(colorDark, roundRect.Item2);
            //                //go to 
            //            }
            //            else
            //            {
            //                frame.Polygon(colorLight, roundRect.Item2);
            //            }
            //            break;
            //        }
            //    }

            //    //обработаем колесико
            //    if (GeomHelper.PointInSimpleRect(input.Mouse, rect))
            //    {
            //        if (input.Wheel != 0)
            //        {
            //            //нужно оставить то, что под мышкой, на месте, и при этом изменить height
            //            double k = 1 + input.Wheel / 10.0;
            //            double mousePosTime = (input.Mouse.Y - zeroY) / currentHeightPerRound;
            //            if (mousePosTime > this.rounds.Count && input.Wheel > 0)
            //                mousePosTime = this.rounds.Count;
            //            currentHeightPerRound *= k;
            //            double curDist = mousePosTime - currentTopTime;
            //            currentTopTime = mousePosTime - curDist / k;
            //            if (currentTopTime < 0)
            //                currentTopTime = 0;
            //        }
            //    }


            //    //сделаем переключение хода, если был клик

            //    //обработаем колесико и стрелочки вверх-вниз

            //    //если текущая позиция за кадром и мышь не наведена , перейти на нее

            //}
            #endregion
            
        }


        
        


        void InitTimeLine()
        {
            minDistBetweenLines = rect.size.Y / 8;
            ;
            mouseDropMaxDistance = minDistBetweenLines / 6;
            currentHeightPerRound = minDistBetweenLines * 1.1;
            initialized = true;
        }
        bool initialized = false;
        Rect2d rect = new Rect2d(600, 100, 200, 500);
        double minDistBetweenLines = 0;
        double mouseDropMaxDistance = 0;
        double currentHeightPerRound = 0;
        double currentTopTime = 0;

        Point PointDifference(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public Turn TryGetHumanTurn(Player player, GlInput input)//todo keyboard to input
        {
            this.RotateField(player.team);

            var keys = new Dictionary<Key, Point>{
                {Key.Left , PointDifference(this.RotatePoint( new Point(0, -1), player.team, 0) ,this.RotatePoint( new Point(0, 0), player.team, 0))},
                {Key.Right , PointDifference(this.RotatePoint( new Point(0, 1), player.team, 0),this.RotatePoint( new Point(0, 0), player.team, 0))},
                {Key.Up , PointDifference(this.RotatePoint( new Point(-1,0), player.team, 0),this.RotatePoint( new Point(0, 0), player.team, 0))},
                {Key.Down , PointDifference(this.RotatePoint( new Point(1, 0), player.team, 0),this.RotatePoint( new Point(0, 0), player.team, 0))}
            };
            if (player.humanSelectsDestination)
            {

                if (input.KeyTime(Key.Enter) == 1)
                {
                    player.humanSelectsDestination = false;
                    if (GetAllPossibleDestinations(this.field, player.humanSource).Contains(player.humanDestination))
                    {
                        return new Turn
                        {
                            firstValidCommand =
                                Tuple.Create(player.humanSource, player.humanDestination)
                            ,
                            totalComment = "",
                            shortTotalComment = "1 из 1",
                            colorOnTimeLine = player.color,
                            colorStatusOnTimeLine = Color.Red,
                            nameOnTimeLine = (this.roundNumber/4).ToString()
                        };
                    }
                }
                foreach (var item in keys)
                {
                    if (input.KeyTime(item.Key) == 1)
                    {

                        var aim = player.humanDestination;
                        aim.Offset(item.Value);
                        if (Val(aim))
                            player.humanDestination = aim;
                    }
                }
            }
            else
            {
                if (input.KeyTime(Key.Enter) == 1)
                {
                    player.humanSelectsDestination = true;
                    player.humanDestination = player.humanSource;
                }
                foreach (var item in keys)
                {
                    if (input.KeyTime(item.Key) == 1)
                    {

                        var aim = player.humanSource;
                        aim.Offset(item.Value);
                        if (Val(aim))
                            player.humanSource = aim;
                    }
                }
            }

            return null;
        }

        public Turn GetProgramTurn(Player player, string output, ExecuteResult executionResult, string executionResultRussianComment)
        {
            this.RotateField(player.team);
            var turn = new Turn { output = output };

            if (executionResult == ExecuteResult.Ok)
            {
                using (var reader = new StringReader(output))
                {
                    string s;
                    int i = 0;
                    int? selectedCommand = null;
                    while ((s = reader.ReadLine()) != null && i < 10)
                    {
                        string comment;
                        var stringNumbers = s.Split(' ');
                        int a, b, c, d;
                        try
                        {
                            a = int.Parse(stringNumbers[1]);
                            b = int.Parse(stringNumbers[0]);
                            c = int.Parse(stringNumbers[3]);
                            d = int.Parse(stringNumbers[2]);
                        }
                        catch
                        {
                            comment = "Неверный формат команды";
                            break;
                        }
                        if (Val(a) && Val(b) && Val(c) && Val(d))
                        {
                            if (this.field[a, b] != player)
                            {
                                comment = "В заданной клетке нет вашей шашки";
                            }
                            else
                            {
                                var possible = GetAllPossibleMovements(this.field, player)
                                    .Contains(Tuple.Create(new Point(a, b), new Point(c, d)));
                                if (possible)
                                {
                                    comment = "OK";
                                    if (turn.firstValidCommand == null)
                                    {
                                        turn.firstValidCommand = Tuple.Create(new Point(a, b), new Point(c, d));
                                        selectedCommand = i;
                                    }
                                }
                                else
                                {
                                    comment = "Ход не возможен";
                                }
                            }
                        }
                        else
                        {
                            comment = "Координата за пределами поля";
                        }
                        turn.commandComments.Add(comment);
                    }

                    turn.totalComment = string.Format("Принята команда {0} из {1}", i + 1, turn.commandComments.Count);
                    turn.shortTotalComment = string.Format("{0} из {1}", i + 1, turn.commandComments.Count);

                }
            }
            else
            {
                turn.totalComment = executionResultRussianComment;
                turn.shortTotalComment = "Error";
            }

            turn.colorOnTimeLine = player.color;
            turn.colorStatusOnTimeLine = Color.Red;
            turn.nameOnTimeLine = (this.roundNumber / 4).ToString();
            return turn;

            string r = "■ ►";
        }
        Round _lastProcessedRound = null;
        public void ProcessRoundAndSetTotalStage(Round round) //todo animationLength
        {
            _lastProcessedRound = round;
            round.nameForTimeLine = (this.lastPlayerMadeTurns + (this.teamTurn == 3 ? -1 : 0)).ToString();

            this.RotateField(this.teamTurn);

            if (this.teamTurn >= this.players.Count)
            {
                this.teamTurn = 0;
                if (this.players.Any(p =>
                {
                    this.RotateField(p.team);
                    return GetDistanceToWin(this.field, p) == 0;
                }
                    ))
                {
                    this.GameFinished = true;
                }
            }

            if(this.roundNumber == 199)
            {
                this.GameFinished = true; //50 turns each
            }

            var movement = round.turns.First().firstValidCommand;
            if (movement == null)
            {
                this.playerAnimator = null;
                round.totalStage = 0;
                return;
            }
            this.movePath = GetWayForTurn(this.field, movement);

            this.field[movement.Item2.X, movement.Item2.Y] = this.field[movement.Item1.X, movement.Item1.Y];
            this.field[movement.Item2.X, movement.Item2.Y].movingOnRound = this.roundNumber;
            this.field[movement.Item2.X, movement.Item2.Y].movingOnRoundTile = movement.Item2;
            this.field[movement.Item1.X, movement.Item1.Y] = null;

            bool wentToNearest = Math.Abs(movement.Item1.X - movement.Item2.X)
                + Math.Abs(movement.Item1.Y - movement.Item2.Y) == 1;
            if (wentToNearest)
            {
                this.playerAnimator = new Animator<double>(Animator.SinSwingRefined, 0, 1, 0.5);
                round.totalStage = 0.5;
                return;
            }
            else
            {
                this.playerAnimator = new Animator<double>(Animator.SinSwingRefined, 0, this.movePath.Count - 1, this.movePath.Count - 1);
                round.totalStage = this.movePath.Count - 1;
                return;
            }
        }

        public void PreparationsBeforeRound()
        {
            if (this.teamTurn == 3)
                this.lastPlayerMadeTurns++;
            this.teamTurn = (this.teamTurn + 1) % 4;
        }

        public List<Player> GetTurnOrderForNextRound()
        {

            return new List<Player>{
                this.players[this.teamTurn]
            };
        }

        public string GetInputFile(Player player)
        {
            this.RotateField(player.team);
            var sb = new StringBuilder();
            for (int row = 0; row < 10; row++)
            {
                List<int> numbers = new List<int>();
                for (int col = 0; col < 10; col++)
                {
                    if (this.field[row, col] == null)
                        numbers.Add(0);
                    else
                    {
                        int team = this.field[row, col].team;
                        if (team == player.team)
                            numbers.Add(1);
                        else
                        {
                            if (team < player.team)
                                numbers.Add(team + 2);
                            else
                                numbers.Add(team + 1);
                        }
                    }
                }
                sb.AppendLine(string.Join(" ", numbers));
            }
            return sb.ToString();
        }

        bool Val(Point a) { return Val(a.X) && Val(a.Y); }
        bool Val(int a) { return a >= 0 && a < 10; }
        int GetDistanceToWin(Player[,] field, Player player)
        {
            int r = 0;
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    if (field[row, col] == player)
                    {
                        var neareasRow = row >= 6 ? row : 6;
                        var nearestCol = col >= 6 ? col : 6;
                        r += neareasRow - row + nearestCol - col;
                    }
                }
            }
            return r;
        }



        List<Tuple<Point, Point>> GetAllPossibleMovements(Player[,] field, Player player)
        {
            var res = new List<Tuple<Point, Point>>();
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    var point = new Point(row, col);
                    if (field[row, col] == player)
                        res.AddRange(
                            GetAllPossibleDestinations(field, new Point(row, col))
                            .Select(x => Tuple.Create(point, x))
                        );
                }

            }
            return res;
        }

        /// <summary>
        /// элемент 0 - начальная точка, последний - конечная
        /// </summary>
        /// <param name="movement"></param>
        /// <returns></returns>
        List<Point> GetWayForTurn(Player[,] field, Tuple<Point, Point> movement)
        {
            Point?[,] previousPoint;
            GetAllPossibleDestinations(field, movement.Item1, out previousPoint);
            var res = new List<Point>();
            var cur = movement.Item2;
            while (true)
            {
                res.Add(cur);
                if (previousPoint[cur.X, cur.Y] != null)
                    cur = previousPoint[cur.X, cur.Y].Value;
                else
                    break;
            }
            res.Reverse();
            return res;
        }
        List<Point> GetAllPossibleDestinations(Player[,] field, Point source)
        {
            Point?[,] uselessVar;
            return GetAllPossibleDestinations(field, source, out uselessVar);
        }
        List<Point> GetAllPossibleDestinations(Player[,] field, Point source, out Point?[,] previousPoint)
        {
            List<Point> res = new List<Point>();
            var reachable = new bool[10, 10];
            previousPoint = new Point?[10, 10];
            reachable[source.X, source.Y] = true;

            var drow = new int[] { 0, -1, 0, 1 };
            var dcol = new int[] { -1, 0, 1, 0 };
            bool added = true;
            while (added)
            {
                added = false;
                for (int row = 0; row < 10; row++)
                {
                    for (int col = 0; col < 10; col++)
                    {
                        if (reachable[row, col] == false)
                            continue;
                        for (int k = 0; k < 4; k++)
                        {
                            var aimRow = row + drow[k] * 2;
                            var aimCol = col + dcol[k] * 2;
                            var throughRow = row + drow[k];
                            var throughCol = col + dcol[k];
                            if (Val(aimRow) && Val(aimCol)
                                && field[aimRow, aimCol] == null
                                && reachable[aimRow, aimCol] == false
                                && field[throughRow, throughCol] != null)
                            {
                                res.Add(new Point(aimRow, aimCol));
                                reachable[aimRow, aimCol] = true;
                                if (previousPoint[aimRow, aimCol] == null)
                                    previousPoint[aimRow, aimCol] = new Point(row, col);
                                added = true;
                            }
                        }
                    }
                }
            }

            for (int k = 0; k < 4; k++)
            {
                var aimRow = source.X + drow[k];
                var aimCol = source.Y + dcol[k];
                if (Val(new Point(aimRow, aimCol)) && field[aimRow, aimCol] == null)
                {
                    res.Add(new Point(aimRow, aimCol));
                    previousPoint[aimRow, aimCol] = new Point(source.X, source.Y);
                }
            }

            return res;
        }


        public string GetCurrentSituation()
        {
            return "Сыграно ходов: " + (this.roundNumber + 1).ToString();
        }
    }
}
