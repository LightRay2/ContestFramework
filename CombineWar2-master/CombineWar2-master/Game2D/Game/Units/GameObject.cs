using Game2D.Game.DrawableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.Units
{
    public enum OwnerType { Player1, Player2, Neutral };
    public enum GameObjectType {Stone, Unit, Projectile}
    class GameObject
    {
        public OwnerType Owner { get; set; }
        public GameObjectType ObjectType { get; set; }
        public ComplexDynamicObject obj;
        public int Line { get; set; }
        int posTileX = -1;
        public GameObject(OwnerType owner, GameObjectType type, int line, int position)
        {
            obj = new ComplexDynamicObject();
            Line = line;
            ObjectType = type;
            posTileX = position;
            Owner = owner;
        }


        public int PosTileX
        {
            get 
            { 
                return posTileX; 
            }
            set
            {
                posTileX = value;
            }
        }
    }
}
