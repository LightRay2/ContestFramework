using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    
    public class SpriteList
    {
        public static Dictionary<Enum, SpriteList> All = new Dictionary<Enum, SpriteList>();

        public Enum SpriteEnum { get; set; }
        public Vector2d? Size { get; set; }
        public Vector2d? ScaleFromOriginal { get; set; }
        public int FrameCountHorizontal { get; set; }
        public int FrameCountVertical { get; set; }
        public double Depth { get; set; }
        public double Opacity { get; set; }

        //дальше служебные

        
        public Vector2d InitialSize { get; set; }

        //todo отрисовщик не забыть
        /// <summary>
        /// это должен учитывать отрисовщик
        /// </summary>
        public Vector2d ScaleToPowerOf2 { get; set; }
        public int OpenglTexture { get; set; }
        private SpriteList() { }
        public static void LoadDefaultSize(Enum sprite, int frameCountHorizontal=1, int frameCountVertical=1 , double depth = 0, double opacity=1)
        {
            var s = new SpriteList();
            s.SpriteEnum = sprite;
            s.FrameCountHorizontal = frameCountHorizontal;
            s.FrameCountVertical = frameCountVertical;
            s.Depth = depth;
            s.Opacity = opacity;
            All.Add(sprite, s);
        }

        public static void LoadSetSize(Enum sprite, Vector2d size, int frameCountHorizontal = 1, int frameCountVertical = 1, double depth = 0, double opacity = 1)
        {
            var s = new SpriteList();
            s.SpriteEnum = sprite;
            s.Size = size;
            s.FrameCountHorizontal = frameCountHorizontal;
            s.FrameCountVertical = frameCountVertical;
            s.Depth = depth;
            s.Opacity = opacity;
            All.Add(sprite, s);
        }

        public static void LoadSetScale(Enum sprite, double scaleFromOriginal = 1, int frameCountHorizontal = 1, int frameCountVertical = 1, double depth = 0, double opacity = 1)
        {
            var s = new SpriteList();
            s.SpriteEnum = sprite;
            s.ScaleFromOriginal = new Vector2d( scaleFromOriginal);
            s.FrameCountHorizontal = frameCountHorizontal;
            s.FrameCountVertical = frameCountVertical;
            s.Depth = depth;
            s.Opacity = opacity;
            All.Add(sprite, s);
        }

        public static void LoadSetScale(Enum sprite, Vector2d scaleFromOriginal, int frameCountHorizontal = 1, int frameCountVertical = 1, double depth = 0, double opacity = 1)
        {
            var s = new SpriteList();
            s.SpriteEnum = sprite;
            s.ScaleFromOriginal = scaleFromOriginal;
            s.FrameCountHorizontal = frameCountHorizontal;
            s.FrameCountVertical = frameCountVertical;
            s.Depth = depth;
            s.Opacity = opacity;
            All.Add(sprite, s);
        }

    }
}
