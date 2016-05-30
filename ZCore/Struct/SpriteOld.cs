using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public struct SpriteOld
    {

        public ESprite name;
        public int frame;
        public int frameCount;
        public double width, height;
        public Vector2Old pos;

        /// <summary>
        /// это поле менять не желательно, само выставится как надо
        /// </summary>
        public string texture;

        /// <param name="name">какой спрайт</param>
        /// <param name="frame">номер кадра, если анимация. Начинаем с 0</param>
        /// <param name="width">ширина</param>
        /// <param name="height">высота</param>
        /// <param name="pos">Где находится спрайт и угол поворота</param>
        public SpriteOld(ESprite name, double width, double height, Vector2Old pos, int frame=0)
        {
            if (name != ESprite.end)
                this.frameCount = Config.Sprites[name.ToString()].horFrames *
                                  Config.Sprites[name.ToString()].vertFrames;
            else
                this.frameCount = 0;
            this.name = name;
            this.frame = frame;
            this.width = width;
            this.height = height;
            this.pos = pos;
            this.texture = name.ToString();
        }

    }
}
