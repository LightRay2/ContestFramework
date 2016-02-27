using Game2D.Game.DrawableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.Units
{
    class Armored : Unit
    {
        public Armored(OwnerType owner, int line, int position, int destination): base(owner, position, line, destination)
        {
            State = UnitState.Forward;
            Type = UnitTypes.Armored;
            MaxHp = 125;
            Hp = MaxHp;
            Cost = 15000;
            RotationRate = 60;
            ThreatRadius = 4;
            DamageRate = 4;
            Damage = 3;
            Width = Const.UnitStandartWidth;
            Height = Const.UnitStandartHeight*1.25;
            double TurretWidth = Const.UnitStandartWidth ;
            double TurretHeight = Const.UnitStandartHeight;
            ESprite hull, turret;
            if (owner == OwnerType.Player1)
            {
                hull = ESprite.armoBox;
                turret = ESprite.armoGun;
            }
            else
            {
                hull = ESprite.armoBox2;
                turret = ESprite.armoGun2;
            }
            Vector2 ps = new Vector2(Const.FieldOriginX + Const.TileWidth * (position + 0.5),
                                     Const.FieldOriginY + Const.TileHeight * (line + 0.5));
            obj.objs.Add(new DynamicObject(new Sprite(hull, Width, Height, ps)));
            obj.objs.Add(new DynamicObject(new Sprite(turret, TurretWidth, TurretHeight, ps)));
        }
    }
}
