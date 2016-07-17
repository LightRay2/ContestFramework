using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    /// <summary>
    /// игра не должна хранить данных, только делать действия
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <typeparam name="TTurn"></typeparam>
    /// <typeparam name="TPlayer"></typeparam>
    public interface IGame<TState, TTurn, TRound, TPlayer> 
        where TTurn : ITurn<TPlayer>
        where TState: IState<TPlayer, TRound, TTurn>, new()
        where TPlayer : IPlayer
        where TRound :IRound<TTurn,TPlayer>

    {
        void LoadSpritesAndFonts();
        void DrawAll(Frame frame, TState state, double stage, double totalStage, bool humanMove, GlInput input);
        void ProcessRoundAndSetTotalStage(TState state, TRound round);
        List<TPlayer> GetTurnOrderForNextRound(TState state);
        string GetInputFile(TState state, TPlayer player);
        TTurn TryGetHumanTurn(TState state, TPlayer player, GlInput input);
        TTurn GetProgramTurn(TState state, TPlayer player, string output, ExecuteResult executionResult, string executionResultRussianComment);
        string GetCurrentSituation(TState state);
    }

    public interface IState<TPlayer, TRound, TTurn>
        where TPlayer : IPlayer
        where TRound : IRound<TTurn, TPlayer>
        where TTurn: ITurn<TPlayer>
    {
        void Init(object settings);
        int roundNumber { get; set; }
        int frameNumber { get; set; }
        List<TPlayer> players { get; }
        List<TRound> rounds { get; set; }
        bool GameFinished { get; }
    }

    public interface ITurn<TPlayer> where TPlayer:IPlayer
    {
        string input { set; }
        string output { set; }
        TPlayer player { set; }
    }

    /// <summary>
    /// можно что нибудь сохранить в раунд при первом подсчете, если расчеты сложные
    /// </summary>
    public interface IRound<TTurn, TPlayer> where TPlayer:IPlayer where TTurn:ITurn<TPlayer>
    {
        List<TTurn> turns { get; set; }
        Random random { set; }
        double totalStage { get; set; }
        string nameForTimeLine { get; set; }
    }

    public interface IPlayer
    {
        /// <summary>
        /// можно при желании менять адреса программ во время игры
        /// </summary>
        string programAddress { get; }
        bool controlledByHuman { get; }
        string name { get; }
    }
}
