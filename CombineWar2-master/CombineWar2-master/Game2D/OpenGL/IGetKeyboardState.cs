using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Opengl
{
    interface IGetKeyboardState
    {
        //узнать время, в течение которого нажата кнопка действия клавиатуры
        int GetActionTime(EKeyboardAction action);
        //Свойства мыши
        Point2 MousePos { get;  }
        bool MouseClick { get;  }
        bool MouseRightClick { get;  }
    }
}
