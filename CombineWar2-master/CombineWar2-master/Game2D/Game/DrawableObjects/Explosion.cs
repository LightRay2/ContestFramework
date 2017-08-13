using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.DrawableObjects
{
    class Explosion :IDrawable
    {
        public Point2 pos;
        public void Draw(ref Frame frame, double stage, int turnNumber)
        {
            int numberOfFrame = (int)(Math.Round(81*stage));
            frame.Add(new Sprite(ESprite.explosion, 40, 40, new Vector2(pos.x, pos.y), numberOfFrame));
        }
    }
}
