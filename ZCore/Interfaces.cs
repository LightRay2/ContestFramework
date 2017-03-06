using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public interface IGame<TState, TTurn, TRound, TPlayer> 
        where TTurn : ITurn<TPlayer>
        where TPlayer : IPlayer
        where TRound :IRound<TTurn,TPlayer>

    {
        int roundNumber { get; set; }
        int frameNumber { get; set; }
        List<TPlayer> players { get; }
        List<TRound> rounds { get; set; }
        bool GameFinished { get; }

        void LoadSpritesAndFonts();
        void PreparationsBeforeRound();
        void DrawAll(Frame frame,  double stage, double totalStage, bool humanMove, GlInput input);
        void ProcessRoundAndSetTotalStage( TRound round);
        List<TPlayer> GetTurnOrderForNextRound();
        string GetInputFile(TPlayer player);
        TTurn TryGetHumanTurn(TPlayer player, GlInput input);
        TTurn GetProgramTurn(TPlayer player, string output, ExecuteResult executionResult, string executionResultRussianComment);
        string GetCurrentSituation();
    }

    public interface IParamsFromStartForm
    {
        string JavaPath { get; }
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
