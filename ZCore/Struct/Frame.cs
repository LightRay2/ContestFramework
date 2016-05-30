using OpenTK;
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

        public void SpriteCustomPoint(double originPointX, double originPointY, ISprite sprite) { }
        public void SpriteCustomPoint(double originPointX, double originPointY, ISpriteSpecial sprite) { }
        public void SpriteCustomPoint(Enum sprite, double originPointX, double originPointY, double x, double y) { }
        public void SpriteCustomPoint(Enum sprite, double originPointX, double originPointY, double x, double y, double angleInDegrees) { }
        public void SpriteCustomPoint(Enum sprite, double originPointX, double originPointY, double x, double y, Vector2d direction) { }
        public void SpriteCustomPoint(Enum sprite, double originPointX, double originPointY, double x, double y, double angleInDegrees, SpecialDraw specialDraw) { }
        public void SpriteCustomPoint(Enum sprite, double originPointX, double originPointY, Vector2d position) { }
        public void SpriteCustomPoint(Enum sprite, double originPointX, double originPointY, Vector2d position, double angleInDegrees) { }
        public void SpriteCustomPoint(Enum sprite, double originPointX, double originPointY, Vector2d position, Vector2d direction) { }
        public void SpriteCustomPoint(Enum sprite, double originPointX, double originPointY, Vector2d position, double angleInDegrees, SpecialDraw specialDraw) { }

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

    }
}
