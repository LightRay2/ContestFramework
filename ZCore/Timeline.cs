using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Framework
{
    public enum EDirections { down, right }
    public class Timeline
    {
        private Enum _font;
        bool _followLastTurn = true;
        bool _thereWasADraw = false;
        public Timeline(Enum font, int tileWidth, int tileHeight)
        {
            _font = font;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            TIMELINE_SPEED_DECREASE_PER_TICK = _tileHeight / 30;
        }
        public void Draw(Frame frame, GlInput input, List<ITimelineCell> turns, double firstTurnOffset, int indexOfLastViewedTile,
            Vector2d positionOfFirstTile, double maxLength, double tileWidth, double tileLength, double statusNearTileWidth, EDirections direction)
        {
            #region implementation
            //todo all variables named for down direction and only down is implemented

            //draw last two
            var timeLineX = positionOfFirstTile.X;
            {

                var lastPos = positionOfFirstTile.Y + maxLength - tileLength;
                //todo add rect instead

                var rect = new Rect2d(timeLineX, lastPos, tileWidth, tileLength);
                var statusRect = new Rect2d(timeLineX - statusNearTileWidth, lastPos, statusNearTileWidth, tileLength);

                {
                    var lastTurn = turns.Last();
                    string buttonName = string.Format("__timeline{0}", turns.Count - 1);
                    var statusColor = input.ButtonUnderMouse(buttonName) ? InvertedColor(lastTurn.colorStatusOnTimeLine) : lastTurn.colorStatusOnTimeLine;//todo input
                    frame.Polygon(lastTurn.colorOnTimeLine, rect);
                    frame.Polygon(statusColor, statusRect);
                    frame.TextCenter(_font, lastTurn.nameOnTimeLine, rect.center);
                    input.Button(rect, buttonName);
                }
                {
                    var lastViewedTurn = turns[indexOfLastViewedTile];
                    string buttonName = string.Format("__timeline{0}", indexOfLastViewedTile);
                    var statusColor = input.ButtonUnderMouse(buttonName) ? InvertedColor(lastViewedTurn.colorStatusOnTimeLine) : lastViewedTurn.colorStatusOnTimeLine;//todo input
                    rect = rect - Vector2d.UnitY * tileLength * 1.5;
                    statusRect = statusRect - Vector2d.UnitY * tileLength * 1.5;
                    frame.Polygon(lastViewedTurn.colorOnTimeLine, rect);
                    frame.Polygon(statusColor, statusRect);
                    frame.TextCenter(_font,  lastViewedTurn.nameOnTimeLine, rect.center);
                    input.Button(rect, buttonName);
                }

                maxLength -= 3 * tileLength; //todo закрыть неровности кнопками вверх вниз
                //todo брать ходы из другого места
            }
            //draw first 
            {
                for (int i = 0; i < turns.Count; i++)
                {
                    var turn = turns[i];
                    var rect = new Rect2d(timeLineX, firstTurnOffset + i * tileLength, tileWidth, tileLength);

                    if (rect.bottom <= positionOfFirstTile.Y || rect.bottom > positionOfFirstTile.Y + maxLength)
                        continue;

                    var statusRect = new Rect2d(rect.lefttop - Vector2d.UnitX * statusNearTileWidth, statusNearTileWidth, tileLength);
                    string buttonName = string.Format("__timeline{0}", i);
                    var statusColor = input.ButtonUnderMouse(buttonName) ? InvertedColor(turn.colorStatusOnTimeLine) : turn.colorStatusOnTimeLine;//todo input
                    frame.Polygon(turn.colorOnTimeLine, rect);
                    frame.Polygon(statusColor, statusRect);
                    frame.TextCenter(_font, turn.nameOnTimeLine, rect.center);
                    input.Button(rect, buttonName);
                }
            }
            #endregion

        }



        public double _firstTurnOffset = 0;
        public int _indexOfLastViewedTile = -1;
        public double _tileWidth ;
        public double _tileHeight ;
        public double _timelineSpeed = 0;
        public double TIMELINE_SPEED_DECREASE_PER_TICK = 1;
        public int ManageTimelineByInputAndGetClickedTurn(Frame frame, GlInput input, Rect2d fullTimelineRect, int turnCount)
        {
            if (input.RightMouseTime >= 1 && GeomHelper.PointInSimpleRect(input.Mouse, fullTimelineRect))
            {
                _timelineSpeed = 0;
                if (input.RightMouseTime > 1)
                {
                    _firstTurnOffset += input.MouseDelta.Y;
                }
            }
            else if (input.RightMouseUp)
            {
                _timelineSpeed = input.MouseDelta.Y;
            }


            if (input.Wheel != 0 && GeomHelper.PointInSimpleRect(input.Mouse, fullTimelineRect))
            {
                _timelineSpeed = 0;
                _firstTurnOffset += input.Wheel * _tileWidth;
            }

            _firstTurnOffset += _timelineSpeed;
            if (_timelineSpeed > 0)
            {
                _timelineSpeed = Math.Max(0, _timelineSpeed - TIMELINE_SPEED_DECREASE_PER_TICK);
            }
            else
            {
                _timelineSpeed = Math.Min(0, _timelineSpeed + TIMELINE_SPEED_DECREASE_PER_TICK);

            }

            //correction  - must be <=0 and at least one tile should be visible
            if(_thereWasADraw == false)
            {
                _firstTurnOffset = -10000000;
                _thereWasADraw = true;
            }
            _firstTurnOffset = _firstTurnOffset.ToRange(-turnCount * _tileHeight + _tileHeight*10, 0); //todo now only ten are visible

            var clickedTurn = input.AllClickedButtons().FirstOrDefault(x => x.StartsWith("__timeline"));
            if(clickedTurn != null)
            {
                int turnIndex = int.Parse(clickedTurn.Replace("__timeline", ""));
                return turnIndex;
            }
            return -1;
        }

        Color InvertedColor(Color color)
        {
            //not perfect , from stackoverflow, just changes the color
            Color invertedColor = Color.FromArgb(color.ToArgb() ^ 0xffffff);

            if (invertedColor.R > 110 && invertedColor.R < 150 &&
                invertedColor.G > 110 && invertedColor.G < 150 &&
                invertedColor.B > 110 && invertedColor.B < 150)
            {
                int avg = (invertedColor.R + invertedColor.G + invertedColor.B) / 3;
                avg = avg > 128 ? 200 : 60;
                invertedColor = Color.FromArgb(avg, avg, avg);
            }
            return invertedColor;
        }
    }
}
