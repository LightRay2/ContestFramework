using Framework.Opengl;
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
        where TState: IState<TPlayer>, new()
        where TPlayer : IPlayer
        where TRound :IRound<TTurn,TPlayer>

    {
        void DrawAll(ref Frame frame, TState state, double stage, IGetKeyboardState keyboard);
        void ProcessRound(ref TState state, TRound round);
        List<TPlayer> GetTurnOrderForNextRound(TState state);
        string GetInputFile(TState state, TPlayer player);
        TTurn TryGetHumanTurn(TState state, TPlayer player, IGetKeyboardState keyboard);
        TTurn GetProgramTurn(TState state, TPlayer player, string output, ExecuteResult executionResult, string exitCode);
        string GetCurrentSituation(TState state);
    }

    public interface IState<TPlayer> where TPlayer : IPlayer
    {
        void Init(FormMainSettings settings);
        int roundNumber { get; set; }
        int frameNumber { get; set; }
        List<TPlayer> players { get; }
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
        Random Random { set; }

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
