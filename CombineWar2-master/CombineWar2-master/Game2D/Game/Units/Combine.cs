using Game2D.Game.DrawableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.Units
{
    class Combine : Unit
    {
        public Combine(OwnerType owner, int line, int position, int destination): base (owner, position, line, destination)
        {
            Width = Const.UnitStandartWidth;
            Height = Const.UnitStandartHeight;
            State = UnitState.Forward;
            Type = UnitTypes.Combine;
            MaxHp = 225;
            Hp = MaxHp;
            Cost = 15000;
            RotationRate = 60;
            ThreatRadius = 0;
            DamageRate = 0;
            Damage = 0;
            ESprite hull;
            if (owner == OwnerType.Player1)
                hull = ESprite.combine;
            else
                hull = ESprite.combine2;
            Vector2 ps = new Vector2(Const.FieldOriginX + Const.TileWidth * (position + 0.5),
                                     Const.FieldOriginY + Const.TileHeight * (line + 0.5));
            obj.objs.Add(new DynamicObject(new Sprite(hull, Width, Height, ps)));
        }

    }
}
