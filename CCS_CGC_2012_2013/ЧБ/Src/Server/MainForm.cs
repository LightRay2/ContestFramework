using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;


namespace Turncoat.Server
{
    public partial class MainForm : Form
    {
        private class StepResult
        {
            private CellValue player;
            private int row1;
            private int col1;
            private int row2;
            private int col2;
            private string error;
            private string comment;

            public StepResult(CellValue player, int row1, int col1, int row2, int col2, string error, string comment) {
                this.player = player;
                this.row1 = row1;
                this.col1 = col1;
                this.row2 = row2;
                this.col2 = col2;
                this.error = error;
                this.comment = comment;
            }

            public StepResult(CellValue player, int row1, int col1, int row2, int col2) : this(player, row1, col1, row2, col2, null, null) { }
            public StepResult(CellValue player, string error, string comment) : this(player, -1, -1, -1, -1, error, comment) { }

            public CellValue Player { get { return player; } }
            public int Row1 { get { return row1; } }
            public int Col1 { get { return col1; } }
            public int Row2 { get { return row2; } }
            public int Col2 { get { return col2; } }
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


        private TurncoatGame game = null;
        private TurncoatGameClientProgramExecuter player1Executer = null;
        private TurncoatGameClientProgramExecuter player2Executer = null;

        private TurncoatGameClientProgramExecuter.Step humanStepTemp = null;

        CellValue nextStepPlayer = CellValue.WhiteChip;
        int nextStepCount = 1;
        private bool gameExecuting = false;
        private int allowSteps = 0;

        //для перемещения
        struct Selection
        {
            public bool clickMade;
            public int row1, col1, row2, col2;
            public Selection(bool cM, int r1, int c1, int r2, int c2) {
                row1 = r1;
                col1 = c1;
                row2 = r2;
                col2 = c2;
                clickMade = cM;
            }
            public void ToNull() {
                row1 = col1 = row2 = col2 = -1;
                clickMade = false;
            }
        }
        Selection selection = new Selection();

        private IList<StepResult> steps = null;


        public MainForm() {
            InitializeComponent();
        }


        private string GameConfigFile { get { return selectGameConfigTextBox.Text; } }
        private string Player1Program { get { return player1SelectProgramCheckBox.Checked ? player1SelectProgramTextBox.Text : null; } }
        private string Player2Program { get { return player2SelectProgramCheckBox.Checked ? player2SelectProgramTextBox.Text : null; } }

        private double ProgramMaxTime { get { return stepTimeHScrollBar.Value / 10f; } }
        private double BetweenTime { get { return betweenStepTimeHScrollBar.Value / 10f; } }

        private int DrawFieldOffset { get { return 30; } }
        private int CellSize {
            get {
                return Math.Min((fieldPanel.Width - DrawFieldOffset - 1) / game.ColCount,
                                (fieldPanel.Height - DrawFieldOffset - 1) / game.RowCount);
            }
        }

        public bool Finished {
            get {
                if (game == null || !gameExecuting)
                    return true;

                return game.Finished;
            }
        }


        private void ViewRefresh() {
            fieldPanel.Invalidate();

            startStopGameButton.Text = gameExecuting ? "Закончить игру" : "Новая игра";
            pauseGameButton.Enabled = gameExecuting;
            pauseGameButton.Text = gameExecuting && allowSteps > 1 ? "Пауза" : "Запустить";

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
            Bitmap btm = new Bitmap(Server.Properties.Resources.fon, new Size(cellSize * game.RowCount, cellSize * game.ColCount));
            //e.Graphics.DrawImage(new Bitmap(btm), offset, offset);
            if (selection.clickMade == true) {
                RectangleF rect = new RectangleF((selection.col1) * cellSize + offset, (selection.row1) * cellSize + offset, cellSize, cellSize);
                e.Graphics.FillRectangle(Brushes.Red, rect);
            }
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
            if (!selection.clickMade && game[row, col] == nextStepPlayer) {
                selection.row1 = row;
                selection.col1 = col;
                selection.clickMade = true;
                ViewRefresh();
            } else {
                if (game[row, col] == CellValue.Empty) {
                    selection.row2 = row;
                    selection.col2 = col;
                    if (game.IsStepAllow(nextStepPlayer, selection.row1, selection.col1, selection.row2, selection.col2))
                        humanStepTemp = new TurncoatGameClientProgramExecuter.Step(selection.row1, selection.col1, selection.row2, selection.col2);
                }
                selection.ToNull();
                ViewRefresh();
            }
        }


        private TurncoatGameClientProgramExecuter.Step GetHumanStep() {
            TurncoatGameClientProgramExecuter.Step step;
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


        private void AddStep(StepResult stepResult) {
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
            game = new TurncoatGame(GameConfigFile);
            steps = new List<StepResult>();

            stepsLogListBox.DataSource = steps;

            gameExecuting = true;
            humanStepTemp = null;
            allowSteps = 0;

            player1Executer = null;
            if (player1SelectProgramCheckBox.Checked)
                player1Executer = new TurncoatGameClientProgramExecuter(player1SelectProgramTextBox.Text,
                                                                       inputFilename, outputFilename);
            player2Executer = null;
            if (player2SelectProgramCheckBox.Checked)
                player2Executer = new TurncoatGameClientProgramExecuter(player2SelectProgramTextBox.Text,
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
                    bool isStepAllow = game.IsStepAllow(nextStepPlayer);
                    if (isStepAllow) {
                        TurncoatGameClientProgramExecuter playerExecuter = nextStepPlayer == CellValue.WhiteChip ? player1Executer : player2Executer;
                        if (playerExecuter != null) {
                            int row1, col1, row2, col2;
                            bool skip;
                            string comment;
                            ExternalProgramExecuteResult execResult = playerExecuter.Execute(game, nextStepPlayer, ProgramMaxTime,
                                                                                             out row1, out col1, out row2, out col2, out skip, out comment);
                            switch (execResult) {
                                case ExternalProgramExecuteResult.Ok:
                                    if (skip) {
                                        AddStep(new StepResult(nextStepPlayer, null, "пропуск хода"));
                                        nextStepCount = 2;
                                        thisStepCount = 1;
                                    } else {
                                        isStepAllow = game.IsStepAllow(nextStepPlayer, row1, col1, row2, col2);
                                        if (isStepAllow) {
                                            game.Step(nextStepPlayer, row1, col1, row2, col2);
                                            AddStep(new StepResult(nextStepPlayer, row1, col1, row2, col2));
                                        } else {
                                            AddStep(new StepResult(nextStepPlayer, row1, col1, row2, col2, "недопустимый ход!", null));
                                            nextStepCount = 2;
                                            thisStepCount = 1;
                                        }
                                    }
                                    break;
                                default:
                                    AddStep(new StepResult(nextStepPlayer, null, executeResultToErrorString(execResult)));
                                    nextStepCount = 2;
                                    thisStepCount = 1;
                                    break;
                            }
                        } else {
                            TurncoatGameClientProgramExecuter.Step step;
                            do {
                                step = GetHumanStep();
                                isStepAllow = step != null && game.IsStepAllow(nextStepPlayer, step.Row1, step.Col1, step.Row2, step.Col2);
                            }
                            while (step != null && !isStepAllow);

                            if (step != null) {
                                game.Step(nextStepPlayer, step.Row1, step.Col1, step.Row2, step.Col2);
                                AddStep(new StepResult(nextStepPlayer, step.Row1, step.Col1, step.Row2, step.Col2));
                            }
                        }
                    } else {
                        AddStep(new StepResult(nextStepPlayer, null, "Нет возможности хода"));
                        thisStepCount = 1;
                    }

                    thisStepCount--;
                    ViewRefresh();
                }

                nextStepPlayer = nextStepPlayer == CellValue.WhiteChip ? CellValue.BlackChip : CellValue.WhiteChip;
                allowSteps--;
                //ViewRefresh();

                if (player1Executer != null && player1Executer != null)
                    Sleep(BetweenTime);
            }

            CellValue c = WhoWin();
            string s;
            switch (c) {
                case CellValue.WhiteChip: s = "White"; break;
                case CellValue.BlackChip: s = "Black"; break;
                case CellValue.Empty: s = "Friendship"; break;
                default: s = "Who?"; break;

            }
            MessageBox.Show(s + " win");
            ViewRefresh();
        }

        public CellValue WhoWin() {
            int w = 0, b = 0;
            foreach (CellValue c in game.Field()) {
                if (c == CellValue.WhiteChip) w++;
                else
                    if (c == CellValue.BlackChip) b++;
            }
            return w > b ? CellValue.WhiteChip : b > w ? CellValue.BlackChip : CellValue.Empty;
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

            bool finished = game.Finished;
            if (finished || !gameExecuting)
                return;

            allowSteps = allowSteps > 1 ? 0 : int.MaxValue;
            ViewRefresh();
        }


        private void player1OneStepButton_Click(object sender, EventArgs e) {
            if (game == null)
                return;

            bool finished = game.Finished;
            if (game == null || finished || !gameExecuting ||
                allowSteps != 0 || nextStepPlayer != CellValue.WhiteChip)
                return;

            allowSteps = 1;
            ViewRefresh();
        }


        private void player2OneStepButton_Click(object sender, EventArgs e) {
            if (game == null)
                return;

            bool finished = game.Finished;
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
            string[] strs=System.IO.File.ReadAllLines(selectGameConfigTextBox.Text);
            foreach (string s in strs) 
                textBox1.Text += s + Environment.NewLine;
            

            stepTimeHScrollBar.Value--;
            stepTimeHScrollBar.Value++;

            WindowState = FormWindowState.Maximized;
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show(@"
Черные vs Белые - управлюяющий модуль

НЕДЕЛЯ ИНФОРМАЦИОННЫХ ТЕХНОЛОГИЙ ФКН 2013, командный тур


Автор: Тестов Борис (ФКН ВГУ)      
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
            textBox1.Text = "";
            string[] strs = System.IO.File.ReadAllLines(selectGameConfigTextBox.Text);
            foreach (string s in strs)
                textBox1.Text += s + Environment.NewLine;
        }


        private void stepTimeHScrollBar_ValueChanged(object sender, EventArgs e) {
            stepTimeLabel.Text = string.Format("Время на ход ({0} сек.)", ProgramMaxTime);
        }


        private void stepsLogListBox_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index < 0)
                return;

            ListBox listBox = (ListBox)sender;

            StepResult step = (StepResult)listBox.Items[e.Index];

            Rectangle rect = e.Bounds;
            rect.Height -= 2;
            rect.Width = rect.Height;
            rect.X++;
            rect.Y++;
            e.Graphics.FillEllipse(step.Player == CellValue.WhiteChip ? Player1Brush : Player2Brush, rect);
            e.Graphics.DrawEllipse(step.Player == CellValue.WhiteChip ? Player2Pen : Player1Pen, rect);

            string message = string.Format("r: {0}, c: {1} -->> r: {2}, c: {3}", step.Row1 + 1, step.Col1 + 1, step.Row2 + 1, step.Col2 + 1);
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

        private void stepTimeLabel_Click(object sender, EventArgs e) {

        }

        private void selectGameConfigTextBox_TextChanged(object sender, EventArgs e) {

        }
    }
}
