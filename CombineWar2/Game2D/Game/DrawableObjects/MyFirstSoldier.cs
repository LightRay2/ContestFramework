using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.DrawableObjects
{
    class MyFirstSoldier : IDrawable
    {
        public Point2 start = new Point2(15, 15);
        public Point2 moveVector = new Point2(350, 250);
        public void Draw(ref Frame frame, double stage, int turnNumber)
        {
            Point2 add = new Point2(moveVector.x , moveVector.y);
            add.Normalize(add.Length()*stage);
            Vector2 realPosition = new Vector2( start.x+add.x, start.y + add.y);
            frame.Add(new Sprite(ESprite.tank, 30, 40, realPosition, 0));
            frame.Add(new Sprite(ESprite.tank, 30, 40, realPosition, 1));
        }

    }
}
