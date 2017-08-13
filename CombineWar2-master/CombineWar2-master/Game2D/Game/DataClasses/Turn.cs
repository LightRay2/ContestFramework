using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game2D.Game;
using System.Xml.Serialization;
namespace Game2D.Game.DataClasses
{

    /// <summary>
    /// описание очередного хода, которого достаточно, чтобы перейти из одного состояние игры в другое
    /// </summary>
    [Serializable]
    public class Turn
    {
        public enum CommandType { Buy, Start, Remove, Error };
        public string memory = null;

        [Serializable]
        public class Command
        {
            public CommandType type;
            public List<int> arguments = new List<int>();
            public string comment = "";
            public Command() { }
            public Command(CommandType type, params int[] args)
            {
                this.type = type;
                arguments.AddRange(args);
            }

        }

        //public double x, y;

        public string input = "", rawOutput = "";
        public List<Command> commands = new List<Command>();
        

        public ExternalProgramExecuteResult TurnStatus;
        
    }

    [Serializable]
    public class SimultaneousTurn
    {
        public Turn FirstPlayerTurn;
        public Turn SecondPlayerTurn;
        [XmlIgnore]
        public static readonly Random Rand = new Random();

        public int _randomSeed =-1;

        public List<int> stoneRespawnLine ;
        public List<int> stoneRespawnPos ;

        public int randomSeed { get {  if (_randomSeed == -1) _randomSeed = Rand.Next(); return _randomSeed; } }
        
    }
}
