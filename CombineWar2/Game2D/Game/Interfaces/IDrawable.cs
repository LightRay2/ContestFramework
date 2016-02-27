using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game
{
    interface IDrawable
    {
        /// <summary>
        /// stage От 0 до 1 - какая часть хода уже отрисована
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="stage"></param>
        void Draw(ref Frame frame, double stage, int turnNumber);
    }
}
