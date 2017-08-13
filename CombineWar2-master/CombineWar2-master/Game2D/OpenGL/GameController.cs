using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;
using System.Diagnostics;
using OpenTK;
using Game2D.Opengl;
using Game2D.Game;
using Game2D;
using OpenTK.Graphics;

namespace Game2D
{
    class GameController
    {

        int _windowCode;
        Dictionary<string, int> _textureCodes;

        System.Windows.Forms.Timer _loopTimer = new System.Windows.Forms.Timer { Interval = 16 };

        GameForm _parentForm;
        GLControl control;
        KeyboardState _keyboardState;
        IGame _game;
        double _windowWidth, _windowHeight;
        public GameController(GLControl control, GameForm gameForm)
        {
            _game = new Game.Game();
            _parentForm = gameForm;
            this.control = control;
            _keyboardState = new KeyboardState();
            //GlInput.Init(gameForm, control); класс сыроват, пока лучше его не использовать

            GraphicsContext.CurrentContext.VSync = true;
            //keyboard support
            _windowWidth = control.Width;
            _windowHeight = control.Height;
            _parentForm.Text = Config.WindowName;
            control.MouseMove += new MouseEventHandler((o, e) => PassiveMotion(e.X, e.Y));
            control.MouseClick += new MouseEventHandler((o, e) => _keyboardState.MouseClick = true);
            _parentForm.KeyPreview = true;
            _parentForm.KeyDown += new KeyEventHandler((o, e) => { try { _keyboardState.KeyPress((byte)e.KeyValue); } catch { } });
            _parentForm.KeyUp += new KeyEventHandler((o, e) => { try { _keyboardState.KeyUp((byte)e.KeyValue); } catch { } });
            //------


            //инициализация openGL
            Initializer.SetupViewport(control);
            control.Resize += new EventHandler((o, e) => Initializer.SetupViewport(o as GLControl));
            _textureCodes = Initializer.LoadTextures();

            //игровой круг
            _loopTimer.Start();
            _loopTimer.Tick += MainLoop;

            _parentForm.FormClosed += new FormClosedEventHandler((o, e) => _loopTimer.Stop());
        }



        bool previousStateDrawed = true;
        void MainLoop(object sender, EventArgs e)
        {
            //вроде эти проверки излишни

            if (!previousStateDrawed) return; //если вдруг не успели отрисоваться за время кадра, подождем следующего тика
             previousStateDrawed = false;

         //   GlInput.EveryFrameStartRefresh();
            Frame  frame = _game.Process(_keyboardState);
            if (frame == null)
            {
                CloseWindow();
                return;
            }
            FramePainter.DrawFrame(frame, _textureCodes);
            control.SwapBuffers();
            _keyboardState.StepEnded();
            previousStateDrawed = true; //справились с рисованием
        }




        public void PassiveMotion(int x, int y)
        {
            _keyboardState.MousePos = new Point2((double)x * Config.ScreenWidth / _windowWidth, (double)y * Config.ScreenHeight / _windowHeight);
        }



        //public void KeyUp(byte key, int x, int y)
        //{
        //    _keyboardState.KeyUp(key);
        //}

        //public void Key(byte key, int x, int y)
        //{
        //    if (key == 27)
        //    {
        //        Glut.glutLeaveGameMode();
        //        Glut.glutDestroyWindow(_windowCode);
        //    }
        //    _keyboardState.KeyPress(key);
        //}

        //public void KeySpecialUp(int key, int x, int y)
        //{
        //    if (key >= 0 && key <= 11) _keyboardState.KeyUp((byte)(key + 131)); //f1-f12
        //    if (key >= 100 && key <= 103) _keyboardState.KeyUp((byte)(key - 63)); //arrows
        //}

        //public void KeySpecial(int key, int x, int y)
        //{
        //    if (key >= 0 && key <= 11) _keyboardState.KeyPress((byte)(key + 131)); //f1-f12
        //    if (key >= 100 && key <= 103) _keyboardState.KeyPress((byte)(key - 63)); //arrows
        //}

       

        void CloseWindow()
        {
            _parentForm.Close();
        }











    }
}

