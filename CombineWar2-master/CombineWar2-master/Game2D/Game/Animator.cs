using Game2D.Game.DataClasses;
using Game2D.Game.DrawableObjects;
using Game2D.Game.Units;
using Game2D.Opengl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Game2D.Game
{
    class Animator
    {

        public bool IsFinished = false;
        State state = null;
        double stage = 0; // от 0 до 1 - 
        int _activeLogLine = -1;
        int _firstLogLine = -1;
        bool _copied = false;
        //current


        /// <summary>
        /// загружаем в аниматор состояние, которое он будет отрисовывать в течение какого-то времени
        /// </summary>
        /// <param name="state"></param>
        public void Run(State state)
        {
            this.state = state;
            stage = 0;
            IsFinished = false;
        }
        public Frame DrawAll(ref Frame frame, IGetKeyboardState keyboard)
        {
            //тут надо все рисовать
            //примерный порядок: сначала фон, затем все IDrawable объекты, затем тексты
            try
            {
                #region remove sprites of dead units
                for (int n = 0; n < 2; n++)
                {
                    for (int i = 0; i < Const.NumberOfLines; i++)
                    {
                        Unit unit = state.players[n].Units[i];
                        if (unit == null)
                            continue;
                        if (unit.State == UnitState.Dead)
                        {
                            double deathTime = unit.DeathStage;
                            if (Utility.doubleGreaterOrEqual(stage - deathTime, 0.35))
                            {
                                foreach (DynamicObject sprite in unit.obj.objs)
                                {
                                    state.objects.Remove(sprite);
                                }
                                unit.obj.objs.Clear();
                            }
                            if (Utility.doubleEqual(stage, 1.0) && deathTime > 0.0)
                                unit.DeathStage -= 1.0;
                        }
                    }
                }
                #endregion

                frame.Add(new Sprite(ESprite.background, Const.ScreenWidth, Const.ScreenHeight, new Vector2(Const.ScreenWidth / 2, Const.ScreenHeight / 2)));
                var drawable = new List<IDrawable>();
                drawable.AddRange(state.objects);
                drawable.Add(state.effects);

                foreach (var d in drawable)
                    d.Draw(ref frame, stage, state.turn);

                double left = Const.FieldOriginX,
                    right = Const.FieldOriginX + Const.TileWidth * (Const.NumberOfColumns + 2),
                    center = (left + right) / 2;
                string message = state.Message;
                if (message != null)
                {
                    frame.Add(new Text(EFont.orange, new Point2(TextX(message, center), Const.FieldOriginY / 2), Const.smallLetterWidth, Const.smallLetterHeight,
                                   message));
                }
                else
                {
                    ///счет, деньги, номер хода


                    string turn = (Math.Max(0, state.turn - 1)).ToString(),
                        score0 = state.players[0].Score.ToString(),
                        score1 = state.players[1].Score.ToString(),
                        money0 = "-" + (state.players[0].Money - state.players[0].moneySpent).ToString() + "-",
                        money1 = "-" + (state.players[1].Money - state.players[1].moneySpent).ToString() + "-";
                    frame.Add(new Text(EFont.orange, new Point2(TextX(turn, center), Const.FieldOriginY / 2), Const.smallLetterWidth, Const.smallLetterHeight,
                                       turn));
                    frame.Add(new Text(EFont.green, new Point2(TextX(score0, left + Const.TileWidth / 2), Const.FieldOriginY / 2), Const.smallLetterWidth, Const.smallLetterHeight,
                                       score0));
                    frame.Add(new Text(EFont.red, new Point2(TextX(score1, right - Const.TileWidth / 2), Const.FieldOriginY / 2), Const.smallLetterWidth, Const.smallLetterHeight,
                                       score1));
                    frame.Add(new Text(EFont.green, new Point2(TextX(money0, left + Const.TileWidth * 3), Const.FieldOriginY / 2), Const.smallLetterWidth, Const.smallLetterHeight,
                                       money0));
                    frame.Add(new Text(EFont.red, new Point2(TextX(money1, right - Const.TileWidth * 3), Const.FieldOriginY / 2), Const.smallLetterWidth, Const.smallLetterHeight,
                                       money1));
                }


                int logLineCount = 22;
                bool needRectangle = false;
                if (Game.PauseButtonPressed && state.turnCommentList.Count > 0)
                {
                    //управление логом
                    if (keyboard.GetActionTime(EKeyboardAction.Fire) == 1)
                    {
                        _copied = false;
                        _activeLogLine = state.turnCommentList.Count - 1;
                        _firstLogLine = Math.Max(0, state.turnCommentList.Count - logLineCount);
                    }
                    if (keyboard.GetActionTime(EKeyboardAction.up) == 1)
                    {
                        _copied = false;
                        _activeLogLine = Math.Max(0, _activeLogLine - 5);
                        if (_activeLogLine < _firstLogLine)
                            _firstLogLine = _activeLogLine;
                    }
                    if (keyboard.GetActionTime(EKeyboardAction.down) == 1)
                    {
                        _copied = false;
                        _activeLogLine = Math.Min(state.turnCommentList.Count - 1, _activeLogLine + 1);
                        if (_activeLogLine >= _firstLogLine + logLineCount)
                            _firstLogLine++;
                    }
                    if (keyboard.GetActionTime(EKeyboardAction.I) == 1)
                    {
                        Clipboard.SetText(state.inputList[_activeLogLine]);
                        _copied = true;
                    }
                    if (keyboard.GetActionTime(EKeyboardAction.O) == 1)
                    {
                        if (state.players[_activeLogLine % 2].programAddress != null)
                        {
                            Clipboard.SetText(state.outputList[_activeLogLine]);
                            _copied = true;
                        }
                    }

                    needRectangle = true;
                }
                else
                {
                    //показываем лог
                    _firstLogLine = Math.Max(0, state.turnCommentList.Count - logLineCount);

                }

                int showCount = Math.Min(state.turnCommentList.Count, logLineCount);
                for (int i = 0; i < showCount; i++)
                {
                    bool onActive = needRectangle && (_firstLogLine + i == _activeLogLine);
                    string text = String.Format("{0}. {1}", (_firstLogLine + i) / 2, state.turnCommentList[_firstLogLine + i]);
                    if (onActive && _copied)
                        text = "Cкопировано";
                    var font = (_firstLogLine + i) % 2 == 0 ? EFont.green : EFont.red;
                    var pos = new Point2(right + 20, 24 + i * 12);
                    if (onActive)
                        RectangleAroundText(ref frame, pos, text);
                    frame.Add(new Text(font, pos, Const.smallLetterWidth, Const.smallLetterHeight, text));

                }

                //hp
                for (int line = 0; line < Const.NumberOfLines; line++)
                {
                    if (state.players[0].Units[line] != null)
                    {
                        var dict = state.players[0].Units[line].HpDuringTurn;
                        var hp = dict.OrderBy(x => x.Key).Last(x => Utility.doubleGreaterOrEqual(stage, x.Key)).Value;
                        TextInCell(ref frame, EFont.green, Math.Max(0, hp).ToString(), 0, line);
                    }
                    if (state.players[1].Units[line] != null)
                    {
                        var dict = state.players[1].Units[line].HpDuringTurn;
                        var hp = dict.OrderBy(x => x.Key).Last(x => Utility.doubleGreaterOrEqual(stage, x.Key)).Value;
                        TextInCell(ref frame, EFont.red, Math.Max(0, hp).ToString(), Const.NumberOfColumns + 1, line);
                    }
                }

                //int mouseLine, mousePosition;
                //if (TurnReceiver.GetCellUnderMouse(keyboard, out mouseLine, out mousePosition))
                //{
                //    if (state.players[0].Units[mouseLine] != null)
                //    {
                //        TextInCell(ref frame, EFont.green, state.players[0].Units[mouseLine].Hp.ToString(), 0, mouseLine);
                //    }
                //    if (state.players[1].Units[mouseLine] != null)
                //    {
                //        TextInCell(ref frame, EFont.red, state.players[1].Units[mouseLine].Hp.ToString(), Const.NumberOfColumns + 1, mouseLine);
                //    }
                //}



                UpdateStage();

            }
            catch (Exception)
            {

            }
              
            return frame;
        }

        public static double TextX(string text, double center)
        {
            double w = text.Length * Const.smallLetterWidth;
            return center - w / 2 + Const.smallLetterWidth / 2;
        }

        void RectangleAroundText(ref Frame frame, Point2 pos, string text)
        {
            double padding = Const.TileHeight / 8;
            double w = text.Length * Const.smallLetterWidth + padding*2;
            double x = pos.x-  Const.smallLetterWidth / 2 - padding;
            double y = pos.y - Const.smallLetterHeight / 2-padding;
            double h = Const.smallLetterHeight + padding * 2;
            Rectangle(ref frame,  ESprite.gridPoint,x,y,w,h);
            }

        public static void Rectangle(ref Frame frame, ESprite sprite, double x, double y, double w, double h, double alpha=1)
        {
            double pointSz = Const.TileHeight / 15;
            frame.Add(new Sprite(sprite, w, pointSz, new Vector2(x + w / 2, y) ){ Alpha= alpha });
            frame.Add(new Sprite(sprite, pointSz, h, new Vector2(x, y + h / 2)) { Alpha = alpha });
            frame.Add(new Sprite(sprite, w, pointSz, new Vector2(x + w / 2, y + h)) { Alpha = alpha });
            frame.Add(new Sprite(sprite, pointSz, h, new Vector2(x + w, y + h / 2)) { Alpha = alpha });
       
        }

        public static void RectangleAroundCell(ref Frame frame, ESprite sprite, double x, double y)
        {
            double xx = Const.FieldOriginX+x*Const.TileWidth;
            double yy = Const.FieldOriginY + y * Const.TileHeight;
            Rectangle(ref frame, sprite, xx, yy, Const.TileWidth, Const.TileHeight);
        }

        public static void TextInCell(ref Frame frame, EFont font, string text, double x, double y)
        {
            double xx = Const.FieldOriginX + x * Const.TileWidth + Const.TileWidth/2;
            double yy = Const.FieldOriginY + y * Const.TileHeight + Const.TileHeight/2;
            frame.Add(new Text(font, new Point2(TextX(text, xx), yy), Const.smallLetterWidth, Const.smallLetterHeight, text));
        }

        void UpdateStage()
        {
            if (Utility.doubleEqual(stage, 1))
            {
                IsFinished = true;
            }
            stage += 1.0 / Const.FramesPerTurn;
            if (stage > 1) 
                stage = 1;
        }
    }
}
