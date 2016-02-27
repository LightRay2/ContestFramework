using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.DrawableObjects
{
    class ComplexDynamicObject
    {
        public List<DynamicObject> objs = new List<DynamicObject>();

        public void Draw (ref Frame frame, double stage, int turnNumber)
        {
            foreach (DynamicObject obj in objs)
                obj.Draw(ref frame, stage, turnNumber);
        }
    }
}
