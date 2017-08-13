using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Game2D.Game.DrawableObjects;

namespace Game2D.Game.Units
{
    class Mine : Unit
    {
        public int ExplosionRadius = 5;
        public Mine(OwnerType owner, int line, int position, int destination): base(owner, position, line, destination)
        {
            Width = Const.UnitStandartWidth;
            Height = Const.UnitStandartHeight;
            State = UnitState.Forward;
            Type = UnitTypes.Mine;
            MaxHp = 125;
            Hp = MaxHp;
            Cost = 30000;
            RotationRate = 0;
            ThreatRadius = 0; 
            DamageRate = 0;
            Damage = 95;
            ESprite hull;
            if (owner == OwnerType.Player1)
                hull = ESprite.car;
            else
                hull = ESprite.car2;
            Vector2 ps = new Vector2(Const.FieldOriginX + Const.TileWidth * (position + 0.5),
                                     Const.FieldOriginY + Const.TileHeight * (line + 0.5));
            obj.objs.Add(new DynamicObject(new Sprite(hull, Width, Height, ps)));
        }
    }
}
