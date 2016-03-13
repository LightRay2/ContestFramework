using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Framework
{
    class FramePainter
    {
        public static void DrawFrame(Frame frame, Dictionary<string, int> spriteCodes)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Translate(-frame.camera.x, -frame.camera.y, 0);
            GL.Rotate(-frame.camera.angleDeg, 0, 0, 1);
            if (frame.sprites != null)
            {
                foreach (Sprite sprite in frame.sprites.OrderBy(x => Config.Sprites[x.name.ToString()].depth))
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
                    foreach (Sprite sprite in text.GetSpritesWithRelativePos())
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

            GL.Finish();
           // Glut.glutSwapBuffers();
        }

        private static void DrawTexture(Sprite sprite, int textureCode)
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
