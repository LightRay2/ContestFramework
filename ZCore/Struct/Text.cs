﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public struct Text
    {
        EFont font;
        public List<string> lines;
        /// <summary>
        /// Позиция центра текста
        /// </summary>
        public Vector2Old pos;
        /// <summary>
        /// ширина и высота всего текста
        /// </summary>
        public double width, height;

        /// <summary>
        /// задается ширина и высота отдельной буквы, верхний левый угол
        /// </summary>
        public Text(EFont font, Point2 loc, double letterWidth, double letterHeight, params string[] lines)
        {
            this.lines = new List<string>(lines);
            int r = 0;
            foreach (string s in lines)
                if (s.Length > r) r = s.Length;
            width = letterWidth * r;
            height = letterHeight * this.lines.Count;
            this.font = font;

            pos = new Vector2Old(loc.x+width/2, loc.y+height/2, 0);
        }

        /// <summary>
        /// Задается позиция центра, ширина и высота всего текста
        /// </summary>
        public Text(EFont font, Vector2Old pos, double textWidth, double textHeight, params string[] lines)
        {
            this.lines = new List<string>(lines);
            this.pos = pos;
            this.height = textHeight;
            this.width = textWidth;
            this.font = font;
        }

        /// <summary>
        /// Чтобы отрисовать, нужно сначала координаты сдвинуть и повернуть на pos
        /// </summary>
        public List<SpriteOld> GetSpritesWithRelativePos()
        {
            List<SpriteOld> res = new List<SpriteOld>();
            double letterWidth = width/maxLineLength();
            double letterHeight = height/lines.Count;

            for(int i = 0; i < lines.Count; i++)
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (Config.FontLetters.Contains(lines[i][j]))
                    {
                        Vector2Old translation = new Vector2Old(0,0,-width / 2 + j * letterWidth,
                            -height / 2 + i * letterHeight);
                        translation.Rotate(this.pos.angleDeg);

                        SpriteOld toAdd = new SpriteOld(ESprite.end, letterWidth, letterHeight,
                            new Vector2Old(pos.x+ translation.vx , pos.y+translation.vy, pos.angleDeg),
                            Config.FontLetters.IndexOf(lines[i][j]));
                        toAdd.texture = font.ToString();
                        /*
                        Sprite toAdd = new Sprite(ESprite.end, letterWidth, letterHeight,
                            new Vector2(-width / 2 + j * letterWidth, -height / 2 + i * letterHeight, 0),
                            Config.FontLetters.IndexOf(lines[i][j]));
                        */

                        res.Add(toAdd);
                    }
                    //иначе будет пустое место - пробел
                }

            return res;
        }

        

        int maxLineLength()
        {
            int r = 0;
            foreach (string s in lines)
                if (s.Length > r) r = s.Length;
            return r;

        }
    }
}
