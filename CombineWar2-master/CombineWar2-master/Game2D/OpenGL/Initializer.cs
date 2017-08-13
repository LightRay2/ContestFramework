using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Game2D
{
    class Initializer
    {

        #region public static methods


        public static void SetupViewport(GLControl control)
        {
            //viewport
            double w = Config.ScreenWidth;
            double h = Config.ScreenHeight;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, h, 0, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0,
                 control.Width, control.Height); // Use all of the glControl painting area
        }

        public static Dictionary<string, int> LoadTextures()
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
            foreach (var tex in Config.Sprites)
            {
                Vector2d realSize; //todo что нибудь с этим сделать
                int code = LoadTexture(Config.Sprites[tex.Key].file, out realSize);
                if (code != -1) res.Add(tex.Key, code);
            }
            return res;
        }
        #endregion

        #region private texture load and make
        public static int LoadTexture(string filename, out Vector2d realSize)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Bitmap bmp = LoadAsArgb(filename);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            realSize = new Vector2d(bmp.Width, bmp.Height);

            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            return id;
        }

        static Bitmap LoadAsArgb(string file)
        {
            //todo cast to 1024
            Bitmap orig = new Bitmap(file);
            Bitmap clone = new Bitmap(orig.Width, orig.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (Graphics gr = Graphics.FromImage(clone))
            {
                gr.DrawImage(orig, new Rectangle(0, 0, clone.Width, clone.Height));
            }
            orig.Dispose();

            return clone;
        }


       
        #endregion
    }
}
