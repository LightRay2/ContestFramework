using Game2D.Game.DrawableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.Units
{
    class Tank : Unit
    {
        public Tank(OwnerType owner, int line, int position, int destination) : base (owner, position, line, destination)
        {
            State = UnitState.Forward;
            Type = UnitTypes.Tank;
            MaxHp = 100;
            Hp = MaxHp;
            Cost = 20000;
            RotationRate = 60;
            ThreatRadius = 5;
            DamageRate = 2;
            Damage = 9;
            Width = Const.UnitStandartWidth;
            Height = Const.UnitStandartHeight ;
            double TurretWidth = Const.UnitStandartWidth*2;
            double TurretHeight = Const.UnitStandartHeight;
            ESprite hull, turret;
            if (owner == OwnerType.Player1)
            {
                hull = ESprite.tankBox;
                turret = ESprite.tankGun;
            }
            else
            {
                hull = ESprite.tankBox2;
                turret = ESprite.tankGun2;
            }
            Vector2 ps = new Vector2(Const.FieldOriginX + Const.TileWidth * (position + 0.5),
                                     Const.FieldOriginY + Const.TileHeight * (line + 0.5));
            obj.objs.Add(new DynamicObject(new Sprite(hull, Width, Height, ps)));
            obj.objs.Add(new DynamicObject(new Sprite(turret, TurretWidth, TurretHeight, ps)));
        }
    }
}
