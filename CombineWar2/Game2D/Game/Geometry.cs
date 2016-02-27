using Game2D.Game.Units;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Game2D.Game
{

    //все вспомогательные классы я написал внизу

    //логика вызова методов движется сверху вниз, т.е. чем ниже, тем глубже вызов
    class Geometry
    {
        public static Point2 GetShellFinishPoint(Point2 startPoint, Point2 aimCurrentPoint, bool aimGoesToTheLeft)
        {
            double shellSpeed = Const.ProjectileSpeed * Const.TileWidth;
            double d = aimGoesToTheLeft ? -1 : 1;
            //подберем время, через которое юнит и снаряд окажутся в одной точке
            double l = 0, r = 1000;
            Point2 aimPos=new Point2();
            while(r-l > 0.0001){
                double mid = (r+l)/2;
                aimPos = new Point2(
                    aimCurrentPoint.x + d*mid,
                    aimCurrentPoint.y
                    );
                double shellDistance = startPoint.DistTo( aimPos);
                double timeShellReachesAim = shellDistance / shellSpeed;
                if( mid < timeShellReachesAim) // т е цель доедет до места встречи раньше, чем снаряд долетит. Время нужно увеличить
                    l=mid;
                else
                    r = mid;

            }

            return aimPos;
        }
        public static List<Collision>  GetAllCollisionsDuringTheTurn(List<Unit> unitList, List<Shell> shellList)
        {
            var units = unitList.Select(x => new UnitForCollision(x)).ToList();

            //Денис!!
            //тут надо сделать из снарядов объекты, удобные для меня
            //т.е. если тип решишь делать не unit, нужно поменять его и поменять
            //объект ShellForCollision, чтобы принимал в конструкторе твой тип и правильно реализовывал все методы

            var shells = shellList.Select(x => new ShellForCollision(x)).ToList();

            return CalculateAllCollisions(units, shells);
            
        }

        static List<Collision> CalculateAllCollisions(List<UnitForCollision> units, List<ShellForCollision> shells)
        {
            var collisionList = new List<Collision>();

            int blockCount = 10;
            double blockDuration = 1.0 / blockCount;
            //максимальное сближение снаряда и юнита за данный интервал времени
            double maxDistanceDecreasing = (Const.ProjectileSpeed * blockDuration
                + 1.0 * blockDuration) * Const.TileWidth;

            for (int i = 0; i < blockCount; i++)
            {
                double start = Math.Max(0, i * blockDuration);
                double finish = Math.Min(1,  (i + 1) * blockDuration);
                var pairList = new List<CollisionPair>(); 
                
                //тут надо добавить все пары снаряд-юнит, которые достаточно близко и в этом интервале времени теоретически могут столкнуться

                foreach (var shell in shells)
                {
                    //если на этом интервале еще не родился снаряд (или уже умер), пропускаем его
                    Point2? shellFinish =shell.Position(finish);
                    if(shellFinish ==null)
                        continue;
                    if(OutsideMap( shellFinish.Value))
                        pairList.Add(new CollisionPair{shell = shell});

                    Point2 shellStart = shell.Position(start) ?? shell.shell.StartPosition;
                    foreach (var unit in units)
                    {
                        if (unit.IsOwnerOfShell(shell))
                            continue;
                        if(unit.Position(start) == null)
                            continue;

                        Point2 unitStart = unit.Position(start).Value;

                        //берем чуть с запасом, чтобы не отмести лишнее
                        double safeDistanceSqr = Sqr(maxDistanceDecreasing + unit.maxRadius) + 0.01;
                        double startDistanceSqr = Sqr(unitStart.x - shellStart.x) + Sqr(unitStart.y - shellStart.y);
                        if (startDistanceSqr < safeDistanceSqr)
                            pairList.Add(new CollisionPair { shell = shell, unit = unit });
                    }
                }
                
                AddCollisionsDuringTimeInterval(collisionList, start, finish, pairList);

            }

                return collisionList;
        }
        public static int maxPairCount =-1;
        static void AddCollisionsDuringTimeInterval(List<Collision> collisionList, 
            double start, double finish, 
            List<CollisionPair> pairList)
        {
            maxPairCount = Math.Max(maxPairCount, pairList.Count);

            int stepCount = 100;
            double intervalDuration = finish - start;
            for (int i = 1; i <= stepCount; i++)
            {
                //получили стадию
                double stage = start+ intervalDuration / stepCount * i;
                if (stage > 1)
                    stage = 1;

                foreach (var pair in pairList)
                {
                     //если снаряда нет, то выходим
                    var shellPosition = pair.shell.Position(stage);
                    if(shellPosition == null) 
                        continue;

                    //не врезались ли в стену
                    if (OutsideMap(shellPosition.Value))
                    {
                        var collision = new Collision { stage = stage };
                        pair.shell.Collide(collision);
                        collisionList.Add(collision);
                        continue;
                    }

                    //если не врезались, значит можем попасть в юнит

                    //если юнита нет, выходим
                    if (pair.unit == null)
                        continue;
                    var unitPosition = pair.unit.Position(stage);
                    if (unitPosition == null)
                            continue;

                    //если есть, проверяем
                    if (PointInsideRect(shellPosition.Value, unitPosition.Value, pair.unit.hitBox))
                    {
                        double previousStage = start + intervalDuration / stepCount * (i - 1);
                        if (previousStage < 0)
                            previousStage = 0;

                        double collisionTime =  CalculateCollisionTimeWithBigPrecision(pair,previousStage, stage);
                        
                        pair.Collide(collisionList, collisionTime);
                    }
                }
                
            }
        }

        private static double CalculateCollisionTimeWithBigPrecision(CollisionPair pair, double stageWhenNoCollision, double stageWhenCollision)
        {
            //т.к. у stage шаг большой, не в том месте может создасться взрыв
            //поточнее найдем бин поиском
            

            //l - нет столкновения, r - есть
            double l = stageWhenNoCollision, r = stageWhenCollision, mid;
            while (r - l > 0.000001)
            {
                mid = (r + l) / 2;

                var exactShellPos = pair.shell.Position(mid);
                var exactUnitPos = pair.unit.Position(mid);
                bool collisionOccured = exactShellPos != null
                    && exactUnitPos != null
                    && PointInsideRect(exactShellPos.Value, exactUnitPos.Value, pair.unit.hitBox);

                if (collisionOccured)
                    r = mid;
                else
                    l = mid;
            }
            return r;
        }

        

        public static double Sqr(double x) { return x * x; }

        public static bool OutsideMap(Point2 point)
        {
            double left = Const.FieldOriginX + Const.TileWidth,
                right = left + Const.FieldWidth,
                top = Const.FieldOriginY,
                bottom = Const.FieldOriginY + Const.FieldHeight;

            return point.x < left || point.x > right || point.y < top || point.y > bottom;
        }
        static bool PointInsideRect(Point2 point, Point2 rectCenter, HitBoxType rectSize)
        {
            double left = rectCenter.x - rectSize.Width / 2 ,
                right = rectCenter.x + rectSize.Width / 2,
                top = rectCenter.y - rectSize.Height / 2,
                bottom = rectCenter.y + rectSize.Height / 2;


            return point.x >= left && point.x <= right && point.y >= top && point.y <= bottom;
        }


    }

   class Collision
    {
        //если unit == null , значит столкнулся снаряд со стеной
       public Unit unit=null;
       public Shell shell;
       public double stage;
        //соответственно место взрыва можно узнать по положению снаряда в этот момент
    }
    class UnitForCollision
    {
        Unit unit;
        //это нужно, т к хп в ходе симуляции может меняться, а менять исходный unit мы не должны
        int hp;
        public HitBoxType hitBox;
        public OwnerType team;

        //это нужно для сокращенного просчета большинства пар объектов, т.к. если слишком далеко, то нет смысла проверять
        public double maxRadius;
        public UnitForCollision(Unit unit)
        {
            this.unit = unit;
            this.hp = unit.Hp;
            this.team = unit.Owner;
            //создаем чуть увеиченный хитбокс, чтобы считать снаряд точкой
            //решение не идеально точное, но сойдет
            hitBox = new HitBoxType
            {
                Height = unit.HitBox.Height + Const.ShellRadius * 2,
                Width = unit.HitBox.Width + Const.ShellRadius * 2
            };
            this.maxRadius = Math.Sqrt(Geometry.Sqr(hitBox.Width / 2) + Geometry.Sqr(hitBox.Height / 2));

        }

        public Point2? Position(double stage)
        {
            if (hp <= 0)
                return null;
            return unit.GetCoordinate(stage);

        }

        public void Collide(Collision collision, ShellForCollision shell)
        {
            collision.unit = this.unit;
            if(this.team != shell.team)
                this.hp -= shell.damage;
        }

        public bool IsOwnerOfShell(ShellForCollision shell)
        {
            return this.unit == shell.ownerOfShell;
        }

    }

    class ShellForCollision
    {
        
        bool dead = false;
        public OwnerType team;
        public int damage;
        public Unit ownerOfShell;
        public Shell shell;
        public ShellForCollision(Shell shell)
        {
            this.shell = shell;
            this.team = shell.Owner;
            this.damage = shell.Damage;

           
            this.ownerOfShell = shell.OwnerUnit; 

        }

        public Point2? Position(double stage)
        {
            if (dead)
                return null;
            return shell.GetCoordinate(stage);

        }


        public void Collide(Collision collision)
        {
            collision.shell = this.shell;
            
            this.dead = true;
        }
    }

    class CollisionPair
    {
        public ShellForCollision shell;
        public UnitForCollision unit;
        public void Collide(List<Collision> collisionList, double stage)
        {
            if (!Geometry.OutsideMap(shell.Position(stage).Value))
            {
                var collision = new Collision { stage = stage };
                unit.Collide(collision, shell);
                shell.Collide(collision);
                collisionList.Add(collision);
            }
        }
    }
}
