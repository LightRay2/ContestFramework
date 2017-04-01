using Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MyContest
{
    public partial class StartForm : Form
    {
        #region init everything
        public StartForm()
        {
            Game.SetFrameworkSettings();
            this.KeyPreview = true;
            InitializeComponent();
        }

        FormState formState;
        bool needRefreshControls = true;
        private void StartForm_Load(object sender, EventArgs e)
        {
            LoadFormState();

            RefreshScoreLabel();

            edtReplayFolder.DataBindings.Add("Text", formState, "ReplayFolder");
            edtSaveReplays.DataBindings.Add("Checked", formState, "SaveReplays");

            edtUseFixedRandomSeed.DataBindings.Add("Checked", formState, "UseFixedRandomSeed");
            edtFixedRandomSeed.DataBindings.Add("Value", formState, "FixedRandomSeed");


            refreshTimer.Tick += (s, args) =>
            {
                if (needRefreshControls)
                {
                    try { 
                        needRefreshControls = false;
                        RefreshControls();
                    }
                    catch { if (Debugger.IsAttached) throw; }
                }
            };
            refreshTimer.Start();


            if (FrameworkSettings.RunGameImmediately && formState.ProgramAddressesInMatch.Count > 0)
                btnRun_Click(null, null);
        }

        private void LoadFormState()
        {
            try
            {
                formState = FormState.LoadOrCreate();


                formState.PropertyChanged += (s, args) => needRefreshControls = true;
            }
            catch { if (Debugger.IsAttached) throw; }
        }
        #endregion

        public object CreateGameParamsFromFormState(FormState state)
        {
            var p = new GameParams();
            return p;
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;
        private void SuspendDrawing()
        {
            SendMessage(this.Handle, WM_SETREDRAW, false, 0);
        }

        private void ResumeDrawing()
        {

            SendMessage(this.Handle, WM_SETREDRAW, true, 0);
        }
        public void RefreshControls()
        {
            int scrollValue = panelPlayers.VerticalScroll.Value;
            SuspendDrawing();
            Color checkedColor = Color.LawnGreen;
            Color uncheckedColor = Color.LightGray;
            ToolTip toolTip = new ToolTip();
            var panelPlayers_DeleteButtons = new List<Control>();
            #region panelPlayers
            panelPlayers.Controls.Clear();
            for (int i = 0; i < formState.ProgramAddressesAll.Count; i++)
            {
                string text = formState.ProgramAddressesAll[i] ?? "Человек";
                var checkBox = new CheckBox
                {
                    Tag = i,
                    Checked = formState.ProgramAddressesInMatch.Contains(i),
                    Margin = new Padding { Left = 10, Top = 10 },
                    Size = new Size(205, 30),
                    FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                    Appearance = System.Windows.Forms.Appearance.Button,
                    Text = new string(text.Reverse().Take(25).Reverse().ToArray()),
                    BackColor = formState.ProgramAddressesInMatch.Contains(i) ? checkedColor : uncheckedColor
                };
                toolTip.SetToolTip(checkBox, text);
                var deleteButton = new Button
                {
                    Tag = i,
                    Margin = new Padding { All = 10 },
                    Size = new Size(30, 30),
                    FlatStyle = FlatStyle.Flat,
                    Text = "-",
                    BackColor = uncheckedColor
                };
                checkBox.CheckedChanged += PlayerCheckedChanged;
                deleteButton.Click += deleteButton_Click;
                panelPlayers.Controls.Add(checkBox);
                panelPlayers.Controls.Add(deleteButton);
                panelPlayers_DeleteButtons.Add(deleteButton);
            }
            #endregion

            #region panel players in match
            panelPlayersInMatch.Controls.Clear();
            for (int i = 0; i < formState.ProgramAddressesInMatch.Count; i++)
            {
                string text = formState.ProgramAddressesAll[formState.ProgramAddressesInMatch[i]] ?? "Человек";
                var label = new Label
                {
                    Tag = i,
                    Text = new string(text.Reverse().Take(70).Reverse().ToArray()),
                    Padding = new Padding { All = 5 },
                    Margin = new Padding { All = 3 },
                    Size = new Size(560, 32),
                    BorderStyle = BorderStyle.FixedSingle
                };
                toolTip.SetToolTip(label, text);
                panelPlayersInMatch.Controls.Add(label);
            }
            #endregion


            btnChangeJavaPath.Visible = string.IsNullOrEmpty(formState.JavaPath) == false;
            btnChangePythonPath.Visible = string.IsNullOrEmpty(formState.PythonPath) == false;

            try
            {
                panelPlayers.VerticalScroll.Value = scrollValue;
            }
            catch { if (Debugger.IsAttached) throw; }
            ResumeDrawing();

            //  MessageBox.Show(scrollValue.ToString());
            this.Refresh();
        }

        void deleteButton_Click(object sender, EventArgs e)
        {
            int index = (int)((Control)sender).Tag;
            formState.RemoveProgramAddress(index);

        }
        //todo а как сделать изначальное состояние для копирования настроек? или поменять путь?
        public void PlayerCheckedChanged(object sender, EventArgs e)
        {
            var s = (CheckBox)sender;
            int index = (int)s.Tag;
            if (s.Checked)
            {
                formState.ProgramAddressesInMatch.Add(index);
                if (FrameworkSettings.PlayersPerGameMax != 0 && formState.ProgramAddressesInMatch.Count > FrameworkSettings.PlayersPerGameMax)
                {
                    formState.ProgramAddressesInMatch.RemoveAt(0);
                    //todo check java path when run
                }
            }
            else
            {
                formState.ProgramAddressesInMatch.Remove(index);
            }
        }

        private void btnAddProgram_Click(object sender, EventArgs e)
        {
            var lastAddress = formState.ProgramAddressesAll.LastOrDefault(x => x != null);
            var initialDirectory = Path.GetDirectoryName(Application.StartupPath) + "//..//Players";
            if (!Directory.Exists(initialDirectory))
                initialDirectory = Path.GetDirectoryName(Application.StartupPath) + "//..";
            if (!Directory.Exists(initialDirectory))
                initialDirectory = Path.GetDirectoryName(Application.StartupPath);
            openFileDialog1.InitialDirectory = lastAddress == null ? initialDirectory : Path.GetDirectoryName(lastAddress);
            openFileDialog1.Filter = "Исполняемые файлы|*.exe;*.jar;*.py";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if (CheckSelectSetJavaPath(new List<string> { openFileDialog1.FileName }) && CheckSelectSetPythonPath(new List<string> { openFileDialog1.FileName }))
                {
                    formState.ProgramAddressesAll.Add(openFileDialog1.FileName);
                    if (FrameworkSettings.PlayersPerGameMax != 0 && formState.ProgramAddressesInMatch.Count < FrameworkSettings.PlayersPerGameMax)
                    {
                        formState.ProgramAddressesInMatch.Add(formState.ProgramAddressesAll.Count - 1);
                    }
                }
            }
        }

        bool CheckSelectSetJavaPath(List<string> programAddresses)
        {
            //!!!дублирование кода ява и питон
            if (string.IsNullOrEmpty(formState.JavaPath) == false && File.Exists(formState.JavaPath))
                return true; //уже задан
            bool required = programAddresses.Any(x => x.Substring(x.Length - 4) == ".jar");

            if (required == false)
                return true;


            var folderDialog = new FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.Description = @"Укажите директорию Java (например, 
C:\Program Files (x86)\Java\jdk1.7.0_55 или 
C:\Program Files\Java\jre1.8.0_73 )";
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string javaPath = (folderDialog.SelectedPath + "\\bin\\java.exe");
                if (File.Exists(javaPath))
                {
                    formState.JavaPath = javaPath;
                    return true;
                }
                else
                {
                    MessageBox.Show("Выбранная директория не содержит путь /bin/java.exe");
                    return false;
                }
            }
            else
                return false;
        }

        bool CheckSelectSetPythonPath(List<string> programAddresses)
        {
            //!!!дублирование кода ява и питон
            if (string.IsNullOrEmpty(formState.PythonPath) == false && File.Exists(formState.PythonPath))
                return true; //уже задан
            bool required = programAddresses.Any(x => x.Substring(x.Length - 3) == ".py");

            if (required == false)
                return true;


            var folderDialog = new FolderBrowserDialog
            {

                Description = "Выберите ПАПКУ, которая содержит файл python.exe",
                ShowNewFolderButton = false

            };
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string pythonPath = (folderDialog.SelectedPath + "\\python.exe");
                if (File.Exists(pythonPath))
                {
                    formState.PythonPath = pythonPath;
                    return true;
                }
                else
                {
                    MessageBox.Show("Выбранная директория не содержит файл /python.exe");
                    return false;
                }
            }
            else
                return false;
        }

        private void btnAddHuman_Click(object sender, EventArgs e)
        {
            formState.ProgramAddressesAll.Add(null);
            if (FrameworkSettings.PlayersPerGameMax != 0 && formState.ProgramAddressesInMatch.Count < FrameworkSettings.PlayersPerGameMax)
            {
                formState.ProgramAddressesInMatch.Add(formState.ProgramAddressesAll.Count - 1);
            }
        }

        private void btnChangeOrder_Click(object sender, EventArgs e)
        {
            if (formState.ProgramAddressesInMatch.Count != 0)
            {
                int last = formState.ProgramAddressesInMatch.Last();
                formState.ProgramAddressesInMatch.RemoveAt(formState.ProgramAddressesInMatch.Count - 1);
                formState.ProgramAddressesInMatch.Insert(0, last);
            }
        }

        private void btnClearSelection_Click(object sender, EventArgs e)
        {
            formState.ProgramAddressesInMatch.Clear();
        }




        private void btnSaveRoomDescription_Click(object sender, EventArgs e)
        {

        }



        private void btnChangeJavaPath_Click(object sender, EventArgs e)
        {
            formState.JavaPath = null;
            CheckSelectSetJavaPath(formState.ProgramAddressesAll.ToList());
        }

        private void btnChangePythonPath_Click(object sender, EventArgs e)
        {
            formState.PythonPath = null;
            CheckSelectSetPythonPath(formState.ProgramAddressesAll.ToList());
        }

        private void StartForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ExternalProgramExecuter.DeleteTempSubdir(); //todo framework

        }

        private void StartForm_KeyDown(object sender, KeyEventArgs e)
        {
            //пусть и на продуктиве будет
             if (Debugger.IsAttached)
            {
                if (e.Control && e.Shift && e.KeyCode == Keys.C) //config
                {
                    //пересоздать конфиг
                    try
                    {
                        File.Delete(FormState.saveLoadPath);
                        LoadFormState();
                        needRefreshControls = true;
                    }
                    catch { }
                }
            }
        }

        private void btnRunReplay_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = formState.ReplayFolder,
                Filter = "Файлы повтора|*.rpl"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                GameCore<FormState, Turn, Round, Player>.TryRunAsSingleton((x, y) => new Game(x, y), new List<FormState> { formState }, openFileDialog.FileName);
            }
        }

        private void btnSelectReplayFolder_Click(object sender, EventArgs e)
        {
            var selectFolder = new FolderBrowserDialog
            {
                SelectedPath = formState.ReplayFolder
            };
            if (selectFolder.ShowDialog() == DialogResult.OK)
            {
                formState.ReplayFolder = selectFolder.SelectedPath;
            }
        }


        #region score


        void SetScore(List<GameResult> gameResult)
        {
            while(formState.GameResults.Count>0)
                formState.GameResults.RemoveAt(0);
            gameResult.ForEach(x=>formState.GameResults.Add(x));
        }

        List<GameResult> cur = new List<GameResult>();

        List<GameResult> GetScore()
        {
            return new List<GameResult>(formState.GameResults);
        }



        private void btnRun_Click(object sender, EventArgs e)
        {
            if (CheckSelectSetJavaPath(formState.ProgramAddressesAll.ToList()) == false)
                return;
            if (CheckSelectSetPythonPath(formState.ProgramAddressesAll.ToList()) == false)
                return;
            if (formState.ProgramAddressesInMatch.Count < FrameworkSettings.PlayersPerGameMin)
            {
                MessageBox.Show("Для запуска матча требуется игроков: " + FrameworkSettings.PlayersPerGameMax.ToString());
                return;
            }
            //нужно встряхнуть рандомайзер
            if (formState.UseFixedRandomSeed)
                formState.RandomSeed = formState.FixedRandomSeed;
            else
                formState.RandomSeed = new Random().Next();


            if (formState.FixedRandomSeed == 2042017)
            {
                //matches with bot
                MatchesWithBot();
            }
            else
            {
                //usual
                GameCore<FormState, Turn, Round, Player>.TryRunAsSingleton((x, y) => new Game(x, y), new List<FormState> { formState }, null);
            }

            formState.GameParamsList.Clear(); //todo remove
        }

        string easyMd5 = "4c‌​06‌​33‌​b8‌​66‌​6e‌​99‌​5c‌​98‌​84‌​6b‌​20‌​d0‌​75‌​73‌​99";
        string normalMd5 = "b8‌​ba‌​08‌​6a‌​0d‌​84‌​17‌​2e‌​44‌​03‌​6b‌​d7‌​e8‌​cb‌​3f‌​09";
        string hardMd5 = "8c‌​3a‌​bc‌​22‌​0d‌​65‌​54‌​2e‌​15‌​06‌​16‌​53‌​10‌​de‌​91‌​d0";
        string veryHardMd5 = "e2‌​b0‌​8a‌​e8‌​7d‌​9f‌​ea‌​3b‌​b1‌​38‌​e4‌​40‌​94‌​b9‌​62‌​99";
        string extremeMd5 = "c5‌​fe‌​75‌​0f‌​ea‌​c1‌​2f‌​14‌​6b‌​8d‌​e0‌​4b‌​4f‌​48‌​1c‌​44";
        void MatchesWithBot()
        {
            CheckAllBotsHere();
        }


        private void CheckAllBotsHere()
        {

            try
            {
                using (var md5 = MD5.Create())
                {
                    var hashes = formState.ProgramAddressesAll.Select(x => md5.ComputeHash(File.OpenRead(x)))
                        .Select(x => BitConverter.ToString(x).Replace("-", "‌​").ToLower()).ToList();

                    if (hashes.Contains(normalMd5) == false)
                    {
                        MessageBox.Show("Пожалуйста, добавьте программу Normal.exe");
                        return;
                    }
                    if (hashes.Contains(hardMd5) == false)
                    {
                        MessageBox.Show("Пожалуйста, добавьте программу Hard.exe");
                        return;
                    }
                    if (hashes.Contains(veryHardMd5) == false)
                    {
                        MessageBox.Show("Пожалуйста, добавьте программу VeryHard.exe");
                        return;
                    }
                    if (hashes.Contains(extremeMd5) == false)
                    {
                        MessageBox.Show("Пожалуйста, добавьте программу Extreme.exe");
                        return;
                    }

                    var notOurBots = hashes.Where(x => x != normalMd5 && x != easyMd5 && x != hardMd5 && x != veryHardMd5 && x != extremeMd5).ToList();
                    if (notOurBots.Count > 1 || notOurBots.Count == 0)
                    {
                        MessageBox.Show("Пожалуйста, оставьте в списке программ только стратегии организаторов и ОДНУ свою стратегию (удалите остальные из списка)");
                        return;
                    }

                    var list = new List<string>
                    {
                        normalMd5,
                        hardMd5,
                        veryHardMd5,
                        extremeMd5
                    };
                    while (true)
                    {
                        var currentResult = GetScore();
                        if (currentResult.Count >= 4)
                            break;

                        formState.LastGameResult = null;
                        formState.ProgramAddressesInMatch[0] = hashes.FindIndex(x => x == notOurBots[0]);
                        formState.ProgramAddressesInMatch[1] = hashes.FindIndex(x => x == list[currentResult.Count]);
                        GameCore<FormState, Turn, Round, Player>.TryRunAsSingleton((x, y) => new Game(x, y), new List<FormState> { formState }, null);
                        if (formState.LastGameResult == null)
                        {
                            return; //выключил игру раньше, а так делать не нужно
                            // currentResult.Add(new GameResult { botNumber = currentResult.Count, BotScore = 3, OurScore = 3, possession = 0 });
                        }


                        currentResult.Add(formState.LastGameResult);
                        SetScore(currentResult);//зафиксировали
                        RefreshScoreLabel();

                    }


                }
            }
            catch
            {
                MessageBox.Show("Ошибка при верификации программ");
                if (Debugger.IsAttached)
                    throw;
            }
        }

        private void RefreshScoreLabel()
        {
            var sb = new StringBuilder();
            var currentResult = GetScore();
            if (currentResult.Count == 0)
            {
                labelScoreWithBots.Text = "";
                return;
            }
            currentResult.ForEach(x => sb.AppendLine(string.Format("Матч с {0}: {1} - {2}, владение: {3}",
                x.BotName,
                x.OurScore,
                x.BotScore,
                x.possession 
                )));

            sb.AppendLine(string.Format("Побед: {0}, ничьих: {1}, разница мячей: {2}, владение: {3}",
                currentResult.Count(x => x.OurScore > x.BotScore),
                currentResult.Count(x => x.OurScore == x.BotScore),
                currentResult.Sum(x => x.OurScore - x.BotScore),
                currentResult.Sum(x => x.possession)
                ));

            labelScoreWithBots.Text = sb.ToString();
        }




        #endregion
    }

    public class GameResult
    {
        public string BotName;// { get {return botNumber == 1 ? "Normal.exe" : botNumber == 2 ? "Hard.exe" : botNumber == 3 ? "VeryHard.exe" : botNumber == 4 ? "Extreme.exe" : ""; } }
        public int OurScore;
        public int BotScore;
        /// <summary>
        /// 0-10000
        /// </summary>
        public int possession;
    }
}
