using System;
using System.Text;
using System.IO;


namespace Turncoat.Server
{
  public class TurncoatGameClientProgramExecuter : ExternalProgramExecuter
  {
    public class Step 
    {
      private static Step skipStep = new Step();

      private bool skip;
      private int row1;
      private int col1;
      private int row2;
      private int col2;
      
      public Step(bool skip, int row1, int col1, int row2, int col2)
      {
        this.skip = skip;
        this.row1 = row1;
        this.col1 = col1;
        this.row2 = row2;
        this.col2 = col2;
      }

      public Step(int row1, int col1, int row2, int col2) : this(false, row1, col1, row2, col2) { }
      public Step() : this(true, -1, -1,-1,-1) { }

      public bool Skip { get { return skip; } }
      public int Row1 { get { return row1; } }
      public int Col1 { get { return col1; } }
      public int Row2 { get { return row2; } }
      public int Col2 { get { return col2; } }

      public static Step SkipStep { get { return skipStep; } }
    }


    public override string TempSubdir {
      get {
        return "Turncoat";
      }
    }


    public TurncoatGameClientProgramExecuter(string programExecutable,
                                            string inputFileName, string outputFileName) :
      base(programExecutable, inputFileName, outputFileName) {
    }


    /*
      Исполняет программу, подсовывая в качестве входных данных состояние в игре game для игрока player;
      воpащает код выполнения и через row1, col1, row2, col2, skip - выходные данные;
      maxTime - максимальное время выполнения в секундах, после чего процесс убивается
    */
    public ExternalProgramExecuteResult Execute(TurncoatGame game, CellValue player, double maxTime,
                                                out int row1, out int col1, out int row2, out int col2, out bool skip, out string comment)
    {
      row1 = -1;
      col1 = -1;
      row2 = -1;
      col2 = -1;
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
        string firstLine = sr.ReadLine();
        string secondLine = sr.ReadLine();
        sr.Close();
        if (firstLine.ToLower() == "NOT")
          return result;

        string[] parts1 = firstLine.Split(' ');
        row1 = int.Parse(parts1[0].Trim()) - 1;
        col1 = int.Parse(parts1[1].Trim()) - 1;
        if (row1 < 0 || row1 >= game.RowCount ||
            col1 < 0 || col1 >= game.ColCount)  {
          return ExternalProgramExecuteResult.WrongInputData;
        }
        string[] parts2 = secondLine.Split(' ');
        row2 = int.Parse(parts2[0].Trim()) - 1;
        col2 = int.Parse(parts2[1].Trim()) - 1;
        if (row2 < 0 || row2 >= game.RowCount ||
            col2 < 0 || col2 >= game.ColCount)
        {
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
