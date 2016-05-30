using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    
    class SpriteList
    {
        public static Dictionary<Enum, SpriteList> All = new Dictionary<Enum, SpriteList>();

        public Enum SpriteEnum { get; set; }
        public Vector2d Size { get; set; }
        public int FrameCount { get; set; }
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
        public static void Load(Enum sprite, Vector2d size, int frameCount=1 , double depth = 0, double opacity=0)
        {
            var s = new SpriteList();
            var file = s.FindImageInProjectFolder(sprite.ToString());
            s.LoadTexture(file);
            s.SpriteEnum = sprite;
            s.Size = size;
            s.FrameCount = frameCount;
            s.Depth = depth;
            s.Opacity = opacity;
            All.Add(sprite, s);
        }

        public static void Load(Enum sprite, double scaleFromOriginal, int frameCount = 1, double depth = 0, double opacity = 0)
        {
            var s = new SpriteList();
            var file = s.FindImageInProjectFolder(sprite.ToString());
            s.LoadTexture(file);
            s.SpriteEnum = sprite;
            s.Size = s.InitialSize * scaleFromOriginal;
            s.FrameCount = frameCount;
            s.Depth = depth;
            s.Opacity = opacity;
            All.Add(sprite, s);
        }

        public static void Load(Enum sprite, Vector2 scaleFromOriginal, int frameCount = 1, double depth = 0, double opacity = 0)
        {
            var s = new SpriteList();
            var file = s.FindImageInProjectFolder(sprite.ToString());
            s.LoadTexture(file);
            s.SpriteEnum = sprite;
            s.Size = s.InitialSize.MultEach((Vector2d)scaleFromOriginal);
            s.FrameCount = frameCount;
            s.Depth = depth;
            s.Opacity = opacity;
            All.Add(sprite, s);
        }



        string FindImageInProjectFolder(string nameWithoutExtension)
        {
            return ""; //or throw
        }

        void LoadTexture(string file)
        {

           
            InitialSize = new Vector2d(0);
            ScaleToPowerOf2 = new Vector2d(0);
            

            OpenglTexture = 2;
        }
    }
}
