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
        private static double brightenFactor = 1;
        /// <summary>
        /// 1.0 - 4.0
        /// </summary>
        public static double BrightenFactor
        {
            get { return brightenFactor; }
            set { brightenFactor = Math.Min(4, Math.Max(1, value)); }
        }
        public static void DrawFrame(Control control, Frame frame, Dictionary<string, int> spriteCodes)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //camera
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var rect = frame.cameraViewport;
            GL.Ortho(rect.left, rect.right, rect.bottom, rect.top, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Rotate(frame.cameraRotationDeg, 0, 0, 1);
            //--

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //тут отвечаем только за глубину
            var everything = new List<Tuple<double, int, int>>();
            for (int i = 0; i < frame.spriteList.Count; i++ )
            {
                var x = frame.spriteList[i];
                everything.Add(
                    Tuple.Create(x.Item5 != null && x.Item5.depth != null ? x.Item5.depth.Value : SpriteList.All[x.Item1].Depth,
                 0, i));
            }
            for(int i =0; i < frame.textList.Count ; i++){
                var x = frame.textList[i];
                everything.Add(
                Tuple.Create(x.Item7 ?? FontList.All[x.Item1].depth,
                 1, i));
            }

            everything.Sort((a, b) =>
            {
                if (a.Item1 == b.Item1)
                {
                    if (a.Item2 == b.Item2)
                        return a.Item3.CompareTo(b.Item3);
                    else
                        return a.Item2.CompareTo(b.Item2);
                }
                else
                    return a.Item1.CompareTo(b.Item1);
            });
            foreach (var item in everything)
            {
                if (item.Item2 == 0)
                    DrawSprite(frame, item.Item3);
                else if (item.Item2 == 1)
                    DrawText(frame, item.Item3);
            }

            GL.Finish();

            double ms = stopwatch.ElapsedMilliseconds;
            double ticks = stopwatch.ElapsedTicks;
        }

        static void DrawSprite(Frame frame, int index)
        {
            //prepare
            var sprite = frame.spriteList[index];
            if(SpriteList.All.ContainsKey(sprite.Item1) == false)
                throw new Exception("Спрайт с именем "+sprite.Item1.ToString() + " не добавлен при инициализации");
            var settings = SpriteList.All[sprite.Item1];
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, settings.OpenglTexture);
            
            //opacity
            {
                var opacity = sprite.Item5 == null || sprite.Item5.opacity == null? settings.Opacity : sprite.Item5.opacity.Value;
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GL.Color4(sprite.Item5.opacity ?? 1,sprite.Item5.opacity ?? 1,sprite.Item5.opacity ?? 1,sprite.Item5.opacity ?? 1);
            }
            //go to position
            {
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix(); 
                GL.Translate(new Vector3d(sprite.Item3));
                GL.Rotate(sprite.Item4, 0, 0, 1);
            }

            //select texture part by frame and draw it
            {
                var frameNumber = sprite.Item5 == null || sprite.Item5.frame == null ? 0 : sprite.Item5.frame.Value;
                if(frameNumber < 0)
                    frameNumber = 0;
                if(frameNumber > settings.FrameCountVertical * settings.FrameCountHorizontal - 1)
                    frameNumber = settings.FrameCountHorizontal*settings.FrameCountVertical -1;
                 int hor = settings.FrameCountHorizontal;
                int vert = settings.FrameCountVertical;

                double horPart = 1d / hor, vertPart = 1d / vert;
                double bottom = 1 - (frameNumber / hor + 1) * vertPart;
                double top = 1 - frameNumber / hor * vertPart;
                double right = (frameNumber % hor + 1) * horPart;
                double left = frameNumber % hor * horPart;

                Vector2d? size = null;
                if(size == null && sprite.Item5 != null && sprite.Item5.size != null)
                    size = sprite.Item5.size;
                if(size == null && settings.Size != null)
                    size = settings.Size;
                if(size == null && settings.ScaleFromOriginal != null)
                    size = settings.InitialSize.MultEach(settings.ScaleFromOriginal.Value);
                if(size == null)
                    size = settings.InitialSize;

                //здесь размер точно не null
                if(size == null && sprite.Item5 != null && sprite.Item5.scaleSize != null)
                    size = size.Value.MultEach(sprite.Item5.scaleSize.Value);
                
                var topLeft = -size.Value.MultEach(sprite.Item2);
                 GL.Begin(PrimitiveType.Quads);
                // указываем поочередно вершины и текстурные координаты
                GL.TexCoord2(left, top);
                GL.Vertex2(topLeft);
                GL.TexCoord2(right, top);
                GL.Vertex2(topLeft.X + size.Value.X, topLeft.Y);
                GL.TexCoord2(right, bottom);
                GL.Vertex2(topLeft.X + size.Value.X, topLeft.Y + size.Value.Y);
                GL.TexCoord2(left, bottom);
                GL.Vertex2(topLeft.X, topLeft.Y + size.Value.Y);
                GL.End();

                //draw rect to brighten sprite

                if (brightenFactor.Equal(1) == false)
                {
                    double bright = Math.Min(2, brightenFactor) - 1;
                    GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.One);
                    GL.Color4(ColorByBright(bright));
                    GL.Begin(PrimitiveType.Quads);
                    // указываем поочередно вершины и текстурные координаты
                    GL.Vertex2(topLeft);
                    GL.Vertex2(topLeft.X + size.Value.X, topLeft.Y);
                    GL.Vertex2(topLeft.X + size.Value.X, topLeft.Y + size.Value.Y);
                    GL.Vertex2(topLeft.X, topLeft.Y + size.Value.Y);
                    GL.End();

                    if (brightenFactor > 2)
                    {

                        GL.Color4(ColorByBright((BrightenFactor - 2) / 2));
                        GL.Begin(PrimitiveType.Quads);
                        // указываем поочередно вершины и текстурные координаты
                        GL.Vertex2(topLeft);
                        GL.Vertex2(topLeft.X + size.Value.X, topLeft.Y);
                        GL.Vertex2(topLeft.X + size.Value.X, topLeft.Y + size.Value.Y);
                        GL.Vertex2(topLeft.X, topLeft.Y + size.Value.Y);
                        GL.End();
                    }
                }
            }



            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);

        }

        static void DrawText(Frame frame, int index)
        {
            QFont.Begin();
            var text = frame.textList[index];
            var font = FontList.All[text.Item1];
            var fontState = _textManager.LoadOrCheckFont(font.fontFamily, (float)font.emSize, font.fontStyle, text.Item2);
            fontState.QFont.Options.Colour = font.color;
                
            float maxWidth = text.Item6 == null ? float.MaxValue : (float)text.Item6.Value;

            var sizeOnbitmap = fontState.QFont.Measure(text.Item2, maxWidth, text.Item5);
            var realSize =(new Vector2d(sizeOnbitmap.Width, sizeOnbitmap.Height));// rect.size
                //.DivEach(new Vector2d(control.Width, control.Height))
                //.MultEach(new Vector2d(sizeOnbitmap.Width, sizeOnbitmap.Height));

            //  if (maxWidth != null)
            //      maxWidth *= (rect.size.X / control.Width);
            fontState.QFont.Print(text.Item2, maxWidth, text.Item5, (Vector2)(text.Item4 - text.Item3.MultEach(realSize)));
            QFont.End();
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

        static Color ColorByBright(double e)
        {
            int x = 1 + (int)(253 * e);
            return Color.FromArgb(255, x, x, x);
        }

    }
}
