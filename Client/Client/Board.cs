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

namespace Client
{
    public class State : IState<Player>
    {
        public int roundNumber { get; set; }
        public int frameNumber { get; set; }
        public List<Player> players { get; set; }
        public bool GameFinished { get; set; }

        /// <summary>
        /// 0,0 - левый верхний угол и база первого игрока (i=1), второй игрок против часовой стрелке
        /// </summary>
        public Player[,] field = new Player[10, 10];
        int currentCorner = 0;
        public int maxDistanceToWin;
        public int teamTurn = 0;
        public void Init(object settingsObject)
        {
            var settings = (FormState)settingsObject;
            players =  settings.ProgramAddressesInMatch
                .Select(index => settings.ProgramAddressesAll[index])
                .Select(address => new Player
                {
                    name = address == null ? "Человек" : Path.GetFileNameWithoutExtension(address),
                    controlledByHuman = address ==null,
                    programAddress = address
                }).ToList();
            for (int i = 0; i < 4; i++)
            {
                players[i].team = i;

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
    }

    public class Round : IRound<Turn, Player>
    {
        public List<Turn> turns { get; set; }
        public Random Random { get; set; }
    }

    public class Turn : ITurn<Player>
    {
        public string input { get; set; }
        public string output { get; set; }
        public Player player { get; set; }

        public int moveCount;
        public Tuple<Point, Point> firstValidCommand;
        public List<string> commandComments = new List<string>();


        public string totalComment { get; set; }

        public string shortTotalComment { get; set; }
    }

    public class Player : IPlayer
    {
        public int team;
        public bool humanSelectsDestination;
        public Point humanSource, humanDestination;

        public string programAddress { get; set; }
        public bool controlledByHuman { get; set; }
        public string name { get; set; }
    }
    public class GameParams
    {

    }
    public class Board : IGame<State, Turn, Round, Player>
    {
        enum EFont { small, big}
        enum ESprite { board10, Design }
        public void LoadSpritesAndFonts()
        {
            FontList.Load(EFont.small, "Arial", 12, Color.Azure, FontStyle.Strikeout, 1);
            SpriteList.LoadDefaultSize(ESprite.board10);
            SpriteList.LoadDefaultSize(ESprite.Design);
        }
        public void RunGame(object gameParams, ConcurrentDictionary<int, object> roundList = null)
        {
            GameCore<State, Turn, Round, Player>.TryRunAsSingleton(this, new List<object>(){ gameParams}, roundList);
        }

        public void DrawAll(Frame frame, State state, double stage, Framework.Opengl.IGetKeyboardState keyboard)
        {
            //total 509 509
            //49 49
            //8 10 (x y)
            frame.CameraViewport(800, 600);
            frame.TextCustom(EFont.small, "маленький текст o k очень много текста длинный тест", new Vector2d(0.0, 0), new Vector2d(200, 200), Align.justify, 200);
            frame.TextCustom(EFont.small, "маленький текст o k очень много текста длинный тест", new Vector2d(0.5, 0.5), new Vector2d(200, 200), Align.justify, 200, 3);
            frame.TextCustom(EFont.small, "маленький текст o k очень много текста длинный тест", new Vector2d(1.0, 0.2), new Vector2d(200, 500), Align.justify, 200);
          //  frame.SpriteCustom(ESprite.board10, 0, 0, 0, 0, 0, SpecialDraw.All(1, 1, 0, 2, 0.5));
            frame.SpriteCustom(ESprite.Design, 0, 0, 0, 0, 0, SpecialDraw.All(1, 1, 0, 2, 0.5));

            //for (int i = 0; i < Config2.TileCount; i++)
            //{
            //    double x = 10 + i * Config2.TileSize;
            //    double y = 40;
            //    var sprite = i == state.players[0].score && state.players[0].frameNumberHumanDoingTurn == state.frameNumber ?
            //        ESprite.gridPoint2 : ESprite.gridPoint1;
            //    FrameHelper.Rectangle(ref frame, sprite, x, y, Config2.TileSize, Config2.TileSize);
            //    y = 100;
            //    sprite = i == state.players[1].score && state.players[1].frameNumberHumanDoingTurn == state.frameNumber ?
            //        ESprite.gridPoint2 : ESprite.gridPoint1;
            //    FrameHelper.Rectangle(ref frame, sprite, x, y, Config2.TileSize, Config2.TileSize);
            //}

            //for (int i = 0, y = 40; i < 2; i++, y += 60)
            //{
            //    double x = 10 + (state.players[i].previousScore + stage * (state.players[i].score - state.players[i].previousScore)) * Config2.TileSize;
            //    frame.Add(new SpriteOld(i == 0 ? ESprite.car : ESprite.combine, Config2.TileSize * 0.8, Config2.TileSize * 0.5, new Vector2Old(x + Config2.TileSize / 2, y + Config2.TileSize / 2)));

            //}

            //if (state.GameFinished && stage.Equals(1))
            //{
            //    string text = state.players[0].score == state.players[1].score ? "Ничья!" :
            //        state.players[1].score < state.players[0].score ? "Первый игрок победил!" :
            //        "Второй игрок победил";

            //    frame.Add(new Text(EFont.fiol, new Point2(20, 20), 7, 12, text));
            //}
        }

        public void ProcessRound(State state, Round round)
        {
            state.RotateField(state.teamTurn);
            
            state.teamTurn++;
            if (state.teamTurn >= state.players.Count)
            {
                state.teamTurn = 0;
                if(state.players.Any(p=>{ 
                    state.RotateField(p.team);
                    return GetDistanceToWin(state.field, p) ==0; }
                    ))
                {
                    state.GameFinished = true;
                }              
            }
        }

        public List<Player> GetTurnOrderForNextRound(State state)
        {
            return new List<Player>{
                state.players[state.teamTurn]
            };
        }

        public string GetInputFile(State state, Player player)
        {
            state.RotateField(player.team);
            var sb = new StringBuilder();
            for (int row = 0; row < 10; row++)
            {
                List<int> numbers = new List<int>();
                for (int col = 0; col < 10; col++)
                {
                    if (state.field[row, col] == null)
                        numbers.Add(0);
                    else
                        numbers.Add(state.field[row, col].team);
                }
                sb.AppendLine(string.Join(" ", numbers));
            }
            return sb.ToString();
        }

        bool Val(Point a) { return Val(a.X) && Val(a.Y); }
        bool Val(int a) { return a >= 0 && a < 10; }
        int GetDistanceToWin(Player[,] field, Player player)
        {
            int r=0;
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
        List<Point> GetAllPossibleDestinations(Player[,] field, Point source)
        {
            List<Point> res = new List<Point>();
            var reachable = new bool[10, 10];
            reachable[source.X, source.Y] = true;

            var drow = new int[]{0,-1,0,1};
            var dcol = new int[]{-1, 0, 1, 0};
            bool added = true;
            while (added)
            {
                added = false;
                for (int row = 0; row < 10; row++)
                {
                    for (int col = 0; col < 10; col++)
                    {
                        for(int k = 0; k < 4; k++){
                            var aimRow = row + drow[k]*2;
                            var aimCol = col + dcol[k]*2;
                            var throughRow = row + drow[k];
                            var throughCol = col + dcol[k];
                            if(Val(aimRow) && Val(aimCol) 
                                && field[aimRow, aimCol] == null 
                                && reachable[aimRow, aimCol] == false
                                && field[throughRow, throughCol] != null)
                            {
                                res.Add(new Point(aimRow, aimCol));
                                reachable[aimRow, aimCol] = true;
                                added = true;
                            }
                        }
                    }
                }
            }

            return res;
        }
        public Turn TryGetHumanTurn(State state, Player player, Framework.Opengl.IGetKeyboardState keyboard)
        {
            state.RotateField(player.team);

            var keys  = new Dictionary<Keys, Point>{
                {Keys.Left , new Point(0, -1)},
                {Keys.Right , new Point(0, 1)},
                {Keys.Up , new Point(-1, 0)},
                {Keys.Down , new Point(1, 0)}
            };
            if (player.humanSelectsDestination)
            {

                if (GlInput.KeyTime(Keys.Enter) == 1)
                {
                    player.humanSelectsDestination = false;
                    if (GetAllPossibleDestinations(state.field, player.humanSource).Contains(player.humanDestination))
                    {
                        return new Turn
                        {
                            firstValidCommand =
                                Tuple.Create(player.humanSource, player.humanDestination  )
                            ,
                             totalComment = "",
                              shortTotalComment = "1 из 1"
                        };
                    }
                }
                foreach (var item in keys)
                {
                    if (GlInput.KeyTime(item.Key) == 1)
                    {

                        var aim= player.humanDestination;
                        aim.Offset(item.Value);
                        if(Val(aim))
                            player.humanDestination = aim;
                    }
                }
            }
            else
            {
                if (GlInput.KeyTime(Keys.Enter) == 1)
                {
                    player.humanSelectsDestination = true;
                }
                foreach (var item in keys)
                {
                    if (GlInput.KeyTime(item.Key) == 1)
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

        public Turn GetProgramTurn(State state, Player player, string output, ExecuteResult executionResult,  string executionResultRussianComment)
        {
            state.RotateField(player.team);
            var turn = new Turn();
            
            if (executionResult == ExecuteResult.Ok)
            {
                using (var reader = new StringReader(output))
                {
                    string s;
                    int i =0;
                    int? selectedCommand=null;
                    while ((s = reader.ReadLine()) != null && i < 10)
                    {
                        string comment;
                        var stringNumbers = s.Split(' ');
                        int a, b, c, d;
                        try
                        {
                            a = int.Parse(stringNumbers[0]);
                            b = int.Parse(stringNumbers[1]);
                            c = int.Parse(stringNumbers[2]);
                            d = int.Parse(stringNumbers[3]);
                        }
                        catch
                        {
                            comment = "Неверный формат команды";
                            break;
                        }
                        if(Val(a) && Val(b) && Val(c) && Val(d)){
                            if(state.field[a,b] != player){
                                comment = "В заданной клетке нет вашей шашки";
                            }
                            else{
                                var possible = GetAllPossibleMovements(state.field, player)
                                    .Contains(Tuple.Create(new Point(a,b), new Point(c,d)));
                                if(possible)
                                {
                                    comment = "OK";
                                    if(turn.firstValidCommand == null)
                                    {
                                        turn.firstValidCommand = Tuple.Create(new Point(a, b), new Point(c, d));
                                        selectedCommand = i;
                                    }
                                }
                                else{
                                    comment="Ход не возможен";
                                }
                            }
                        }
                        else
                        {
                            comment = "Координата за пределами поля";
                        }
                        turn.commandComments.Add(comment);
                    }

                    turn.totalComment = string.Format("Принята команда {0} из {1}", i+1, turn.commandComments.Count);
                    turn.shortTotalComment = string.Format("{0} из {1}", i+1, turn.commandComments.Count);
                }
            }
            else{
                turn.totalComment = executionResultRussianComment;
                turn.shortTotalComment = "Error";
            }
            return turn;

            string r = "■ ►";
        }

        public string GetCurrentSituation(State state)
        {
            return "Сыграно ходов: " + (state.roundNumber + 1).ToString();
        }
    }
}
