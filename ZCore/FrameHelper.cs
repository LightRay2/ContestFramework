using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class FrameHelper
    {
        public static void Rectangle(ref Frame frame, ESprite sprite, double x, double y, double w, double h, double lineWidth=-1)
        {
            double pointSz = lineWidth == -1 ? Config.ScreenWidth / 200 : lineWidth;
            frame.Add(new Sprite(sprite, w, pointSz, new Vector2(x + w / 2, y)));
            frame.Add(new Sprite(sprite, pointSz, h, new Vector2(x, y + h / 2)));
            frame.Add(new Sprite(sprite, w, pointSz, new Vector2(x + w / 2, y + h)));
            frame.Add(new Sprite(sprite, pointSz, h, new Vector2(x + w, y + h / 2)));

        }
    }
}
