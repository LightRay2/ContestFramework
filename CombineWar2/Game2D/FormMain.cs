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
        Thread game;
        public FormMain()
        {
            InitializeComponent();
        }

        
        List<ParamsFromMainFormToGame> gameList = new List<ParamsFromMainFormToGame>();
        ParamsFromMainFormToGame currentGame = new ParamsFromMainFormToGame();
        


        #region Config save-load
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public void SaveConfig()
        {
            try
            {
                currentGame.AnumationSpeedInPercent = trackBar1.Value*10;
                currentGame.firstIsControlledByHuman = cbPlayer1.Checked;
                currentGame.secondIsControlledByHuman = cbPlayer2.Checked;
                currentGame.firstProgramAddress = edtPlayer1.Text;
                currentGame.secondProgramAddress = edtPlayer2.Text;
                currentGame.lastOpenFileDialog = openFileDialog1.FileName;
                WriteToXmlFile<ParamsFromMainFormToGame>(Application.StartupPath + "//config.cfg", currentGame);
            }
            catch
            {

            }
        }

        public void LoadConfig()
        {
            try
            {
                currentGame = ReadFromXmlFile<ParamsFromMainFormToGame>(Application.StartupPath + "//config.cfg");
                
                
                cbPlayer1.Checked = currentGame.firstIsControlledByHuman;
                cbPlayer2.Checked = currentGame.secondIsControlledByHuman ;
                //   cbHistory.Checked = bool.Parse(ss[2]);
                trackBar1.Value = currentGame.AnumationSpeedInPercent/10;

                edtPlayer1.Text = currentGame.firstProgramAddress ?? "";
                edtPlayer2.Text = currentGame.secondProgramAddress ?? "";
                // edtHistory.Text = reader.ReadLine();

                openFileDialog1.FileName = currentGame.lastOpenFileDialog;
            }
            catch(Exception ex)
            {
            }
        }
#endregion

        #region запуск игры по кнопке "начать"
        private void button1_Click(object sender, EventArgs e)
        {
            SaveConfig();
            if (game != null && game.IsAlive)
            {
                MessageBox.Show("Запрещено запускать 2 игры из одной формы!");
                return;
            }

            if(gameList.Count == 0)
                AddGameParams();

            if (gameList.Count > 0)
            {
                Const.ParamsFromMainFormToGame = gameList;
                new Form1().ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddGameParams();
            button1.Text = "Запустить список игр";
            SaveConfig();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (gameList.Count > 0)
            {
                gameList.RemoveAt(gameList.Count - 1);
                if (gameList.Count == 0)
                    button1.Text = "Запустить игру";
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
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                cbPlayer1.Checked = false;
                edtPlayer1.Text = openFileDialog1.FileName;
                SaveConfig();
            }
        }

        private void btnPlayer2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                cbPlayer2.Checked = false;
                edtPlayer2.Text = openFileDialog1.FileName;
                SaveConfig();
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
            
            
            if (string.IsNullOrEmpty(edtPlayer1.Text))
            {
                if (File.Exists(Application.StartupPath + "\\..\\Players\\easy.exe"))
                {
                    openFileDialog1.FileName = Path.GetFullPath(Application.StartupPath + "\\..\\Players\\normal.exe");
                    edtPlayer1.Text = openFileDialog1.FileName;
                    edtPlayer2.Text = openFileDialog1.FileName;
                }
            }
        }
        #endregion

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
          string text = @"
Реализация: Душкин Денис, ФКН, 1 курс
Идея: Землянухин Михаил, ФКН, 4 курс (abc-0-4@yandex.ru)

Руководитель: Соломатин Дмитрий Иванович, ФКН, преподаватель кафедры ПиИТ

ВГУ, Факультет Компьютерных Наук, 2016 г.
";
            MessageBox.Show(text);
        }

        

        

    }
}
