using Game2D.Game.DrawableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.Units
{
    class Stone : GameObject
    {
        public Stone (OwnerType owner, int position, int line) : base(owner, GameObjectType.Stone, line, position)
        {
            Vector2 ps = new Vector2(Const.FieldOriginX + Const.TileWidth * (position + 0.5),
                                     Const.FieldOriginY + Const.TileHeight * (line + 0.5));
            obj.objs.Add(new DynamicObject(new Sprite(ESprite.stone,
                                                      Const.TileWidth * 0.5,
                                                      Const.TileHeight * 0.5, ps)));
            obj.objs[0].AddAnimation(0, 0, 0, 1, permanent: true);
        }
    }
}
