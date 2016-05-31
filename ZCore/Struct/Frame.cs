using OpenTK;
using QuickFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public enum Align { left, right, center, justify };
    
    /// <summary>
    /// Кадр, который игра обязана вернуть контроллеру для отрисовки
    /// </summary>
    public class Frame //todo скрыть все всопмогательное в неявный интерейс
    {
        public Rect2d cameraViewport = new Rect2d(0,0,800, 600);
        public double cameraRotationDeg;

        public List<SpriteOld> sprites = new List<SpriteOld>();
        public List<Text> texts = new List<Text>();
        /// <summary>
        /// левый верхний угол
        /// </summary>
        public Vector2Old camera ;

        /// <summary>
        /// если включен, к координатам добавляемых объектов автоматом прибавляется сдвиг камеры. 
        /// Лучше исп. в таком порядке: нарисовали объекты и определились с положением камеры, включили мод, нарисовали меню
        /// </summary>
        public bool menuModOn = false;

        public void Add(params SpriteOld[] sprites)
        {
            this.sprites.AddRange(sprites);
        }

        public void Add(params Text[] texts)
        {
            this.texts.AddRange(texts);
        }
        /// <summary>
        /// в этой перегрузке только размер, в остальных - еще и левый верхний угол
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CameraViewport(double width, double height)
        {
            CameraViewport(0, 0, width, height);
        }
        public void CameraViewport(double x, double y, double width, double height)
        {
            cameraViewport = new Rect2d(x, y, width, height);
        }
        public void CameraViewport(Vector2d position, Vector2d size)
        {
            cameraViewport = new Rect2d(position, size);
        }
        public void CameraViewport(Vector2d position, double width, double height)
        {
            cameraViewport = new Rect2d(position, width, height);
        }
        public void CameraRotation(double angleDegrees)
        {
            cameraRotationDeg = angleDegrees;
        }

        //----text-----
        public List<Tuple<Enum, string, Vector2d, Vector2d, QFontAlignment, double?, double?>> textList =
            new List<Tuple<Enum, string, Vector2d, Vector2d, QFontAlignment, double?, double?>>();
        public void TextTopLeft(Enum font, string text, Vector2d position, QFontAlignment align = QFontAlignment.Left, double? widthLimit = null, double? depth = null)
        {
            
        }
        public void TextTopLeft(Enum font, string text, double x, double y, double? widthLimit = null, double? depth = null)
        {

        }
        public void TextBottomLeft(Enum font, string text, Vector2d position, double? widthLimit = null, double? depth = null)
        {

        }
        public void TextCenter(Enum font, string text, Vector2d position, double? widthLimit = null, double? depth = null)
        {

        }
        public void TextCustom(Enum font, string text, Vector2d origin, Vector2d position, Align align = Align.left, double? widthLimit = null, double? depth=null)
        {
            textList.Add(Tuple.Create(font, text, origin, position, (QFontAlignment)(int)align, widthLimit, depth));
        }
        //-------------------------------

        public void SpriteTopLeft(ISprite sprite){}
        public void SpriteTopLeft(ISpriteSpecial sprite){}
        public void SpriteTopLeft(Enum sprite, double x, double y) { }
        public void SpriteTopLeft(Enum sprite, double x, double y, double angleInDegrees) { }
        public void SpriteTopLeft(Enum sprite, double x, double y, Vector2d direction) { }
        public void SpriteTopLeft(Enum sprite, double x, double y, double angleInDegrees, SpecialDraw specialDraw) { }
        public void SpriteTopLeft(Enum sprite, Vector2d position) { }
        public void SpriteTopLeft(Enum sprite, Vector2d position, double angleInDegrees) { }
        public void SpriteTopLeft(Enum sprite, Vector2d position, Vector2d direction) { }
        public void SpriteTopLeft(Enum sprite, Vector2d position, double angleInDegrees, SpecialDraw specialDraw) { }

        public void SpriteBottomLeft(ISprite sprite) { }
        public void SpriteBottomLeft(ISpriteSpecial sprite) { }
        public void SpriteBottomLeft(Enum sprite, double x, double y) { }
        public void SpriteBottomLeft(Enum sprite, double x, double y, double angleInDegrees) { }
        public void SpriteBottomLeft(Enum sprite, double x, double y, Vector2d direction) { }
        public void SpriteBottomLeft(Enum sprite, double x, double y, double angleInDegrees, SpecialDraw specialDraw) { }
        public void SpriteBottomLeft(Enum sprite, Vector2d position) { }
        public void SpriteBottomLeft(Enum sprite, Vector2d position, double angleInDegrees) { }
        public void SpriteBottomLeft(Enum sprite, Vector2d position, Vector2d direction) { }
        public void SpriteBottomLeft(Enum sprite, Vector2d position, double angleInDegrees, SpecialDraw specialDraw) { }

        

        public void SpriteCenter(ISprite sprite) { }
        public void SpriteCenter(ISpriteSpecial sprite) { }
        public void SpriteCenter(Enum sprite, Vector2d position) { }
        public void SpriteCenter(Enum sprite, Vector2d position, double angleInDegrees) { }
        public void SpriteCenter(Enum sprite, Vector2d position, Vector2d direction) { }
        public void SpriteCenter(Enum sprite, Vector2d position, double angleInDegrees, SpecialDraw specialDraw) { }
        public void SpriteCenter(Enum sprite, double x, double y) { }
        public void SpriteCenter(Enum sprite, double x, double y, double angleInDegrees) { }
        public void SpriteCenter(Enum sprite, double x, double y, Vector2d direction) { }
        
        //основной
        public void SpriteCenter(Enum sprite, double x, double y, double angleInDegrees, SpecialDraw specialDraw)
        {

            if(specialDraw != null){

            }
        }

        public void SpriteCustom(double originPointX, double originPointY, ISprite sprite) { }
        public void SpriteCustom(double originPointX, double originPointY, ISpriteSpecial sprite) { }
        public void SpriteCustom(Enum sprite, double originPointX, double originPointY, double x, double y) { }
        public void SpriteCustom(Enum sprite, double originPointX, double originPointY, double x, double y, double angleInDegrees) { }
        public void SpriteCustom(Enum sprite, double originPointX, double originPointY, double x, double y, Vector2d direction) { }
        public void SpriteCustom(Enum sprite, double originPointX, double originPointY, Vector2d position, double angleInDegrees, SpecialDraw specialDraw) { }
        public void SpriteCustom(Enum sprite, double originPointX, double originPointY, Vector2d position) { }
        public void SpriteCustom(Enum sprite, double originPointX, double originPointY, Vector2d position, double angleInDegrees) { }
        public void SpriteCustom(Enum sprite, double originPointX, double originPointY, Vector2d position, Vector2d direction) { }
        public List<Tuple<Enum, Vector2d, Vector2d, double, SpecialDraw>> spriteList
            = new List<Tuple<Enum, Vector2d, Vector2d, double, SpecialDraw>>();
        public void SpriteCustom(Enum sprite, double originPointX, double originPointY, double x, double y, double angleInDegrees, SpecialDraw specialDraw) 
        {
            spriteList.Add(Tuple.Create(sprite, new Vector2d(originPointX, originPointY), new Vector2d(x,y),angleInDegrees, specialDraw));
        }

    }
}
