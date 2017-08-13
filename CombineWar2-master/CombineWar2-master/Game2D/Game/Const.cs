using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game
{
    class Const 
    {

        public static List<ParamsFromMainFormToGame> ParamsFromMainFormToGame;

        public static int DefaultFramesPerTurn = 50;
        public static int FramesPerTurn = 50;
        public static readonly int NumberOfLines = 12;
        public static readonly int NumberOfColumns = 12; 
        public static readonly double ScreenWidth = Config.ScreenWidth;
        public static readonly double ScreenHeight = Config.ScreenHeight;
        public static readonly double TileHeight = (ScreenHeight - 40) / NumberOfLines;
        public static readonly double TileWidth = TileHeight;
        public static readonly double FieldWidth = TileWidth * NumberOfColumns;
        public static readonly double FieldHeight = TileHeight * NumberOfLines;
        public static readonly double FieldOriginX = 10;
        public static readonly double FieldOriginY = 20;

        //misha
        public static double UnitStandartWidth = TileWidth * 0.8;
        public static double UnitStandartHeight = UnitStandartWidth * 0.6;
        public static double UnitExplosionSize = UnitStandartWidth * 3 / 2; 
        //-----

        public static double ProjectileSpeed = 5; 
        public static readonly int MoneyPerTurn = 2500;
        public static readonly int StartMoney = 100000; //чтобы участники при знакомстве с программой сразу могли купить юнитов

        public static readonly double smallLetterWidth = 5, smallLetterHeight = 8;

        public static readonly int MaxStones = NumberOfLines;
        //public static readonly int StoneTime = 10; isn't used
        public static readonly int StoneScore = 1;
        public static readonly int StoneRadius = 1;
        public static readonly double StoneSpawnProbabilityPerTurn = 0.3;

        public static readonly int RepairRate = 10;

        public static readonly int NumberOfTurns = 200;

        public static double ShellRadius = 0.001;//снаряд - это точка

        public static bool EveryoneCanTakeStone = true;
    }
}
