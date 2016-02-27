using Game2D.Game.DrawableObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Game2D.Game.Units
{
    public enum UnitTypes { Combine = 1, Armored, Tank, Cannon, Mine };
    public enum UnitState { Ready = 1, Repair, Forward, Backward, Dead };

    public struct HitBoxType
    {
        public double Height { get; set; }
        public double Width { get; set; }
    }

    abstract class Unit : GameObject
    {
        public UnitTypes Type { get; set; }
        UnitState state;
        public UnitState State 
        { 
            get
            {
                return state;
            }
            set
            {
                Debug.Assert(state != UnitState.Dead);
                if (value == UnitState.Ready || value == UnitState.Repair)
                    Debug.Assert(AtBase());
                state = value;
            }
        }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Cost { get; set; }
        public double RotationRate { get; set; } // in degrees per turn
        public double CurrAngle { get; set; }
        public int ThreatRadius { get; set; } // in tiles
        public bool FireEnabled { get; set; }
        public double DamageRate { get; set; }
        public double StageOfLastShot { get; set; }
        public int Damage { get; set; }
        public int Destination { get; set; }
        public double DeathStage { get; set; }
        double _width, _height;
        public double Width { get { return _width; } set { _width = HitBox.Width = value; } }
        public double Height { get { return _height; } set { _height = HitBox.Height = value; } }
        public HitBoxType HitBox = new HitBoxType();
        bool active = false;
        public bool Active
        {
            set
            {
                Debug.Assert(state != UnitState.Dead);
                if (state == UnitState.Dead)
                    return;
                if (value == true)
                {
                    if (active == true)
                        return;
                    DynamicObject activeAnim;
                    if (Owner == OwnerType.Player1)
                        activeAnim = new DynamicObject(new Sprite(ESprite.active1, Const.UnitExplosionSize, 
                                                              Const.UnitExplosionSize, 
                                                              new Vector2(obj.objs[0].Pos.x,
                                                                          obj.objs[0].Pos.y)));
                    else
                        activeAnim = new DynamicObject(new Sprite(ESprite.active2, Const.UnitExplosionSize, 
                                                              Const.UnitExplosionSize, 
                                                              new Vector2(obj.objs[0].Pos.x,
                                                                          obj.objs[0].Pos.y)));
                    obj.objs.Add(activeAnim);
                    activeAnim.AddFramingAnimation(1, permanent: true);
                    active = true;
                }
                else
                {
                    if (active == false)
                        return;
                    active = false;
                    obj.objs.Remove(obj.objs.Last());
                }
            }
            get
            {
                return active;
            }
        }
        public bool WithStone { get; set; }
        public Dictionary<double, int> HpDuringTurn = new Dictionary<double,int>();


        public Unit(OwnerType owner, int position, int line, int destination)
            : base(owner, GameObjectType.Unit, line, position)
        {
            Destination = destination;
            State = UnitState.Forward;
            FireEnabled = false;
            HitBox.Height = Const.TileHeight / 2;
            HitBox.Width = Const.TileWidth * 3 / 4;
            StageOfLastShot = -1;
            WithStone = false;
           // if (Owner == OwnerType.Player1)
                CurrAngle = 0;
            //else
            //   CurrAngle = 180;
        }

        public void AddMove(double x, double y, double duration, double latency = 0)
        {
            for (int i = 0; i < obj.objs.Count; i++)
            {
                obj.objs[i].AddMovingAnimation(x, y, duration, latency);
            }
        }

        public void AddTurretRotate(double angle, double duration, double latency = 0)
        {
            if (obj.objs.Count > 1)
            {
                obj.objs[1].AddRotatingAnimation(angle, duration, latency);
            }
        }

        public void AddHullRotate(double angle, double duration, double latency = 0)
        {
            obj.objs[0].AddRotatingAnimation(angle, duration, latency);
        }

        public void AddGunRotate(double angle, double duration, double latency = 0)
        {
            AddTurretRotate(angle, duration, latency);
        }

        public void CancelMove()
        {
            for (int i = 0; i < obj.objs.Count; i++)
                obj.objs[i].ClearMovingAnimationQueue();
            if ((Owner == OwnerType.Player1 && State == UnitState.Forward) ||
                (Owner == OwnerType.Player2 && State == UnitState.Backward))
                PosTileX--;
            else
                PosTileX++;
        }

        public void CancelRotate()
        {
            if (obj.objs.Count > 1)
                obj.objs[1].ClearRotatingAnimationQueue();
        }

        public void MoveForward()
        {
            State = UnitState.Forward;
            if (Owner == OwnerType.Player1)
            {
                AddMove(Const.TileWidth, 0, 1.0);
                PosTileX++;
            }
            else
            {
                AddMove(-Const.TileWidth, 0, 1.0);
                PosTileX--;
            }
        }

        public void MoveBackward()
        {
            State = UnitState.Backward;
            if (Owner == OwnerType.Player2)
            {
                AddMove(Const.TileWidth, 0, 1.0);
                PosTileX++;
            }
            else
            {
                AddMove(-Const.TileWidth, 0, 1.0);
                PosTileX--;
            }
        }

        public bool AtDestination()
        {
            return PosTileX == Destination;
        }

        public bool AtBase()
        {
            return PosTileX == GetBase();
        }

        public bool IsMoving()
        {
            return State == UnitState.Backward || State == UnitState.Forward;
        }

        public int GetBase()
        {
            if (Owner == OwnerType.Player1)
                return 0;
            else
                return Const.NumberOfColumns + 1;
        }


        /// <summary>
        /// Returns number of tile where unit was on last turn if unit is still moving, 
        /// otherwise returns position of unit's base
        /// </summary>
        public int GetPreviousTile()
        {
            if (Type == UnitTypes.Cannon && FireEnabled)
                return PosTileX;
            if (Owner == OwnerType.Player1)
            {
                if (State == UnitState.Forward)
                    return PosTileX - 1;
                if (State == UnitState.Backward)
                    return PosTileX + 1;
            }
            else
            {
                if (State == UnitState.Forward)
                    return PosTileX + 1;
                if (State == UnitState.Backward)
                    return PosTileX - 1;
            }
            return GetBase();
        }

        public double GetNextCoordinateXWithStage (double stage)
        {
            int tile = GetPreviousTile();
            double ret = Const.FieldOriginX + Const.TileWidth * (tile + 0.5);
            if (Type == UnitTypes.Cannon && FireEnabled)
                return ret;
            double speed;
            if (Owner == OwnerType.Player1 && State == UnitState.Forward ||
                Owner == OwnerType.Player2 && State == UnitState.Backward)
                speed = Const.TileWidth;
            else
                speed = -Const.TileWidth;
            ret += speed * stage;
            return ret;
        }

        public Point2 GetCoordinate(double stage)
        {
            return new Point2(GetNextCoordinateXWithStage(stage), (Line + 0.5) * Const.TileHeight + Const.FieldOriginY);
        }
    }
}
