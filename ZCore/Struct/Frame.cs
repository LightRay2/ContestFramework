using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    /// <summary>
    /// Кадр, который игра обязана вернуть контроллеру для отрисовки
    /// </summary>
    public class Frame
    {
        public List<Sprite> sprites = new List<Sprite>();
        public List<Text> texts = new List<Text>();
        /// <summary>
        /// левый верхний угол
        /// </summary>
        public Vector2 camera ;

        /// <summary>
        /// если включен, к координатам добавляемых объектов автоматом прибавляется сдвиг камеры. 
        /// Лучше исп. в таком порядке: нарисовали объекты и определились с положением камеры, включили мод, нарисовали меню
        /// </summary>
        public bool menuModOn = false;

        public void Add(params Sprite[] sprites)
        {
            this.sprites.AddRange(sprites);
        }

        public void Add(params Text[] texts)
        {
            this.texts.AddRange(texts);
        }

    }
}
