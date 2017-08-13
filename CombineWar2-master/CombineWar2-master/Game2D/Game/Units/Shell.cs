using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Game2D.Game.Units
{
    class Shell
    {
        public Point2 StartPosition { get; set; }
        public double Angle { get; set; }
        public OwnerType Owner { get; set; }
        public Unit OwnerUnit { get; set; }
        public int Damage { get; set; }
        public double LaunchStage { get; set; }
        double speedX, speedY;
        public Shell (Point2 start, double angle, OwnerType owner, Unit ownerUnit, double launchStage)
        {
            Owner = owner;
            Angle = Utility.LimitAngle(angle);

            //вставил сюда, т к это неизменяемая величина
            speedX = Const.ProjectileSpeed * Const.TileWidth * Math.Cos(Angle / 180 * Math.PI);
            speedY = Const.ProjectileSpeed * Const.TileHeight * Math.Sin(Angle / 180 * Math.PI);
          
            StartPosition = start;
            OwnerUnit = ownerUnit;
            Damage = ownerUnit.Damage;
            LaunchStage = launchStage;
        }

        public Point2? GetCoordinate (double stage)
        {
            Debug.Assert(Utility.inClosedInterval(stage, 0, 1));
            if (Utility.doubleLess(stage, LaunchStage))
                return null;
             return new Point2(StartPosition.x + (stage - LaunchStage) * speedX,
                              StartPosition.y + (stage - LaunchStage) * speedY);
        }
    }
}
