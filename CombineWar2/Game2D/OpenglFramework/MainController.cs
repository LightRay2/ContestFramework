using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;
using System.Diagnostics;

using Tao.OpenGl;
using Tao.FreeGlut;
using Tao.Platform.Windows;
using Tao.DevIl;

namespace Game2D.Opengl
{
    class MainController
    {

        int _windowCode;
        Dictionary<string, int> _textureCodes;

        int _windowWidth, _windowHeight; //пригодится, чтобы пересчитывать координаты мышки в игровые

        KeyboardState _keyboardState;
        IGame _game;
        public MainController(IGame game, int windowWidth, int windowHeight, bool tryFullScreen = false)
        {
            _keyboardState = new KeyboardState();
            _game = game;
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;

            //инициализация openGL
            _windowCode = OpenglInitializer.CreateWindow(windowWidth, windowHeight, tryFullScreen);
            OpenglInitializer.SetDisplayModes();
            _textureCodes =  OpenglInitializer.LoadTextures();
        }


        public void SetMainLoop()
        {
            //отлов действий пользователя
            Glut.glutMotionFunc(ClickedMotion);
            Glut.glutMouseFunc(Mouse);
            Glut.glutPassiveMotionFunc(PassiveMotion);
            Glut.glutKeyboardFunc(Key);
            Glut.glutKeyboardUpFunc(KeyUp);
            Glut.glutSpecialFunc(KeySpecial);
            Glut.glutSpecialUpFunc(KeySpecialUp);

            //старт игрового цикла
            Glut.glutTimerFunc(Config.TimePerFrame, MainProcess, 0);
            Glut.glutMainLoop();
        }


        
        bool previousStateDrawed=true;

        void MainProcess(int value)
        {
            if (!previousStateDrawed) return; //если вдруг не успели отрисоваться за время кадра, подождем следующего тика
            previousStateDrawed = false;

            Glut.glutTimerFunc(Config.TimePerFrame, MainProcess, 0);//сразу засекаем следующие миллисекунды
           
            Frame frame = _game.Process(_keyboardState);
            _keyboardState.StepEnded(); //игра считала кнопки, время классу сделать плановые действия
            
            //рисуем, если есть что рисовать
            if (frame == null) CloseWindow();
            else Painter.DrawFrame((Frame)frame, _textureCodes);

            previousStateDrawed = true; //справились с рисованием
        }

        //------------------------------------------------------------------------------------
        //Дальше несущественный код
        //-------------------------------------

        
        public void PassiveMotion(int x, int y)
        {
            _keyboardState.MousePos = new Point2((double)x * Config.ScreenWidth / _windowWidth, (double)y * Config.ScreenHeight / _windowHeight);
        }

        public void Mouse(int button, int state, int x, int y)
        {
            if (state == Glut.GLUT_DOWN )
            {
                _keyboardState.MouseClick = button == Glut.GLUT_LEFT_BUTTON;
                _keyboardState.MouseRightClick = button == Glut.GLUT_RIGHT_BUTTON;
            }
        }

        public void ClickedMotion(int x, int y)
        {
            PassiveMotion(x, y); // все равно одинаковые действие
        }

        public void KeyUp(byte key, int x, int y)
        {
            _keyboardState.KeyUp(key);
        }

        public void Key(byte key, int x, int y)
        {
            if (key == 27)
            {
                Glut.glutLeaveGameMode();
                Glut.glutDestroyWindow(_windowCode);
            }
            _keyboardState.KeyPress(key);
        }

        public void KeySpecialUp(int key, int x, int y)
        {
            if (key >= 0 && key <= 11) _keyboardState.KeyUp((byte)(key + 131)); //f1-f12
            if (key >= 100 && key <= 103) _keyboardState.KeyUp((byte)(key - 63)); //arrows
        }

        public void KeySpecial(int key, int x, int y)
        {
            if (key >= 0 && key <= 11) _keyboardState.KeyPress((byte)(key + 131)); //f1-f12
            if (key >= 100 && key <= 103) _keyboardState.KeyPress((byte)(key - 63)); //arrows
        }

        void CloseWindow()
        {
            Glut.glutLeaveGameMode();
            Glut.glutDestroyWindow(_windowCode);
        }











    }
}

