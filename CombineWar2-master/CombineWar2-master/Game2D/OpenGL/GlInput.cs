using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game2D
{
    //todo инпут при долгом рисовании, по 1 секунде кадр
    public class GlInput
    {
        static Dictionary<Keys, int> keyTime; 
        public static int KeyTime(Keys key){
            if (!keyTime.Keys.Contains(key))
                return 0;
            return keyTime[key];
        }
        /// <summary>
        /// если мышь снаружи, то последняя позиция
        /// </summary>
        public static Vector2d Mouse;
        public static Vector2d MouseRelative;
        public static int _mousePhysicalX, _mousePhysicalY;

        static int _leftMouseTime = -1, _rightMouseTime = -1, _middleMouseTime=-1;
        public static int LeftMouseTime{get{return _leftMouseTime >=0? _leftMouseTime : 0;}}
        public static int RightMouseTime { get { return _rightMouseTime >= 0 ? _rightMouseTime : 0; } }
        public static int MiddleMouseTime { get { return _middleMouseTime >= 0 ? _middleMouseTime : 0; } }
        public static bool MouseInside;
        static bool _setMouseLeftUp, _setMouseRightUp, _setMouseMiddleUp;
        public static bool LeftMouseUp, RightMouseUp, MiddleMouseUp;
        /// <summary>
        /// -1 0 1
        /// </summary>
        public static int Wheel;//todo сильно зависит от фокуса
        static Control _graphicControl;
        private static int _setWheel;

        
        public static void EveryFrameStartRefresh()
        {
            MouseRelative = new Vector2d((double)_mousePhysicalX / _graphicControl.Width,
               (double)_mousePhysicalY / _graphicControl.Height);
            Mouse = GetAbsoluteCoordByRelativeOnScreen(MouseRelative);

            
            keyTime = keyTime.ToDictionary(p => p.Key, v => v.Value + 1);
            if (_leftMouseTime != -1)
                _leftMouseTime++;
            if (_rightMouseTime != -1)
                _rightMouseTime++;
            if (_middleMouseTime != -1)
                _middleMouseTime++;

            LeftMouseUp = _setMouseLeftUp;
            RightMouseUp = _setMouseRightUp;
            MiddleMouseUp = _setMouseMiddleUp;
            _setMouseLeftUp = _setMouseMiddleUp = _setMouseRightUp = false;

            Wheel = _setWheel;
            _setWheel = 0;
        }

        public static Vector2d GetAbsoluteCoordByRelativeOnScreen(Vector2d relative)
        {
            //todo
           // return new Vector2d(relative.X * GlCore.WIDTH / Frame.cameraScale.X + Frame.cameraOrigin.X,
             //    relative.Y * GlCore.HEIGHT / Frame.cameraScale.Y + Frame.cameraOrigin.Y);
            return new Vector2d(relative.X * Config.ScreenWidth,
                 relative.Y * Config.ScreenHeight);

        }

        public static void Init(Form form, Control graphicControl)
        {
            form.KeyPreview = true;
            form.KeyUp += form_KeyUp;
            form.KeyDown += form_KeyDown;
            form.MouseWheel += form_MouseWheel;
            
            _graphicControl = graphicControl;
            _graphicControl.LostFocus += _graphicControl_LostFocus;
            _graphicControl.MouseMove += _graphicControl_MouseMove;
            _graphicControl.MouseLeave += _graphicControl_MouseLeave;
            _graphicControl.MouseDown += _graphicControl_MouseDown;
            _graphicControl.MouseUp += _graphicControl_MouseUp;
            keyTime = new Dictionary<Keys, int>();
        }

        static void form_MouseWheel(object sender, MouseEventArgs e)
        {
            if (MouseInside)
            {
                if (e.Delta > 0)
                    _setWheel = 1;
                else if (e.Delta < 0)
                    _setWheel = -1;
            }
        }


        static void _graphicControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _leftMouseTime = -1;
                _setMouseLeftUp = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                _middleMouseTime = -1;
                _setMouseRightUp = true;
            }
            if (e.Button == MouseButtons.Middle)
            {
                _rightMouseTime = -1;
                _setMouseMiddleUp = true;
            }
        }

        static void _graphicControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _leftMouseTime = 0;
            if (e.Button == MouseButtons.Right)
                _rightMouseTime = 0;
            if (e.Button == MouseButtons.Middle)
                _middleMouseTime = 0;
        }

        
        

        static void _graphicControl_MouseLeave(object sender, EventArgs e)
        {
            MouseInside = false;
        }

        static void _graphicControl_MouseMove(object sender, MouseEventArgs e)
        {
            //todo recalc coord when wheel
            MouseInside = true;
            _mousePhysicalX = e.X;
            _mousePhysicalY = e.Y;
        }

        static void _graphicControl_LostFocus(object sender, EventArgs e)
        {
            keyTime.Clear();
        }

        //todo modifier keys
        static void form_KeyDown(object sender, KeyEventArgs e)
        {
            if (_graphicControl.Focused)
                keyTime.Add(e.KeyCode,0);
        }

        static void form_KeyUp(object sender, KeyEventArgs e)
        {
            if (_graphicControl.Focused)
                keyTime.Remove(e.KeyData);
        }
    }
}
