using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Game2D.Game;
using Game2D;
using System.Xml.Serialization;

namespace MagicStorm
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        
        List<ParamsFromMainFormToGame> gameList = new List<ParamsFromMainFormToGame>();
        ParamsFromMainFormToGame currentGame = new ParamsFromMainFormToGame();
        


        #region Config save-load
        

        public void SaveConfig(string javaPath = null)
        {
            if (loading)
                return;
            try
            {
                currentGame.AnimationSpeedInPercent = trackBar1.Value*10;
                currentGame.firstIsControlledByHuman = cbPlayer1.Checked;
                currentGame.secondIsControlledByHuman = cbPlayer2.Checked;
                currentGame.firstProgramAddress = edtPlayer1.Text;
                currentGame.secondProgramAddress = edtPlayer2.Text;
                currentGame.lastOpenFileDialog = openFileDialog1.FileName;
                currentGame.replayPath = openFileDialog2.FileName;
                currentGame.Brightness = trackBar2.Value ;
                if(javaPath != null)
                    currentGame.javaPath = javaPath;
                Utility.TryWriteToXmlFile<ParamsFromMainFormToGame>(Application.StartupPath + "//config.cfg", currentGame);
            }
            catch
            {

            }
        }

        bool loading = false;
        public void LoadConfig()
        {
            loading = true;
            try
            {
                currentGame = Utility.TryReadFromXmlFile<ParamsFromMainFormToGame>(Application.StartupPath + "//config.cfg");
                if (currentGame == null)
                    currentGame = new ParamsFromMainFormToGame();
                
                //   cbHistory.Checked = bool.Parse(ss[2]);
                trackBar1.Value = currentGame.AnimationSpeedInPercent/10;
                trackBar2.Value = currentGame.Brightness;
                
                edtPlayer1.Text = currentGame.firstProgramAddress ?? "";
                edtPlayer2.Text = currentGame.secondProgramAddress ?? "";
                cbPlayer1.Checked = currentGame.firstIsControlledByHuman;
                cbPlayer2.Checked = currentGame.secondIsControlledByHuman;
                // edtHistory.Text = reader.ReadLine();

                openFileDialog1.FileName = currentGame.lastOpenFileDialog;
                openFileDialog2.FileName = currentGame.replayPath;
            }
            catch(Exception ex)
            {
            }
            finally
            {
                loading = false;
            }
        }
#endregion

        #region запуск игры по кнопке "начать"
        private void button1_Click(object sender, EventArgs e)
        {
            SaveConfig();

            if(gameList.Count == 0)
                AddGameParams();

            if (gameList.Count > 0)
            {
                Const.ParamsFromMainFormToGame = gameList;
                new GameForm().ShowDialog();
                this.Close(); //во фреймворке проблемы решены, но здесь лучше повторно ничего не запускать
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddGameParams();
            SaveConfig();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (gameList.Count > 0)
            {
                gameList.RemoveAt(gameList.Count - 1);
                
            }
            RefreshListButtonText();
            SaveConfig();
        }
        private void AddGameParams()
        {

            if ((!cbPlayer1.Checked && !File.Exists(edtPlayer1.Text))
                || (!cbPlayer2.Checked && !File.Exists(edtPlayer2.Text)))
            {
                MessageBox.Show("Не задан путь к программе");
                return;
            }

            //запуск в другом потоке

            currentGame.AddToList(gameList);
            RefreshListButtonText();
        }

        void RefreshListButtonText()
        {
            button3.Text = "Добавить в список (" + gameList.Count.ToString() + ")";
           
                button1.Text = gameList.Count == 0?  "Запустить игру":"Запустить список игр";
        }
        #endregion

        #region обработка событий контролов
        private void cbPlayer1_CheckedChanged(object sender, EventArgs e)
        {
            edtPlayer1.Enabled = !cbPlayer1.Checked;
            SaveConfig();
        }

        private void cbPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            edtPlayer2.Enabled = !cbPlayer2.Checked;
            SaveConfig();
        }

        private void btnExchange_Click(object sender, EventArgs e)
        {
            string t = edtPlayer1.Text; edtPlayer1.Text = edtPlayer2.Text; edtPlayer2.Text = t;
            bool check = cbPlayer1.Checked; cbPlayer1.Checked = cbPlayer2.Checked; cbPlayer2.Checked = check;
            SaveConfig();
        }

        private void btnPlayer1_Click(object sender, EventArgs e)
        {
            SelectProgramAddress(edtPlayer1, cbPlayer1);
        }

        private void btnPlayer2_Click(object sender, EventArgs e)
        {
            SelectProgramAddress(edtPlayer2, cbPlayer2);
        }

        private void SelectProgramAddress(TextBox textBox, CheckBox checkBox)
        {
            if (!File.Exists(openFileDialog1.FileName))
                if(textBox.Name == "edtPlayer1")
                    openFileDialog1.FileName = Path.GetFullPath(Application.StartupPath + "\\..\\Players\\easy.exe");
                else
                    openFileDialog1.FileName = Path.GetFullPath(Application.StartupPath + "\\..\\Players\\normal.exe");
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(openFileDialog1.FileName);

            var tempFileDialogFile = openFileDialog1.FileName;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileName.Substring(openFileDialog1.FileName.Length - 4) == ".jar"
                    && (currentGame.javaPath == null || !File.Exists(currentGame.javaPath)))
                {
                    folderBrowserDialog2.ShowNewFolderButton = false;
                    folderBrowserDialog2.Description = "Укажите директорию Java (например, " + @"C:\Program Files\Java\jre1.8.0_73 )";
                    if (folderBrowserDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string javaPath = (folderBrowserDialog2.SelectedPath + "\\bin\\java.exe");
                        if (File.Exists(javaPath))
                        {
                            checkBox.Checked = false;
                            textBox.Text = openFileDialog1.FileName;

                            SaveConfig(javaPath);
                        }
                        else
                        {
                            openFileDialog1.FileName = tempFileDialogFile;
                            MessageBox.Show("Выбранная директория не содержит файл bin/java.exe");
                        }
                    }
                }
                else
                {
                    checkBox.Checked = false;
                    textBox.Text = openFileDialog1.FileName;

                    SaveConfig();
                }


            }
        }

        

        private void cbHistory_CheckedChanged(object sender, EventArgs e)
        {
          //  edtHistory.Enabled = cbHistory.Checked;
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
              //  cbHistory.Checked = true;
              //  edtHistory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            lblTime.Text = (trackBar1.Value * 10).ToString() + "%";
            SaveConfig();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           // cbMap.Items.Add(cbMap.Text);
        }
#endregion
        
        #region загрузка и сохранение контролов в файл
        private void FormMain_Load(object sender, EventArgs e)
        {
            
            LoadConfig();
            
            
            if (string.IsNullOrEmpty(edtPlayer1.Text) || !File.Exists(edtPlayer1.Text))
            {
                if (File.Exists(Application.StartupPath + "\\..\\Players\\easy.exe"))
                {
                    openFileDialog1.FileName = Path.GetFullPath(Application.StartupPath + "\\..\\Players\\easy.exe");
                    edtPlayer1.Text = Path.GetFullPath(Application.StartupPath + "\\..\\Players\\easy.exe");
                    edtPlayer2.Text = Path.GetFullPath(Application.StartupPath + "\\..\\Players\\normal.exe");
                }
            }
        }
        #endregion

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
          string text = @"
Авторы: 
Душкин Денис, ФКН, 1 курс, brandstein@yandex.ru
Землянухин Михаил, ФКН, 4 курс, abc-0-4@yandex.ru

Руководитель: 
Соломатин Дмитрий Иванович, ФКН, преподаватель кафедры ПиИТ

ВГУ, Факультет Компьютерных Наук, Неделя информационных технологий 2016.
";
            MessageBox.Show(text);
        }

        private void управлениеСКлавиатурыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = @"Покупка юнита: наведите указатель мыши на клетку поля и нажмите цифру 1-5.
Выбор точки назначения: кликните по клетке поля. 
Передача хода: ENTER. Покинуть игру: ESC.
Включение\выключение паузы: пробел.

В режиме паузы можно получить входные и выходные данные для любого хода. Для этого выберите стрелками ход и нажмите I (для получения input.txt) или O (для получения output.txt). Откройте любой текстовый редактор и нажмите Ctrl+V (вставить).";
            MessageBox.Show(text);
        
            
        
        }

        private void btnAddReplay_Click(object sender, EventArgs e)
        {
            if(!File.Exists(openFileDialog2.FileName))
            {
                openFileDialog2.FileName = Path.GetFullPath( Application.StartupPath + "\\..\\Replays\\replay.rpl");
            }
            openFileDialog2.InitialDirectory = Path.GetDirectoryName(openFileDialog2.FileName);
            openFileDialog2.Filter = "Файл с повтором|*.rpl";
            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                currentGame.ThisIsReplayGame = true;
                SaveConfig();
                currentGame.AddToList(gameList);
                RefreshListButtonText();
            }
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            FramePainter.BrightenFactor = trackBar2.Value /2.0;
            SaveConfig();
        }


        

        

    }
}
