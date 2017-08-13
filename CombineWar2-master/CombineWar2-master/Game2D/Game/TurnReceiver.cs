using Game2D.Game.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Game2D.Opengl;
using System.Threading;
using System.IO;

namespace Game2D.Game
{
    //todo удалени не работает при ручном управлении
    using Units;
    using System.Diagnostics;
    using System.Windows.Forms;
    class TurnReceiver
    {
        DateTime startTime = DateTime.Now;
        string _javaPath;
        Engine _engine ;
        State _currentStateInSecondThread;
       // ConcurrentDictionary<int, Turn> recordedTurns = new ConcurrentDictionary<int, Turn>();

        ConcurrentDictionary<int, SimultaneousTurn> recordedTurns = new ConcurrentDictionary<int, SimultaneousTurn>();
        Thread _secondThread = null;
        bool takeTurnsFromRecordedTurns= false;

        Turn _savedFirstPlayerTurn;

        //можно по этому листу все выделенные клетки однозначно восстановить
        List<Turn.Command> _commandsFromKeyboard = new List<Turn.Command>();

        public TurnReceiver(State state, Engine engine, string javaPath, bool replayGame)
        {
            if (replayGame)
                return;

            this._javaPath = javaPath;
            _engine = engine;
            _currentStateInSecondThread = new State();
            _currentStateInSecondThread.players[0].programAddress = state.players[0].programAddress;
            _currentStateInSecondThread.players[1].programAddress = state.players[1].programAddress;
            //тут надо посмотреть, есть ли игроки-программы, и если оба игрока программы, запустить для них 
            //отдельный поток, который будет играть всю игру без анимации
            //во втором потоке получится что-то вроде
            //while(!state.GameFinished) ExternalProgramExecuter.Execute ...
            //сами ходы записываются в recordedTurns

            //если один или оба игрока управляются клавиатурой, то этого не делаем
            
            if (state.players.Count(player=>player.programAddress==null)==0)
            {
                takeTurnsFromRecordedTurns = true;
                _secondThread = new Thread(new ThreadStart(FastGameInSecondThread));
                _secondThread.Start();
            }
        }

        /// <summary>
        /// если false, надо подождать, хода пока нет
        /// если true и turn == null, значит возникла ошибка, надо смотреть comment
        /// </summary>
        public bool GetNextTurnAndComment(State state, IGetKeyboardState keyboard, out SimultaneousTurn turn, ref Frame frame)
        {
            turn = null; 
            

            //тут, опять же, две ситуации - либо оба игрока являются программами - тогда мы берем ход из
            //recordedTurns, либо делаем ход здесь и сейчас

            if (takeTurnsFromRecordedTurns)
            {
           //     if (recordedTurns.Count < 200)
            //        return false;
                return recordedTurns.TryGetValue(state.turn, out turn);
            }

            
            int weWantHisTurnNow = _savedFirstPlayerTurn == null ? 0 : 1;
            Turn smallTurn;
            bool received = GetTurn(state, state.players[weWantHisTurnNow], keyboard, out smallTurn, ref frame);

            if (received)
            {
                if (weWantHisTurnNow == 1)
                {
                    turn = new SimultaneousTurn { FirstPlayerTurn = _savedFirstPlayerTurn, SecondPlayerTurn = smallTurn };
                    _savedFirstPlayerTurn = null;
                    state.players.ForEach(x => x.moneySpent = 0);
                    return true;
                }
                else
                {
                    _savedFirstPlayerTurn = smallTurn;
                    return false;
                }
            }
            else
                return false;
        }

        bool GetTurn(State state, Player player, IGetKeyboardState keyboard, out Turn retTurn, ref Frame frame)
        {
            retTurn = null;
            if (player.programAddress != null)
            {
                retTurn = GetTurnFromProgramExecuter(state, player);
                return true;
            }

            ESprite color = player.Owner == OwnerType.Player1 ? ESprite.gridPoint1 : ESprite.gridPoint2;

            player.moneySpent = _commandsFromKeyboard.Where(x => x.type == Turn.CommandType.Buy).Sum(x => CreateUnit(x.arguments[0]).Cost);

            int cellLine, cellPosition;
            bool mouseInField =  GetCellUnderMouse(keyboard, out cellLine, out cellPosition);
            if (mouseInField)
            {
                Animator.RectangleAroundCell(ref frame, color, cellPosition, cellLine);
                if (keyboard.MouseClick)
                {
                    bool unitAtBase = player.Units[cellLine] != null && player.Units[cellLine].AtBase();
                    bool unitJustCreated = _commandsFromKeyboard.Exists(x => x.type == Turn.CommandType.Buy && x.arguments[1] == cellLine);
                    if (unitAtBase || unitJustCreated)
                        _commandsFromKeyboard.Add(new Turn.Command(Turn.CommandType.Start, cellLine, cellPosition));
                    else
                        state.Message = "Нет юнита на базе";
                }

                for(var unitAction = EKeyboardAction.Unit1; unitAction <= EKeyboardAction.Unit5; unitAction++){
                    if(keyboard.GetActionTime(unitAction) ==1){
                        int type = (int)unitAction - (int)EKeyboardAction.Unit1 +1;
                        var unit = CreateUnit(type);
                        int playerMoney = player.Money-player.moneySpent;
                        if (player.Units[cellLine] != null && !player.Units[cellLine].AtBase())
                        {
                            state.Message = "Горизонталь занята";
                        }
                        else if(playerMoney >= unit.Cost)
                        {
                            //предыдущее убираем, чтобы деньги зря не расходовали
                            _commandsFromKeyboard.RemoveAll(x => x.type == Turn.CommandType.Buy && x.arguments[1] == cellLine);
                           // _commandsFromKeyboard.Add(new Turn.Command(Turn.CommandType.Remove,cellLine ));
                            _commandsFromKeyboard.Add(new Turn.Command(Turn.CommandType.Buy, type,cellLine, 1));
                            _commandsFromKeyboard.Add(new Turn.Command(Turn.CommandType.Start, cellLine, cellPosition));
                        }
                        else{
                            state.Message="Недостаточно денег";
                        }
                    }
                }
            }

            

            //рисуем выделенные ранее клетки и цифры , если создан юнит
            for (int i = 0; i < Const.NumberOfLines; i++)
            {
                var lastStartCommandonThisLine = _commandsFromKeyboard.LastOrDefault(x => 
                    x.type == Turn.CommandType.Start && x.arguments[0] == i);
                if (lastStartCommandonThisLine != null)
                    Animator.RectangleAroundCell(ref frame, color , lastStartCommandonThisLine.arguments[1], i);

                var lastBuyCommandonThisLine = _commandsFromKeyboard.LastOrDefault(x =>
                   x.type == Turn.CommandType.Buy && x.arguments[1] == i);
                if (lastBuyCommandonThisLine != null)
                {
                    Animator.TextInCell(ref frame, player.Owner == OwnerType.Player1 ? EFont.green : EFont.red,
                        lastBuyCommandonThisLine.arguments[0].ToString(), lastStartCommandonThisLine.arguments[1], i);
                }

            }


            //dont forget to reflect everything in the end
            if (keyboard.GetActionTime(EKeyboardAction.Enter) == 1)
            {
                if(player.Owner == OwnerType.Player2){
                    _commandsFromKeyboard.Where(x=>x.type == Turn.CommandType.Start).ToList().ForEach(x=>x.arguments[1] = Reflect(x.arguments[1]));
                    _commandsFromKeyboard.Where(x=>x.type == Turn.CommandType.Buy).ToList().ForEach(x=>x.arguments[2] = Reflect(x.arguments[2]));
                }
                retTurn = new Turn { commands = _commandsFromKeyboard, TurnStatus = ExternalProgramExecuteResult.Ok };
                _commandsFromKeyboard = new List<Turn.Command>();
                retTurn.input = GetInput(state, player, player.Owner == OwnerType.Player1 ? state.players[1]: state.players[0]);
                return true;
            }
            return false;
        }

        Unit CreateUnit(int type)
        {
            Unit temp=null;
            switch (type)
            {
                case (int)UnitTypes.Armored:
                    temp = new Armored(OwnerType.Player1 ,0,0,0);
                    break;
                case (int)UnitTypes.Cannon:
                    temp = new Cannon(OwnerType.Player1, 0, 0, 0);
                    break;
                case (int)UnitTypes.Combine:
                    temp = new Combine(OwnerType.Player1, 0, 0, 0);
                    break;
                case (int)UnitTypes.Mine:
                    temp = new Mine(OwnerType.Player1, 0, 0, 0);
                    break;
                case (int)UnitTypes.Tank:
                    temp = new Tank(OwnerType.Player1, 0, 0, 0);
                    break;
            }
            return  temp;
        }

        public static bool GetCellUnderMouse(IGetKeyboardState keyboard, out int line, out int position)
        {
            line = position = -1;
            double x = keyboard.MousePos.x;
            double y = keyboard.MousePos.y;
            if (Utility.doubleLess(y, Const.FieldOriginY) ||
                Utility.doubleGreater(y, Const.FieldOriginY + Const.TileHeight * Const.NumberOfLines))
                return false;
            if (Utility.doubleLess(x, Const.FieldOriginX) ||
                Utility.doubleGreater(x, Const.FieldOriginX + Const.TileWidth * (Const.NumberOfColumns + 2)))
                return false;
             line = (int)Math.Truncate((y - Const.FieldOriginY) / Const.TileHeight);
             position = (int)Math.Truncate((x - Const.FieldOriginX) / Const.TileWidth);
             if (position <= 0 || position >= Const.NumberOfColumns + 1)
                 return false;
            return true;
        }

        Turn GetTurnFromProgramExecuter(State state, Player player)
        {
            ExternalProgramExecuter epe = new ExternalProgramExecuter(player.programAddress,
                                                                      "input.txt", "output.txt", _javaPath);
            bool reflected = player.Owner == OwnerType.Player1 ? false : true;
            Turn ret = new Turn();
            string input = ret.input = GetInput(state, player, reflected ? state.players[0] : state.players[1]);
            string output;
            string comment;
            try
            {
                ExternalProgramExecuteResult res = epe.Execute(input, 2, out output, out comment);
                
                
                //пустой файл допускается
                if (res == ExternalProgramExecuteResult.EmptyOutput)
                {
                    output = "";
                    res = ExternalProgramExecuteResult.Ok;
                }
                ret.TurnStatus = res;

                if (res == ExternalProgramExecuteResult.Ok)
                {
                    ret.rawOutput = output;
                    StringReader reader = new StringReader(output); 
                    string linexxx;   List<string> lines = new List<string>();
                    while (!string.IsNullOrEmpty(linexxx = reader.ReadLine()))
                    {
                        lines.Add(linexxx);
                    }

                    player.Memory = null;
                    if (lines.Count > 0 && lines.Last().Contains("memory"))
                    {
                        ret.memory = lines.Last();
                        lines.RemoveAt(lines.Count - 1);
                        player.Memory = ret.memory.Replace("memory","");
                    }

                    for (int i = 0; i < lines.Count; i++)
                    {
                        string line = lines[i];
                        var command = new Turn.Command();
                        try
                        {
                            string[] temp = line.Split(new char[]{' '},StringSplitOptions.RemoveEmptyEntries);
                            command.type = (Turn.CommandType)(Convert.ToInt32(temp[0]));
                            for (int j = 1; j < temp.Length; j++)
                                command.arguments.Add(Convert.ToInt32(temp[j]));


                            if((command.type== Turn.CommandType.Buy && command.arguments.Count < 3)
                                ||(command.type== Turn.CommandType.Start && command.arguments.Count < 2)
                                || (command.type == Turn.CommandType.Remove && command.arguments.Count < 1))
                            {
                                throw new Exception();
                            }

                            //нарушение логики проверяется в Engine

                            //не надо ничего писать в коммент, если команда хорошая
                           // command.comment = "ok";
                        }
                        catch
                        {
                            command.type = Turn.CommandType.Error;
                            command.comment = "неверный формат";
                        }
                        
                        ret.commands.Add(command);
                    }
                }
            }
            catch (ApplicationException err)
            {
                return ret;
            }

            return ret;
        }

        public static string GetOutputWithComments(Turn turn)
        {
            var reader = new StringReader(turn.rawOutput);
            var writer = new StringBuilder();
            writer.AppendLine(GetFullTurnComment(turn));
            turn.commands.ForEach(command=>{
                writer.AppendLine(String.Format("{0} [ {1} ]", reader.ReadLine(),string.IsNullOrEmpty(command.comment) ?"ok" : command.comment));
            });
            writer.AppendLine(reader.ReadToEnd());
            return writer.ToString();
        }
         
        public static string GetFullTurnComment(Turn turn)
        {
            var status = turn.TurnStatus;
            string s = status == ExternalProgramExecuteResult.Ok ? "Ok" :
                status == ExternalProgramExecuteResult.InternalError? "исключение в ходе выполнения программы участника":
                status == ExternalProgramExecuteResult.NoOutput? "отсутствует файл выходных данных":
                status == ExternalProgramExecuteResult.NotStarted? "не удалось запустить программу участника":
                status == ExternalProgramExecuteResult.OtherError? "неизвестная ошибка":
                status == ExternalProgramExecuteResult.ReadOutputError? "не удалось считать output.txt":
                status == ExternalProgramExecuteResult.TimeOut? "превышен лимит времени":
                status == ExternalProgramExecuteResult.WriteInputError? "ошибка записи входных данных для внешней программы":
                status == ExternalProgramExecuteResult.OutputTooBig? "слишком большой файл выходных данных" :
                "";

            if (status == ExternalProgramExecuteResult.Ok)
            {
                return String.Format("[ {0}, правильных команд {1} из {2} ]",
                    s,
                    turn.commands.Count(x => x.type != Turn.CommandType.Error && string.IsNullOrEmpty(x.comment)),
                    turn.commands.Count);
            }
            else return String.Format("[ {0} ]", s);
        }

        public static string GetShortTurnComment(Turn turn)
        {
            var status = turn.TurnStatus;
            string s = status == ExternalProgramExecuteResult.Ok ? "Ok" : "Error";

            return String.Format("{0} {1}/{2}",
                s,
                turn.commands.Count(x => x.type != Turn.CommandType.Error && string.IsNullOrEmpty(x.comment)),
                turn.commands.Count);
        }

        int Reflect(int x)
        {
            return Const.NumberOfColumns + 1 - x;
        }
        string GetInput (State state, Player player, Player enemyPlayer)
        {
            bool reflected = player.Owner == OwnerType.Player2 ? true : false;
            string ret = "";
            ret += state.turn.ToString() + ' ' + player.Score.ToString() + ' ' + enemyPlayer.Score.ToString() + ' '
                 + player.Money.ToString() + ' ' + enemyPlayer.Money.ToString() + Environment.NewLine;

            for (int i = 0; i < state.Stones.Length; i++)
            {
                if (state.Stones[i] <0)
                {
                    ret += "0 ";
                    continue;
                }
                if (reflected)
                    ret += (Const.NumberOfColumns + 1 - state.Stones[i]).ToString() + ' ';
                else
                    ret += (state.Stones[i]).ToString() + ' ';
            }
            ret += '\n';

            ret += GetUnits(player, reflected);
            ret += GetUnits(enemyPlayer, reflected);

            if (player.Memory != null)
                ret += player.Memory + Environment.NewLine;
            else
                ret += "-1" + Environment.NewLine;
            return ret;
        }

        string GetUnits (Player player, bool reflect)
        {
            string ret = "";
            foreach (Units.Unit unit in player.Units)
            {
                if (unit == null || unit.State == UnitState.Dead)
                {
                    ret += "0 0 0 0" + Environment.NewLine;
                    continue;
                }
                ret += ((int)unit.Type).ToString() + ' ' + ((int)unit.State).ToString() + ' ';
                if (reflect)
                    ret += (Const.NumberOfColumns + 1 - unit.PosTileX).ToString() + ' ';
                else
                    ret += (unit.PosTileX).ToString() + ' ';
                ret += unit.Hp + Environment.NewLine;
            }
            return ret;
        }

        void FastGameInSecondThread()
        {
            var state = _currentStateInSecondThread;
            int turnWithExceptionCount = 0;
            GameForm.GameInSecondThreadIsRunning = true;
            while (!state.IsFinished)
            {

                Turn firstPlayerTurn = GetTurnFromProgramExecuter(state, state.players[0]);
                if (firstPlayerTurn != null && firstPlayerTurn.TurnStatus == ExternalProgramExecuteResult.InternalError)
                    turnWithExceptionCount++;
                Turn secondPlayerTurn = GetTurnFromProgramExecuter(state, state.players[1]);

                if (secondPlayerTurn != null && secondPlayerTurn.TurnStatus == ExternalProgramExecuteResult.InternalError)
                    turnWithExceptionCount++;

                bool stopGame = GameForm.UserWantsToClose;
                if (turnWithExceptionCount >= 2)
                {
                    
                   var dialogResult =  MessageBox.Show("Внешняя программа несколько раз завершилась с ошибкой. Остановить игру? Да = остановить, Нет = продолжить до следующей ошибки, Отмена = продолжить до конца игры, не обращая внимания на ошибки", "Внимание", MessageBoxButtons.YesNoCancel);
                   if (dialogResult == DialogResult.No)
                   {
                       turnWithExceptionCount = 1;
                   }
                   else if (dialogResult == DialogResult.Yes)
                   {
                       stopGame = true;
                   }
                   else
                   {
                       turnWithExceptionCount = -1000000;
                   }
                }


                

                SimultaneousTurn turn = new SimultaneousTurn();
                turn.FirstPlayerTurn = firstPlayerTurn;
                turn.SecondPlayerTurn = secondPlayerTurn;
                //записываем ход и проигрываем его, чтоб можно было дальше программы вызывать
                
                _engine.DoTurn(ref state, turn);
                recordedTurns.TryAdd(state.turn, turn);
                state.turn++;
                if (state.turn == Const.NumberOfTurns || stopGame)
                    state.IsFinished = true;
            }
            Thread.Sleep(500); //чтобы удалить темп, надо подождать, пока программы отдадут права на редактирование директории
            GameForm.GameInSecondThreadIsRunning = false;

            //сохраним игру полностью
            string file = startTime.ToString("yy-MM-dd-hh-mm-ss")
                +"-"+ (state.players[0].programAddress == null ? "человек" : Path.GetFileNameWithoutExtension(state.players[0].programAddress))
                + "-" + (state.players[1].programAddress == null ? "человек" : Path.GetFileNameWithoutExtension(state.players[1].programAddress))
                + "-" + state.players[0].Score.ToString() + "-" + state.players[1].Score.ToString();
            bool success = TrySaveReplay(Application.StartupPath +"\\..\\Replays\\"+file+".rpl");

            ExternalProgramExecuter.DeleteTempSubdir();
        }

        public bool TrySaveReplay(string file)
        {
            var turnList = recordedTurns.Values.ToList();
            return Utility.TryWriteToXmlFile(file, turnList);
        }

        public bool TryLoadReplay(string file){
            List<SimultaneousTurn> turnList = Utility.TryReadFromXmlFile<List<SimultaneousTurn>>(file);
            if (turnList == null)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < turnList.Count; i++)
                {
                    recordedTurns.TryAdd(i, turnList[i]);
                }
                takeTurnsFromRecordedTurns = true;
                return true;
            }
        }
    }
}
