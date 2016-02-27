using Game2D.Game.DrawableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.Units
{
    class Cannon : Unit
    {
        public Cannon(OwnerType owner, int line, int position, int destination): base(owner, position, line, destination)
        {
            State = UnitState.Forward;
            Type = UnitTypes.Cannon;
            MaxHp = 75;
            Hp = MaxHp;
            Cost = 25000;
            RotationRate = 60;
            ThreatRadius = 6;
            DamageRate = 1;
            Damage = 21;
            ESprite hull;
            ESprite turret;
            Width = Const.UnitStandartWidth;
            Height = Const.UnitStandartHeight;
            double TurretWidth = Const.UnitStandartWidth * 2;
            double TurretHeight = Const.UnitStandartHeight;
            if (owner == OwnerType.Player1)
            {
                hull = ESprite.cannonBox;
                turret = ESprite.cannonGun;
            }
            else
            {
                hull = ESprite.cannonBox2;
                turret = ESprite.cannonGun2;
            }
            Vector2 ps = new Vector2(Const.FieldOriginX + Const.TileWidth * (position + 0.5),
                                     Const.FieldOriginY + Const.TileHeight * (line + 0.5));
            obj.objs.Add(new DynamicObject(new Sprite(hull, Width, Height, ps)));
            obj.objs.Add(new DynamicObject(new Sprite(turret, TurretWidth, TurretHeight, ps)));
        }
    }
}
