using System;
using System.Text;
using System.IO;


namespace Othello.Server
{
  public class OthelloGameClientProgramExecuter : ExternalProgramExecuter
  {
    public class Step 
    {
      private static Step skipStep = new Step();

      private bool skip;
      private int row;
      private int col;

      public Step(bool skip, int row, int col) {
        this.skip = skip;
        this.row = row;
        this.col = col;
      }

      public Step(int row, int col) : this(false, row, col) { }
      public Step() : this(true, -1, -1) { }

      public bool Skip { get { return skip; } }
      public int Row { get { return row; } }
      public int Col { get { return col; } }

      public static Step SkipStep { get { return skipStep; } }
    }


    public override string TempSubdir {
      get {
        return "Othello";
      }
    }


    public OthelloGameClientProgramExecuter(string programExecutable,
                                            string inputFileName, string outputFileName) :
      base(programExecutable, inputFileName, outputFileName) {
    }


    /*
      Исполняет программу, подсовывая в качестве входных данных состояние в игре game для игрока player;
      воpащает код выполнения и через row, col, skip - выходные данные;
      maxTime - максимальное время выполнения в секундах, после чего процесс убивается
    */
    public ExternalProgramExecuteResult Execute(OthelloGame game, CellValue player, double maxTime,
                                                out int row, out int col, out bool skip, out string comment) {
      row = -1;
      col = -1;
      skip = true;
      comment = null;

      if (game == null)
        return ExternalProgramExecuteResult.WrongInputData;

      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("{0} {1}", game.RowCount, game.ColCount);
      sb.AppendLine();
      for (int r = 0; r < game.RowCount; r++) {
        for (int c = 0; c < game.ColCount; c++)
          sb.Append(game[r, c] == CellValue.Empty ? '.' : game[r, c] == player ? 'W' : 'B');
        sb.AppendLine();
      }

      String outputFileContent;
      ExternalProgramExecuteResult result = Execute(sb.ToString(), maxTime, out outputFileContent, out comment);
      if (result != ExternalProgramExecuteResult.Ok)
        return result;

      try {
        StringReader sr = new StringReader(outputFileContent);
        string firstLine = sr.ReadLine().Trim();
        sr.Close();
        if (firstLine.ToLower() == "NOT")
          return result;

        string[] parts = firstLine.Split(' ');
        row = int.Parse(parts[0].Trim()) - 1;
        col = int.Parse(parts[1].Trim()) - 1;
        if (row < 0 || row >= game.RowCount ||
            col < 0 || col >= game.ColCount) {
          return ExternalProgramExecuteResult.WrongInputData;
        }
        skip = false;

        return result;
      }
      catch (Exception) {
        return ExternalProgramExecuteResult.WrongOutputFormat;
      }
    }
  }
}
