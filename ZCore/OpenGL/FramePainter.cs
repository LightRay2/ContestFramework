using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using QuickFont;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Framework
{
    class FramePainter
    {
        static TextManager _textManager = new TextManager();
        public static void DrawFrame(Control control, Frame frame, Dictionary<string, int> spriteCodes)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var rect = frame.cameraViewport;
            GL.Ortho(rect.left, rect.right, rect.bottom, rect.top, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Rotate(frame.cameraRotationDeg, 0, 0, 1);
     

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            
            if (frame.sprites != null)
            {
                foreach (SpriteOld sprite in frame.sprites.OrderBy(x => Config.Sprites[x.name.ToString()].depth))
                {
                    GL.PushMatrix();
                    GL.Translate(sprite.pos.x, sprite.pos.y, 0);
                    GL.Rotate(sprite.pos.angleDeg, 0, 0, 1);
                    DrawTexture(sprite, spriteCodes[sprite.name.ToString()]);
                    GL.PopMatrix();
                }
            }

            if (frame.texts != null)
            {
                foreach (Text text in frame.texts)
                {
                    GL.PushMatrix();
                    // GL.Translated(text.pos.x, text.pos.y, 0);
                    //GL.Rotated(text.pos.angleDeg , 0, 0, 1);
                    foreach (SpriteOld sprite in text.GetSpritesWithRelativePos())
                    {

                        GL.PushMatrix();
                        GL.Translate(sprite.pos.x, sprite.pos.y, 0);
                        GL.Rotate(sprite.pos.angleDeg, 0, 0, 1);
                        DrawTexture(sprite, spriteCodes[sprite.texture]);
                        GL.PopMatrix();

                    }
                    GL.PopMatrix();
                }
            }


            GL.Disable(EnableCap.Texture2D);


            QFont.Begin();
            var fontState = _textManager.LoadOrCheckFont("Arial", 24, FontStyle.Regular, "Привет");
            fontState.QFont.Options.Colour =Color.White;
            fontState.QFont.Print("Привет", (Vector2)new Vector2d(0, 0));
            //  var sizeOnbitmap = fontState.QFont.Measure(text.TextString);
            //  var realSize = new Vector2d(sizeOnbitmap.Width / scale, sizeOnbitmap.Height / scale);
            QFont.End();
            GL.Finish();
           // Glut.glutSwapBuffers();
            double ms = stopwatch.ElapsedMilliseconds;
            double ticks = stopwatch.ElapsedTicks;
        }

        private static void DrawTexture(SpriteOld sprite, int textureCode)
        {
            // if (IsSpriteOutScreen(sprite)) return; наверное опенгл и сам это делает

            int hor = Config.Sprites[sprite.texture].horFrames;
            int vert = Config.Sprites[sprite.texture].vertFrames;

            double horPart = 1d / hor, vertPart = 1d / vert;
            double bottom = 1 - (sprite.frame / hor + 1) * vertPart;
            double top = 1 - sprite.frame / hor * vertPart;
            double right = (sprite.frame % hor + 1) * horPart;
            double left = sprite.frame % hor * horPart;

           // GL.Enable(GL._TEXTURE_2D);
            GL.BindTexture(TextureTarget.Texture2D, textureCode);

            GL.Begin(PrimitiveType.Quads);
            // указываем поочередно вершины и текстурные координаты
            GL.TexCoord2(left, top);
            GL.Vertex2(-sprite.width / 2, -sprite.height / 2);
            GL.TexCoord2(right, top);
            GL.Vertex2(sprite.width / 2, -sprite.height / 2);
            GL.TexCoord2(right, bottom);
            GL.Vertex2(sprite.width / 2, sprite.height / 2);
            GL.TexCoord2(left, bottom);
            GL.Vertex2(-sprite.width / 2, sprite.height / 2);
            GL.End();

        }

    }
}
