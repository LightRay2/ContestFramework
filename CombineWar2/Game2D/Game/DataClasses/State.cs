using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Game2D.Game.DataClasses
{
    using DrawableObjects;
    using DrawableObjects.Effects;
    using Game2D.Game.Units;
    class State
    {
        public int inEngineCount = 0;
        public int inStoneProcessCount = 0;
        public int inStoneContinueCount = 0;
        public List<List<int>> stonePositionsX = new List<List<int>>();
        public List<List<int>> stonePositionsY = new List<List<int>>();
        public string stones
        {
            get
            {
                return string.Join(" ", stonePositionsX.SelectMany(x => x).Select(x=>x.ToString()))+
                    "\n\n" + string.Join(" ", stonePositionsY.SelectMany(x => x).Select(x=>x.ToString()));
            }
        }

        public int turn = 0;
        public bool IsFinished = false;

        public EffectsManager effects = new EffectsManager();
        public List<DynamicObject> objects = new List<DynamicObject>();
        public List<Player> players = new List<Player>();

        /* if equals to -1 there no stones in that line 
           if equals to -2 combine collected stone in that line*/
        public int[] Stones = new int[Const.NumberOfLines];

        public List<Shell> shells = new List<Shell>();

        public int stoneCount = 0;

        public Units.GameObject[,] field = new Units.GameObject[Const.NumberOfColumns + 2, Const.NumberOfLines];

        /*public Player ActivePlayer { get { return players[turn % 2]; } }
        public Player EnemyPlayer { get { return players[(turn + 1) % 2]; } }*/


        //turn comments
        public List<string> turnCommentList = new List<string>(),
            inputList = new List<string>(),
            outputList = new List<string>();

        int messageTime = 0;
        string _message;
        public string Message
        {
            get { messageTime--;  return messageTime > 0 ? (_message ?? "") : null; }
            set { _message = value; messageTime = 2000 / Config.TimePerFrame; }
        }

        /*public bool IsSecondPlayerTurn()
        {
            return turn % 2 != 0;
        }*/

        public State()
        {
            //тут надо создать изначальное состояние(перед первым ходом)
            //его особенность - оно должно быть наподвижным, т.е. чтоб не происходило никакой анимации
            players.Add(new Player());
            players[0].Owner = OwnerType.Player1;
            players.Add(new Player());
            players[1].Owner = OwnerType.Player2;
            foreach (Player player in players)
            {
                player.Money = Const.StartMoney;
                player.Score = 0;
            }
            for (int i = 0; i < Const.NumberOfLines; i++)
                Stones[i] = -1;

            //сетка
            double w = Const.TileWidth * (Const.NumberOfColumns + 2);
            double h = Const.TileHeight * Const.NumberOfLines;
            double pointSz = Const.TileHeight / 12;
            objects.Add(new DynamicObject(new Sprite(ESprite.gridPoint, w, pointSz,
                                                 new Vector2(Const.FieldOriginX + w / 2, Const.FieldOriginY))));
            objects.Add(new DynamicObject(new Sprite(ESprite.gridPoint, w, pointSz,
                                                   new Vector2(Const.FieldOriginX + w / 2, Const.FieldOriginY + h))));

            objects.Add(new DynamicObject(new Sprite(ESprite.gridPoint1, pointSz, h,
                                                   new Vector2(Const.FieldOriginX, Const.FieldOriginY + h / 2))));
            objects.Add(new DynamicObject(new Sprite(ESprite.gridPoint1, pointSz, h,
                                                   new Vector2(Const.FieldOriginX + Const.TileWidth, Const.FieldOriginY + h / 2))));
            objects.Add(new DynamicObject(new Sprite(ESprite.gridPoint2, pointSz, h,
                                                   new Vector2(Const.FieldOriginX + w, Const.FieldOriginY + h / 2))));
            objects.Add(new DynamicObject(new Sprite(ESprite.gridPoint2, pointSz, h,
                                                   new Vector2(Const.FieldOriginX + w - Const.TileWidth, Const.FieldOriginY + h / 2))));
            for (int line = 1; line < Const.NumberOfLines; line++)
            {
                for (int column = 2; column <= Const.NumberOfColumns; column++)
                {
                    double x = Const.FieldOriginX + Const.TileWidth * column;
                    double y = Const.FieldOriginY + Const.TileHeight * line;
                    objects.Add(new DynamicObject(new Sprite(ESprite.gridPoint, pointSz, pointSz,
                                                 new Vector2(x, y))));
                }
            }
            //старая сетка закомментирована
            //double x = Const.FieldOriginX + Const.TileWidth / 2; 
            //double y = Const.FieldOriginY + Const.TileHeight / 2;
            //for (int i = 0; i < Const.NumberOfLines; i++)
            //{
            //    x = Const.FieldOriginX + Const.TileWidth / 2 * 3; 
            //    for (int j = 0; j < Const.NumberOfColumns; j++)
            //    {
            //        objects.Add(new DynamicObject(new Sprite(ESprite.background_cell, Const.TileWidth, Const.TileHeight,
            //                                     new Vector2(x, y))));
            //        x += Const.TileWidth;

            //    }
            //    y += Const.TileHeight;
            //}
        }



        public bool TryAddStone(int y, int x) //try to add stone and return if we succeeded
        {
            Debug.Assert(y >= 0 && y < Const.NumberOfLines);
            Debug.Assert(x >= 1 && x <= Const.NumberOfColumns);
            if (Stones[y] != -1)
                return false;
            Stones[y] = x;
            stoneCount++;
            Stone st = new Stone(OwnerType.Neutral, x, y);
            field[x, y] = st;
            objects.Add(st.obj.objs[0]);
            return true;
        }

        public void DeleteStone(int y, int x)
        {
            Debug.Assert(Stones[y] != -1);
            Stone st = (Stone)field[x, y];
            objects.Remove(st.obj.objs[0]);
            Stones[y] = -1;
            field[x, y] = null;
            stoneCount--;
        }

        public void AddUnit(int x, int y, Player player, Unit unit)
        {
            //field[x, y] = unit;
            player.Units[y] = unit;
            objects.AddRange(unit.obj.objs);
        }

        public void DestroyUnit(Unit unit)
        {
            int y = unit.Line;
            Player player = unit.Owner == OwnerType.Player1 ? players[0] : players[1];
            player.Units[y] = null;
            foreach (DynamicObject obj in unit.obj.objs)
            {
                objects.Remove(obj);
            }
            if ((unit.Type == UnitTypes.Combine || Const.EveryoneCanTakeStone) && 
                unit.WithStone == true)
            {
                Stones[y] = -1;
                int x;
                if (!unit.AtBase())
                    x = unit.PosTileX;
                else
                {
                    if (unit.Owner == OwnerType.Player1)
                        x = unit.PosTileX + 1;
                    else
                        x = unit.PosTileX - 1;
                }
                TryAddStone(y, x);
                stoneCount--;
            }
        }
    }
}
