using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;
using System.Diagnostics;
using OpenTK;
using Framework.Opengl;

namespace Framework
{
    class GameController
    {

        int _windowCode;
        Dictionary<string, int> _textureCodes;

        System.Windows.Forms.Timer _loopTimer = new System.Windows.Forms.Timer { Interval = 16 };

        GameForm _parentForm;
        GLControl control;
        KeyboardState _keyboardState;
        Func<IGetKeyboardState, Frame> _processMethod;
        public GameController(GLControl control, Func<IGetKeyboardState, Frame> processMethod, GameForm gameForm)
        {
            _parentForm = gameForm;
            this.control = control;
            _keyboardState = new KeyboardState();
            GlInput.Init(gameForm, control);
            _processMethod = processMethod;

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

            GlInput.EveryFrameStartRefresh();
            Frame frame = _processMethod(_keyboardState);
            FramePainter.DrawFrame(frame, _textureCodes);
            control.SwapBuffers();

            previousStateDrawed = true; //справились с рисованием
        }





        void CloseWindow()
        {
            _parentForm.Close();
        }











    }
}

