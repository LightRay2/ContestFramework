using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tao.OpenGl;
using Tao.FreeGlut;
using Tao.Platform.Windows;
using Tao.DevIl;

namespace Framework.Opengl
{
    class Painter
    {

        public static void DrawFrame(Frame frame, Dictionary<string, int> spriteCodes)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glEnable(Gl.GL_TEXTURE_2D);

            Gl.glLoadIdentity();
            Gl.glTranslated(-frame.camera.x, -frame.camera.y, 0);
            Gl.glRotated(-frame.camera.angleDeg, 0, 0,1);
            if (frame.sprites != null)
            {
                foreach (SpriteOld sprite in frame.sprites.OrderBy(x=>Config.Sprites[x.name.ToString()].depth))
                {
                    Gl.glPushMatrix();
                    Gl.glTranslated(sprite.pos.x, sprite.pos.y, 0);
                    Gl.glRotated(sprite.pos.angleDeg, 0, 0, 1);
                    DrawTexture(sprite, spriteCodes[sprite.name.ToString()]);
                    Gl.glPopMatrix();
                }
            }

            if (frame.texts != null)
            {
                foreach (Text text in frame.texts)
                {
                    Gl.glPushMatrix();
                   // Gl.glTranslated(text.pos.x, text.pos.y, 0);
                    //Gl.glRotated(text.pos.angleDeg , 0, 0, 1);
                    foreach (SpriteOld sprite in text.GetSpritesWithRelativePos())
                    {

                        Gl.glPushMatrix();
                        Gl.glTranslated(sprite.pos.x, sprite.pos.y, 0);
                        Gl.glRotated(sprite.pos.angleDeg, 0, 0, 1);
                        DrawTexture(sprite, spriteCodes[sprite.texture]);
                        Gl.glPopMatrix();
                        
                    }
                    Gl.glPopMatrix();
                }
            }
            Gl.glDisable(Gl.GL_TEXTURE_2D);

            Gl.glFinish();
            Glut.glutSwapBuffers();
        }

        private static void DrawTexture(SpriteOld sprite, int textureCode)
        {
           // if (IsSpriteOutScreen(sprite)) return; наверное опенгл и сам это делает

            int hor = Config.Sprites[sprite.texture].horFrames;
            int vert = Config.Sprites[sprite.texture].vertFrames;

            double horPart = 1d/hor, vertPart = 1d/ vert; 
            double bottom = 1- (sprite.frame / hor+1) * vertPart;
            double top = 1- sprite.frame / hor  * vertPart;
            double right= (sprite.frame % hor+1) * horPart;
            double left = sprite.frame % hor  * horPart;

            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, textureCode);

            Gl.glBegin(Gl.GL_QUADS);
                // указываем поочередно вершины и текстурные координаты
                Gl.glTexCoord2d(left, top);
                Gl.glVertex2d(-sprite.width/2, -sprite.height/2);
                Gl.glTexCoord2d(right, top);
                Gl.glVertex2d(sprite.width / 2, -sprite.height / 2);
                Gl.glTexCoord2d(right, bottom);
                Gl.glVertex2d(sprite.width / 2, sprite.height / 2);
                Gl.glTexCoord2d(left, bottom);
                Gl.glVertex2d(-sprite.width / 2, sprite.height / 2);
            Gl.glEnd();

        }

        static bool IsSpriteOutScreen(SpriteOld sprite)
        {
            double radius = Math.Sqrt(sprite.width*sprite.width + sprite.height * sprite.height) / 2;

            return (sprite.pos.x + radius < 0 || sprite.pos.y + radius < 0 ||
                sprite.pos.x - radius > Config.ScreenWidth || sprite.pos.y - radius > Config.ScreenHeight);
            
        }
    }
}
