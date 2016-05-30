using Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    //public class State : IState<Player>
    //{
    //    public int roundNumber { get; set; }
    //    public int frameNumber { get; set; }
    //    public List<Player> players { get; set; }
    //    public bool GameFinished { get; set; }

    //    public void Init(FormMainSettings settings)
    //    {
    //        players = new List<Player>{
    //            new Player{
    //                 controlledByHuman = settings.FirstIsHuman,
    //                  programAddress = settings.FirstProgram,
    //                   name = "one",
    //                   teamNubmer = 0
    //            },
    //            new Player{
    //                controlledByHuman = settings.SecondIsHuman,
    //                 programAddress = settings.SecondProgram,
    //                 name = "two",
    //                 teamNubmer=1
    //            }
    //        };
    //    }
    //}

    //public class Round : IRound<Turn,Player>
    //{
    //    public List<Turn> turns { get; set; }
    //    public Random Random { get; set; }
    //}

    //public class Turn : ITurn<Player>
    //{
    //    public string input { get; set; }
    //    public string output { get; set; }
    //    public Player player { get; set; }

    //    public int moveCount;
    //}

    //public class Player : IPlayer
    //{
    //    public int score;
    //    public int teamNubmer;
    //    public int previousScore;
    //    public string programAddress { get; set; }
    //    public bool controlledByHuman { get; set; }
    //    public string name { get; set; }
    //    public long frameNumberHumanDoingTurn = -1;
    //}
    //public class GameParams{

    //}
    public class Game : IGame<State, Turn, Round, Player>
    {
        public void RunGame(object gameParams, ConcurrentDictionary<int, object> roundList = null)
        {
         //   GameCore<State, Turn, Round, Player>.TryRunAsSingleton(this, gameParams, roundList);
        }

        public void DrawAll( Frame frame, State state, double stage, Framework.Opengl.IGetKeyboardState keyboard)
        {
            //for(int i =0; i < Config2.TileCount; i++){
            //    double x = 10 + i*Config2.TileSize ;
            //    double y = 40;
            //    var sprite = i == state.players[0].score && state.players[0].frameNumberHumanDoingTurn == state.frameNumber?
            //        ESprite.gridPoint2 :ESprite.gridPoint1;
            //    FrameHelper.Rectangle(ref frame, sprite, x, y, Config2.TileSize, Config2.TileSize);
            //    y = 100;
            //    sprite = i == state.players[1].score && state.players[1].frameNumberHumanDoingTurn == state.frameNumber ?
            //        ESprite.gridPoint2 : ESprite.gridPoint1;
            //    FrameHelper.Rectangle(ref frame, sprite, x,y, Config2.TileSize, Config2.TileSize);
            //}

            //for(int i=0, y=40; i<2; i++,y+=60)
            //{
            //    double x = 10+(state.players[i].previousScore+ stage*(state.players[i].score-state.players[i].previousScore))*Config2.TileSize;
            //    frame.Add(new SpriteOld(i == 0 ? ESprite.car : ESprite.combine, Config2.TileSize * 0.8, Config2.TileSize * 0.5, new Vector2Old(x + Config2.TileSize / 2, y+Config2.TileSize/2)));

            //}

            //if (state.GameFinished && stage.Equals(1))
            //{
            //    string text = state.players[0].score == state.players[1].score? "Ничья!":
            //        state.players[1].score < state.players[0].score? "Первый игрок победил!":
            //        "Второй игрок победил";

            //    frame.Add(new Text(EFont.fiol, new Point2(20, 20), 7, 12, text));
            //}
        }

        public void ProcessRound(State state, Round round)
        {
            //for (int i = 0; i < 2; i++)
            //{
            //    state.players[i].previousScore=state.players[i].score;
            //    state.players[i].score += round.turns[i].moveCount;
            //  //  if (round.Random.Next(2) == 0)
            //   //     state.players[i].score += 2;

            //    if (state.players[i].score >= Config2.TileCount-1)
            //    {
            //        state.players[i].score = Config2.TileCount - 1;
            //        state.GameFinished = true;
            //    }
            //}
        }

        public List<Player> GetTurnOrderForNextRound(State state)
        {
            //todo один игрок случайно отдыхает ход
           // return state.players;

           
            return null; 
        }

        public string GetInputFile(State state, Player player)
        {
            return "";
        }

        public Turn TryGetHumanTurn(State state, Player player, Framework.Opengl.IGetKeyboardState keyboard)
        {
            //player.frameNumberHumanDoingTurn = state.frameNumber;

            //if (GlInput.KeyTime(System.Windows.Forms.Keys.D1) == 1)
            //    return new Turn { moveCount = 1 };
            //if (GlInput.KeyTime(System.Windows.Forms.Keys.D2) == 1)
            //    return new Turn { moveCount = 2 };

           
            return null;
        }

        public Turn GetProgramTurn(State state, Player player, string output, ExecuteResult executionResult, string exitCode)
        {
            //int moveCount;
            //Turn turn = new Turn();
            //if (executionResult == ExecuteResult.Ok && int.TryParse(output, out moveCount))
            //{
            //    turn.moveCount = moveCount;
            //}
            //return turn;
            
            
            return null;
        }

        public string GetCurrentSituation(State state)
        {
           // return "Сыграно ходов: " + (state.roundNumber+1).ToString();
            
            
            return null;
        }
    }
}
