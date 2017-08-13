using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;


namespace Othello.Server
{
  public partial class MainForm: Form
  {
    private class OthelloStepResult
    {
      private CellValue player;
      private int row;
      private int col;
      private string error;
      private string comment;

      public OthelloStepResult(CellValue player, int row, int col, string error, string comment) {
        this.player = player;
        this.row = row;
        this.col = col;
        this.error = error;
        this.comment = comment;
      }

      public OthelloStepResult(CellValue player, int row, int col) : this(player, row, col, null, null) { }
      public OthelloStepResult(CellValue player, string error, string comment) : this(player, -1, -1, error, comment) { }

      public CellValue Player { get { return player; } }
      public int Row { get { return row; } }
      public int Col { get { return col; } }
      public string Error { get { return error; } }
      public string Comment { get { return comment; } }
    }

    /*
      Интервал проверки действий пользователя в игре (в миллисекундах)
    */
    public const int ActionCheckTimeInterval = 10;

    /*
      Интервал между выполнениями ходов при автоматическом режиме игры (в секундах)
    */
    public const double AutoModeBetweenStepsTimeInterval = 1;

    public const string inputFilename = "input.txt";
    public const string outputFilename = "output.txt";
    
    public static readonly Color Player1Color = Color.White;
    public static readonly Color Player2Color = Color.Black;

    public static readonly Pen Player1Pen = new Pen(Player1Color);
    public static readonly Pen Player2Pen = new Pen(Player2Color);
    public static readonly Brush Player1Brush = new SolidBrush(Player1Color);
    public static readonly Brush Player2Brush = new SolidBrush(Player2Color);


    private OthelloGame game = null;
    private OthelloGameClientProgramExecuter player1Executer = null;
    private OthelloGameClientProgramExecuter player2Executer = null;

    private OthelloGameClientProgramExecuter.Step humanStepTemp = null;

    CellValue nextStepPlayer = CellValue.WhiteChip;
    int nextStepCount = 1;
    private bool gameExecuting = false;
    private int allowSteps = 0;

    private IList<OthelloStepResult> steps = null;


    public MainForm() {
      InitializeComponent();
    }


    private string GameConfigFile { get { return selectGameConfigTextBox.Text; } }
    private string Player1Program { get { return player1SelectProgramCheckBox.Checked ? player1SelectProgramTextBox.Text : null; } }
    private string Player2Program { get { return player2SelectProgramCheckBox.Checked ? player2SelectProgramTextBox.Text : null; } }

    private double ProgramMaxTime { get { return stepTimeHScrollBar.Value / 10f; } }

    private int DrawFieldOffset { get { return 30; } }
    private int CellSize {
      get {
        return Math.Min((fieldPanel.Width - DrawFieldOffset - 1) / game.ColCount,
                        (fieldPanel.Height - DrawFieldOffset - 1) / game.RowCount);
      }
    }

    private bool ClassicGame { get { return classicGameCheckBox.Checked; } }

    public bool Finished {
      get {
        if (game == null || !gameExecuting)
          return true;

        return ClassicGame ? game.FinishedClassic : game.Finished;
      }
    }


    private void ViewRefresh() {
      fieldPanel.Invalidate();

      startStopGameButton.Text = gameExecuting ? "Закончить игру" : "Новая игра";
      pauseGameButton.Enabled = gameExecuting;
      pauseGameButton.Text = gameExecuting && allowSteps > 1 ? "Пауза" : "Запустить";
      player1OneStepButton.Enabled = gameExecuting && allowSteps == 0 && nextStepPlayer == CellValue.WhiteChip;
      player2OneStepButton.Enabled = gameExecuting && allowSteps == 0 && nextStepPlayer == CellValue.BlackChip;

      stepsLogListBox.DataSource = null;
      stepsLogListBox.DataSource = steps;
      if (steps != null && steps.Count > 0)
        stepsLogListBox.SelectedIndex = steps.Count - 1;
    }


    private void fieldPanel_Resize(object sender, EventArgs e) {
      ViewRefresh();
    }


    private void fieldPanel_Paint(object sender, PaintEventArgs e) {
      if (game == null)
        return;

      int offset = DrawFieldOffset;
      int cellSize = CellSize;

      Font indexesFont = new Font(SystemFonts.DefaultFont, FontStyle.Bold);
      StringFormat stringFormat = new StringFormat();
      stringFormat.Alignment = StringAlignment.Center;
      stringFormat.LineAlignment = StringAlignment.Center;
      for (int i = 0; i < 2; i++) {
        Pen pen = i == 0 ? Pens.White : Pens.Black;
        Brush brush = i == 0 ? Brushes.White : Brushes.Black;
        for (int r = 0; r <= game.RowCount; r++) {
          int x1 = offset - i,
              y1 = offset + r * cellSize - i,
              x2 = x1 + game.RowCount * cellSize,
              y2 = y1;
          e.Graphics.DrawLine(pen, x1, y1, x2, y2);
          if (r < game.RowCount) {
            Rectangle rect = new Rectangle(x1 - offset, y1, offset, cellSize);
            e.Graphics.DrawString((r + 1).ToString(), indexesFont, brush, rect, stringFormat);
          }
        }
        for (int c = 0; c <= game.ColCount; c++) {
          int x1 = offset + c * cellSize - i,
              y1 = offset - i,
              x2 = x1,
              y2 = y1 + game.ColCount * cellSize;
          e.Graphics.DrawLine(pen, x1, y1, x2, y2);
          if (c < game.RowCount) {
            Rectangle rect = new Rectangle(x1, y1 - offset, cellSize, offset);
            e.Graphics.DrawString((c + 1).ToString(), indexesFont, brush, rect, stringFormat);
          }
        }
      }

      for (int r = 0; r < game.RowCount; r++)
        for (int c = 0; c < game.ColCount; c++)
          if (game[r, c] == CellValue.WhiteChip || game[r, c] == CellValue.BlackChip) {
            RectangleF rect = new RectangleF((c + 0.15f) * cellSize + offset, (r + 0.15f) * cellSize + offset, cellSize * 0.70f - 1, cellSize * 0.70f - 1);
            e.Graphics.FillEllipse(game[r, c] == CellValue.WhiteChip ? Player1Brush : Player2Brush, rect);
            e.Graphics.DrawEllipse(game[r, c] == CellValue.WhiteChip ? Player2Pen : Player1Pen, rect);
          }
    }


    private void fieldPanel_MouseDown(object sender, MouseEventArgs e) {
      if (game == null || e.Button != MouseButtons.Left || allowSteps <= 0)
        return;

      int offset = DrawFieldOffset;
      int cellSize = CellSize;

      int x = e.X - offset + 1,
          y = e.Y - offset + 1;
      if (x % cellSize < 2 || (cellSize - x % cellSize < 2) ||
          y % cellSize < 2 || (cellSize - y % cellSize < 2))
        return;

      int row = y / cellSize,
          col = x / cellSize;
      if (row >= game.RowCount || col >= game.ColCount)
        return;

      humanStepTemp = new OthelloGameClientProgramExecuter.Step(row, col);
    }


    private OthelloGameClientProgramExecuter.Step GetHumanStep() {
      OthelloGameClientProgramExecuter.Step step;
      do {
        step = humanStepTemp;
        humanStepTemp = null;
        if (step == null) {
          Thread.Sleep(ActionCheckTimeInterval);
          Application.DoEvents();
        }
      }
      while (!Finished && step == null);

      return step;
    }


    private void AddStep(OthelloStepResult stepResult) {
      steps.Add(stepResult);
    }


    private string executeResultToErrorString(ExternalProgramExecuteResult execResult) {
      switch (execResult) {
        case ExternalProgramExecuteResult.Ok:
          return null;
        case ExternalProgramExecuteResult.InternalError:
          return "внутренняя ошибка клиента!";
        case ExternalProgramExecuteResult.TimeOut:
          return "превышен лимит ожидания клиента!";
        case ExternalProgramExecuteResult.WriteInputError:
          return "ошибка записи входного файла!";
        case ExternalProgramExecuteResult.NoOutput:
          return "отсутствует выходной файл!";
        case ExternalProgramExecuteResult.ReadOutputError:
          return "ошибка чтения выходного файла!";
        case ExternalProgramExecuteResult.EmptyOutput:
          return "выходной файл пуст!";
        case ExternalProgramExecuteResult.NotStarted:
          return "не удалось запустить клиента!";
        case ExternalProgramExecuteResult.WrongInputData:
          return "не корректные входные данные!!!";
        case ExternalProgramExecuteResult.WrongOutputFormat:
          return "неправильный формат выходного файла!";
        case ExternalProgramExecuteResult.WrongOutputData:
          return "недопустимый ответ клиента!";
        default:
          return "другая ошибка!!!";
      }
    }


    private void Sleep(double interval) {
      DateTime dt = DateTime.Now;
      while (!Finished && (DateTime.Now - dt).TotalSeconds < interval) {
        Thread.Sleep(ActionCheckTimeInterval);
        Application.DoEvents();
      }
    }


    private void StartGame() {
      game = new OthelloGame(GameConfigFile);
      steps = new List<OthelloStepResult>();

      stepsLogListBox.DataSource = steps;

      gameExecuting = true;
      humanStepTemp = null;
      allowSteps = 0;

      player1Executer = null;
      if (player1SelectProgramCheckBox.Checked)
        player1Executer = new OthelloGameClientProgramExecuter(player1SelectProgramTextBox.Text,
                                                               inputFilename, outputFilename);
      player2Executer = null;
      if (player2SelectProgramCheckBox.Checked)
        player2Executer = new OthelloGameClientProgramExecuter(player2SelectProgramTextBox.Text,
                                                               inputFilename, outputFilename);

      nextStepPlayer = player1FirstStepRadioButton.Checked ? CellValue.WhiteChip : CellValue.BlackChip;
      nextStepCount = 1;

      ViewRefresh();

      while (true) {
        while (!Finished && allowSteps <= 0) {
          Thread.Sleep(ActionCheckTimeInterval);
          Application.DoEvents();
        }
        //allowSteps--;

        if (Finished)
          break;

        int thisStepCount = nextStepCount;
        nextStepCount = 1;
        while (thisStepCount > 0) {
          bool isStepAllow = ClassicGame ? game.IsStepAllowClassic(nextStepPlayer) : game.IsStepAllow(nextStepPlayer);
          if (isStepAllow) {
            OthelloGameClientProgramExecuter playerExecuter = nextStepPlayer == CellValue.WhiteChip ? player1Executer : player2Executer;
            if (playerExecuter != null) {
              int row, col;
              bool skip;
              string comment;
              ExternalProgramExecuteResult execResult = playerExecuter.Execute(game, nextStepPlayer, ProgramMaxTime,
                                                                               out row, out col, out skip, out comment);
              switch (execResult) {
                case ExternalProgramExecuteResult.Ok:
                  if (skip) {
                    AddStep(new OthelloStepResult(nextStepPlayer, null, "пропуск хода"));
                    nextStepCount = 2;
                    thisStepCount = 1;
                  }
                  else {
                    isStepAllow = ClassicGame ? game.IsStepAllowClassic(nextStepPlayer, row, col) :
                                                game.IsStepAllow(nextStepPlayer, row, col);
                    if (isStepAllow) {
                      game.Step(nextStepPlayer, row, col);
                      AddStep(new OthelloStepResult(nextStepPlayer, row, col));
                    }
                    else {
                      AddStep(new OthelloStepResult(nextStepPlayer, row, col, "недопустимый ход!", null));
                      nextStepCount = 2;
                      thisStepCount = 1;
                    }
                  }
                  break;
                default:
                  AddStep(new OthelloStepResult(nextStepPlayer, null, executeResultToErrorString(execResult)));
                  nextStepCount = 2;
                  thisStepCount = 1;
                  break;
              }
            }
            else {
              OthelloGameClientProgramExecuter.Step step;
              do {
                step = GetHumanStep();
                isStepAllow = step != null &&
                              (ClassicGame ? game.IsStepAllowClassic(nextStepPlayer, step.Row, step.Col) :
                                             game.IsStepAllow(nextStepPlayer, step.Row, step.Col));
              }
              while (step != null && !isStepAllow);

              if (step != null) {
                game.Step(nextStepPlayer, step.Row, step.Col);
                AddStep(new OthelloStepResult(nextStepPlayer, step.Row, step.Col));
              }
            }
          }
          else {
            AddStep(new OthelloStepResult(nextStepPlayer, null, "Нет возможности хода"));
            thisStepCount = 1;
          }

          thisStepCount--;
          ViewRefresh();
        }

        nextStepPlayer = nextStepPlayer == CellValue.WhiteChip ? CellValue.BlackChip : CellValue.WhiteChip;
        allowSteps--;
        //ViewRefresh();

        if (player1Executer != null && player1Executer != null)
          Sleep(AutoModeBetweenStepsTimeInterval);
      }

      ViewRefresh();
    }


    public void EndGame() {
      gameExecuting = false;
      allowSteps = 0;
      ViewRefresh();
    }


    private void startStopGameButton_Click(object sender, EventArgs e) {
      if (gameExecuting)
        EndGame();
      else
        StartGame();

      ViewRefresh();
    }


    private void pauseGameButton_Click(object sender, EventArgs e) {
      if (game == null)
        return;

      bool finished = ClassicGame ? game.FinishedClassic : game.Finished;
      if (finished || !gameExecuting)
        return;

      allowSteps = allowSteps > 1 ? 0 : int.MaxValue;
      ViewRefresh();
    }


    private void player1OneStepButton_Click(object sender, EventArgs e) {
      if (game == null)
        return;

      bool finished = ClassicGame ? game.FinishedClassic : game.Finished;
      if (game == null || finished || !gameExecuting || 
          allowSteps != 0 || nextStepPlayer != CellValue.WhiteChip)
        return;

      allowSteps = 1;
      ViewRefresh();
    }


    private void player2OneStepButton_Click(object sender, EventArgs e) {
      if (game == null)
        return;

      bool finished = ClassicGame ? game.FinishedClassic : game.Finished;
      if (game == null || finished || !gameExecuting || 
          allowSteps != 0 || nextStepPlayer != CellValue.BlackChip)
        return;

      allowSteps = 1;
      ViewRefresh();
    }    


    private void MainForm_Load(object sender, EventArgs e) {
      player1SelectProgramOpenFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, "clients");
      player2SelectProgramOpenFileDialog.InitialDirectory = player1SelectProgramOpenFileDialog.InitialDirectory;

      selectGameConfigOpenFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, "games");
      selectGameConfigOpenFileDialog.FileName = "default.cfg";
      selectGameConfigTextBox.Text = Path.Combine(selectGameConfigOpenFileDialog.InitialDirectory, selectGameConfigOpenFileDialog.FileName);

      stepTimeHScrollBar.Value--;
      stepTimeHScrollBar.Value++;
    }


    private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
      MessageBox.Show(@"
Реверси (Отелло) - управлюяющий модуль

НЕДЕЛЯ ИНФОРМАЦИОННЫХ ТЕХНОЛОГИЙ ФКН 2012, командный тур


Автор: Дмитрий Соломатин (ФКН ВГУ), solomatin.cs.vsu.ru@gmail.com        
      ".Trim(), "О программе");
    }


    private void player1ProgramCheckBox_CheckedChanged(object sender, EventArgs e) {
      player1SelectProgramButton.Enabled = player1SelectProgramCheckBox.Checked;
      player1SelectProgramTextBox.Enabled = player1SelectProgramCheckBox.Checked;
    }


    private void player2ProgramCheckBox_CheckedChanged(object sender, EventArgs e) {
      player2SelectProgramButton.Enabled = player2SelectProgramCheckBox.Checked;
      player2SelectProgramTextBox.Enabled = player2SelectProgramCheckBox.Checked;
    }


    private void player1GroupBox_Paint(object sender, PaintEventArgs e) {
      int circleSize = player1GroupBox.Height - 22;
      Rectangle rect = new Rectangle(15, 15, circleSize, circleSize);
      e.Graphics.FillEllipse(Player1Brush, rect);
      e.Graphics.DrawEllipse(Player2Pen, rect);
    }    


    private void player2GroupBox_Paint(object sender, PaintEventArgs e) {
      int circleSize = player2GroupBox.Height - 22;
      Rectangle rect = new Rectangle(15, 15, circleSize, circleSize);
      e.Graphics.FillEllipse(Player2Brush, rect);
      e.Graphics.DrawEllipse(Player1Pen, rect);
    }


    private void player1SelectProgramButton_Click(object sender, EventArgs e) {
      if (player1SelectProgramOpenFileDialog.ShowDialog() == DialogResult.OK)
        player1SelectProgramTextBox.Text = player1SelectProgramOpenFileDialog.FileName;
    }


    private void player2SelectProgramButton_Click(object sender, EventArgs e) {
      if (player2SelectProgramOpenFileDialog.ShowDialog() == DialogResult.OK)
        player2SelectProgramTextBox.Text = player2SelectProgramOpenFileDialog.FileName;
    }


    private void selectGameConfigButton_Click(object sender, EventArgs e) {
      if (selectGameConfigOpenFileDialog.ShowDialog() == DialogResult.OK)
        selectGameConfigTextBox.Text = selectGameConfigOpenFileDialog.FileName;
    }


    private void stepTimeHScrollBar_ValueChanged(object sender, EventArgs e) {
      stepTimeLabel.Text = string.Format("Время на ход ({0} сек.)", ProgramMaxTime);
    }


    private void stepsLogListBox_DrawItem(object sender, DrawItemEventArgs e) {
      if (e.Index < 0)
        return;

      ListBox listBox = (ListBox) sender;

      OthelloStepResult step = (OthelloStepResult) listBox.Items[e.Index];

      Rectangle rect = e.Bounds;
      rect.Height -= 2;
      rect.Width = rect.Height;
      rect.X++;
      rect.Y++;
      e.Graphics.FillEllipse(step.Player == CellValue.WhiteChip ? Player1Brush : Player2Brush, rect);
      e.Graphics.DrawEllipse(step.Player == CellValue.WhiteChip ? Player2Pen : Player1Pen, rect);

      string message = string.Format("r: {0}, c: {1}", step.Row + 1, step.Col + 1);
      if (step.Error != null)
        message += string.Format(", {0}", step.Error);
      if (step.Comment != null)
        message += string.Format(", {0}", step.Comment);

      rect = e.Bounds;
      rect.X += rect.Height + 5;
      Brush brush = step.Error == null ? Brushes.Black : Brushes.Red;
      e.Graphics.DrawString(message, e.Font, brush, rect.Location);
    }


    private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
      EndGame();
    }
  }
}
