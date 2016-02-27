using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Opengl
{
    /// <summary>
    /// Учитывает все клавиши на клавиатуре, клик левой и правой кнопкой мыши, позицию указателя 
    /// </summary>
    class KeyboardState :IGetKeyboardState
    {

        int[] _actionTime; // для всех кнопок время, в течение которого они нажаты

        public KeyboardState()
        {
            _actionTime = new int[256];
            for (int i = 0; i < _actionTime.Length; i++) _actionTime[i] = 0;
        }

        /// <summary>
        /// Метод для контроллера. Класс должен выполнить некоторые действия, когда очередная итерация таймера завершена
        /// </summary>
        public void StepEnded()
        {
            MouseClick = false;
            MouseRightClick = false;
            for (int i = 0; i < _actionTime.Length; i++)
                if (_actionTime[i] > 0)
                    _actionTime[i]++;
        }

        //-------------------------------------------------------------------------
        public void KeyPress(byte key)
        {
            if(_actionTime[key] == 0) _actionTime[key] = 1;
        }
        public void KeyUp(byte key)
        {
            _actionTime[key] = 0;
        }

        //узнать время, в течение которого нажата кнопка действия клавиатуры
        public int GetActionTime(EKeyboardAction action) { return _actionTime[Config.Keys[action]]; }
        //Свойства мыши
        public Point2 MousePos { get; set; }
        public bool MouseClick { get; set; }
        public bool MouseRightClick { get; set; }
    }
}
