using Game2D.Game.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game
{
    using DrawableObjects;
    using DrawableObjects.Effects;
    using Game2D.Game.Units;
    using System.Diagnostics;
    class Engine
    {
        Stopwatch timer = new Stopwatch();
        List<int> timeOfRunning = new List<int>();
        public void DoTurn(ref State previousState, SimultaneousTurn turn)
        {
            
            timer.Restart();

            State state = previousState;
            //должен осуществить ход, изменив состояние
            state.inEngineCount++;
           

            ProcessStoneSpawn(ref state, turn);

            

                state.players[0].Money += Const.MoneyPerTurn;
            state.players[1].Money += Const.MoneyPerTurn;

            if (turn.FirstPlayerTurn != null)
                ProcessTurn(ref state, state.players[0], turn.FirstPlayerTurn);
            if (turn.SecondPlayerTurn != null)
                ProcessTurn(ref state, state.players[1], turn.SecondPlayerTurn);

            ProcessUnitsState(ref state, state.players[0], false);
            ProcessUnitsState(ref state, state.players[1], true); //worth=1ms

            ProcessUnitsCollisions(ref state);//worth=1ms

            ProcessFireControl(ref state);//worth=1ms

            ProcessMovingAnimation(ref state);//worth=0ms

            ProcessFire(ref state); //average time = 9ms, worth = 100 ms

            #region missile_test
            //AddProjectile(ref state, new Point2(Const.FieldOriginX, Const.FieldOriginY), new Point2(turn.x, turn.y), 0.5, true, 0);

            //AddProjectile(ref state, new Point2(Const.FieldOriginX, Const.FieldOriginY), new Point2(turn.x - 10, turn.y - 10), 0.5, true, 0);
            //AddProjectile(ref state, new Point2(Const.FieldOriginX, Const.FieldOriginY), new Point2(turn.x - 10, turn.y + 10), 0.5, true, 0);
            //AddProjectile(ref state, new Point2(Const.FieldOriginX, Const.FieldOriginY), new Point2(turn.x + 10, turn.y - 10), 0.5, true, 0);
            //AddProjectile(ref state, new Point2(Const.FieldOriginX, Const.FieldOriginY), new Point2(turn.x + 10, turn.y + 10), 0.5, true, 0);
            #endregion

            #region animation_test
            //Effect eff = new Effect();
            //DynamicObject obj = new DynamicObject(new Sprite(ESprite.shell1, 20, 20,
            //                                                 new Vector2(0, 0)));
            //DynamicObject explsn = new DynamicObject(new Sprite(ESprite.explosion, 40, 40,
            //                                                    new Vector2(0, 0)));
            //eff.AddNextStep(obj);
            //eff.AddAnimationToLastStep(turn.x, turn.y, 360, 1.0);
            //eff.AddAnimationToLastStep(turn.x / 2, turn.y / 2, -360, 1.5);
            //eff.AddAnimationToLastStep(-turn.x / 2, -turn.y / 2, 0, 0.75);
            //eff.AddAnimationToLastStep(turn.x, turn.y, 360, 1.0);
            //eff.AddAnimationToLastStep(turn.x / 2, turn.y / 2, -360, 1.5);
            //eff.AddAnimationToLastStep(-turn.x / 2, -turn.y / 2, 0, 0.75);
            //eff.AddAnimationToLastStep(turn.x, turn.y, 360, 1.0);
            //eff.AddAnimationToLastStep(turn.x / 2, turn.y / 2, -360, 1.5);
            //eff.AddAnimationToLastStep(-turn.x / 2, -turn.y / 2, 0, 0.75);
            //eff.AddAnimationToLastStep(turn.x, turn.y, 360, 1.0);
            //eff.AddAnimationToLastStep(turn.x / 2, turn.y / 2, -360, 1.5);
            //eff.AddAnimationToLastStep(-turn.x / 2, -turn.y / 2, 0, 0.75);
            //eff.AddNextStep(explsn);
            //eff.AddAnimationToLastStep(0, 0, 0, 1.0);
            //state.effects.AddEffect(eff);
            #endregion




            timeOfRunning.Add((int)timer.ElapsedMilliseconds);
            if (state.turn == 190)
            {
                double average = timeOfRunning.Average();
                double worth = timeOfRunning.Max();
                int e = -1;
            }
            //....
        }

        void ProcessStoneSpawn(ref State state, SimultaneousTurn turn)
        {
            state.inStoneProcessCount++;

            if (state.stoneCount >= Const.MaxStones)
                return;
            state.inStoneContinueCount++;
            Random rand = new Random(turn.randomSeed);
            double randomDouble =rand.NextDouble();

            var xxx = new List<int>();
            var yyy = new List<int>();
            //for (int i = 0; i < state.Stones.Length; i++)
            //{
            //    if (state.Stones[i] >= 1)
            //    {
                    
            //    }
            //}
            if (turn.stoneRespawnPos == null)
            {
                turn.stoneRespawnPos = new List<int>();
                turn.stoneRespawnLine = new List<int>();
                if (Utility.doubleLessOrEqual(randomDouble, Const.StoneSpawnProbabilityPerTurn))
                {
                    int line = rand.Next(Const.NumberOfLines);
                    while (state.Stones[line] != -1)
                        line = rand.Next(Const.NumberOfLines);
                    int x = (Const.NumberOfColumns + 2) / 2 - Const.StoneRadius + (Const.NumberOfColumns % 2 == 0 ? 0 : 1) + rand.Next(2 * Const.StoneRadius);

                    xxx.Add(x);
                    yyy.Add(line);
                    turn.stoneRespawnLine.Add(line);
                    turn.stoneRespawnPos.Add(x);
                    state.TryAddStone(line, x);
                }
            }
            else
            {
                for (int i = 0; i < turn.stoneRespawnPos.Count; i++)
                {
                    state.TryAddStone(turn.stoneRespawnLine[i], turn.stoneRespawnPos[i]);
                }
            }
            state.stonePositionsX.Add(xxx);
            state.stonePositionsY.Add(yyy);
        }

        void ProcessTurn(ref State state, Player player, Turn turn)
        {
            if (turn.TurnStatus == ExternalProgramExecuteResult.Ok)
            {
                //если все нормально

                bool reflected = player.Owner == OwnerType.Player1 ? false : true;

                foreach (Turn.Command command in turn.commands)
                {
                    switch (command.type)
                    {
                        case Turn.CommandType.Buy:
                            ProcessCommandBuy(ref state, player, command, reflected);
                            break;
                        case Turn.CommandType.Start:
                            ProcessCommandStart(ref state, player, command, reflected);
                            break;
                        case Turn.CommandType.Remove:
                            ProcessCommandRemove(ref state, player, command, reflected);
                            break;
                        //default:
                        //    command.comment = "Неверный номер команды";
                        //    break;
                    }
                }
            }

            AddTurnComments(state, turn);
        }

        void ProcessCommandBuy(ref State state, Player player, Turn.Command command, bool reflected)
        {
            Unit temp = null;
            int type = command.arguments[0];
            int line = command.arguments[1];
            if (line < 0 || line >= Const.NumberOfLines)
            {
                command.comment = "Неверно указана горизонталь";
                return;
            }
            int bas = reflected ? Const.NumberOfColumns + 1 : 0;
            int destination = reflected ? (Const.NumberOfColumns + 1 - command.arguments[2]) : command.arguments[2];
            if (destination < 1 || destination > Const.NumberOfColumns)
            {
                command.comment = "Неверно указано место назначения";
                return;
            }
            OwnerType owner = player.Owner;
            switch (type)
            {
                case (int)UnitTypes.Armored:
                    temp = new Armored(owner, line, bas, destination);
                    break;
                case (int)UnitTypes.Cannon:
                    temp = new Cannon(owner, line, bas, destination);
                    break;
                case (int)UnitTypes.Combine:
                    temp = new Combine(owner, line, bas, destination);
                    break;
                case (int)UnitTypes.Mine:
                    temp = new Mine(owner, line, bas, destination);
                    break;
                case (int)UnitTypes.Tank:
                    temp = new Tank(owner, line, bas, destination);
                    break;
                default:
                    command.comment = "Неверный тип юнита";
                    return;
            }
            if (player.Money < temp.Cost)
            {
                command.comment = "Недостаточно денег";
                return;
            }
            if (player.Units[line] != null)
            {
                if (player.Units[line].State != UnitState.Dead)
                {
                    command.comment = "Горизонталь занята";
                    return;
                }
                else
                    state.DestroyUnit(player.Units[line]);
            }
            player.Money -= temp.Cost;
            state.AddUnit(bas, line, player, temp);
        }

        void ProcessCommandStart(ref State state, Player player, Turn.Command command, bool reflected)
        {
            int line = command.arguments[0];
            int bas = reflected ? Const.NumberOfColumns + 1 : 0;
            if (line < 0 || line >= Const.NumberOfLines)
            {
                command.comment = "Неверно указана горизонталь";
                return;
            }
            int destination = reflected ? (Const.NumberOfColumns + 1 - command.arguments[1]) : command.arguments[1];
            if (destination < 1 || destination > Const.NumberOfColumns)
            {
                command.comment = "Неверно указано место назначения";
                return;
            }
            if (player.Units[line] == null || player.Units[line].State == UnitState.Dead)
            {
                command.comment = "На указанной горизонтали нет юнитов";
                return;
            }
            if (player.Units[line].PosTileX != bas)
            {
                command.comment = "На указанной горизонтали юнит уже в поле";
                return;
            }

            player.Units[line].State = UnitState.Forward;
            player.Units[line].Destination = destination;
        }

        void ProcessCommandRemove(ref State state, Player player, Turn.Command command, bool reflected)
        {
            int line = command.arguments[0];
            int bas = (reflected ? Const.NumberOfColumns + 1 : 0);
            if (line < 0 || line >= Const.NumberOfLines)
            {
                command.comment = "Неверно указана горизонталь";
                return;
            }
            if (player.Units[line] == null)
            {
                command.comment = "На указанной горизонтали нет юнитов";
                return;
            }
            if (player.Units[line].PosTileX != bas)
            {
                command.comment = "Указанный юнит не находится на базе";
                return;
            }
            state.DestroyUnit(player.Units[line]);
        }

        void ProcessUnitsState(ref State state, Player player, bool reflected)
        {
            for (int i = 0; i < Const.NumberOfLines; i++)
            {
                if (player.Units[i] == null)
                    continue;
                Unit unit = player.Units[i];
                if (unit.State != UnitState.Dead && (unit.Type == UnitTypes.Combine || Const.EveryoneCanTakeStone))
                {
                    if (unit.PosTileX == state.Stones[i])
                        CollectStone(ref state, unit);
                }
                /*if (unit.Type == UnitTypes.Cannon && unit.IsMoving() && unit.FireEnabled)
                    continue;*/
                if (unit.AtBase())
                {
                    if ((unit.Type == UnitTypes.Combine || Const.EveryoneCanTakeStone) &&
                                    unit.WithStone == true)
                    {
                        player.Score += Const.StoneScore;
                        unit.WithStone = false;
                        state.stoneCount--;
                        state.Stones[i] = -1;
                        DisableActive(ref state, unit);
                    }
                }
                switch (unit.State)
                {
                    case UnitState.Forward:
                        if (!unit.AtDestination())
                        {
                            if (unit.Type == UnitTypes.Mine && Math.Abs(unit.PosTileX - unit.Destination) == 1)
                                EnableActive(ref state, unit);
                        }
                        else
                        {
                            unit.State = UnitState.Backward;
                            if (unit.Type == UnitTypes.Mine)
                            {
                                MineExplosion(ref state, (Mine)unit, reflected ? state.players[0] : state.players[1]);
                                state.DestroyUnit(unit);
                            }
                        }
                        break;
                    case UnitState.Backward:
                        if (unit.AtBase())
                        {
                            if (unit.Hp < unit.MaxHp)
                                unit.State = UnitState.Repair;
                            else
                                unit.State = UnitState.Ready;
                        }

                        break;
                    case UnitState.Repair:
                        unit.Hp = Math.Min(unit.MaxHp, unit.Hp + Const.RepairRate);
                        if (unit.Hp == unit.MaxHp)
                            unit.State = UnitState.Ready;
                        break;
                    case UnitState.Dead:
                        state.DestroyUnit(unit);
                        break;
                }
                unit.HpDuringTurn.Clear();
                unit.HpDuringTurn.Add(0.0, unit.Hp);
            }
        }
            
        void ProcessUnitsCollisions(ref State state)
        {
            for (int i = 0; i < Const.NumberOfLines; i++)
            {
                Unit unit = state.players[0].Units[i];
                if (unit == null || unit.State == UnitState.Dead)
                    continue;

                if (unit.State == UnitState.Forward || (unit.Type == UnitTypes.Cannon && unit.IsMoving()))
                {
                    Unit enemyUnit = state.players[1].Units[i];
                    if (enemyUnit == null || enemyUnit.State == UnitState.Dead)
                        continue;
                    int unitPosX = unit.PosTileX;
                    int enemyPosX = enemyUnit.PosTileX;

                    if ((enemyPosX - 1 <= unitPosX + 1 &&
                        enemyUnit.State == UnitState.Forward) || enemyPosX <= unitPosX + 1)
                    {
                        if ((unit.Type == UnitTypes.Cannon || enemyUnit.Type == UnitTypes.Cannon) &&
                            ((enemyPosX - 1 > unitPosX + 1 &&
                            enemyUnit.State == UnitState.Forward) || enemyPosX > unitPosX + 1))
                            continue;
                        if (unit.Type == UnitTypes.Mine)
                        {
                            MineExplosion(ref state, (Mine)unit, state.players[1]);
                            state.DestroyUnit(unit);
                            if (enemyUnit.Type == UnitTypes.Mine)
                            {
                                MineExplosion(ref state, (Mine)enemyUnit, state.players[0]);
                                state.DestroyUnit(enemyUnit);
                                break;
                            }
                            break;
                        }
                        if (!unit.AtBase())
                            unit.State = UnitState.Backward;
                        else
                        {
                            if (unit.Hp < unit.MaxHp)
                                unit.State = UnitState.Repair;
                            else
                                unit.State = UnitState.Ready;
                        }
                        if (enemyUnit.Type == UnitTypes.Mine)
                        {
                            MineExplosion(ref state, (Mine)enemyUnit, state.players[0]);
                            state.DestroyUnit(enemyUnit);
                            break;
                        }
                        if (!enemyUnit.AtBase())
                            enemyUnit.State = UnitState.Backward;
                        else
                        {
                            if (enemyUnit.Hp < enemyUnit.MaxHp)
                                enemyUnit.State = UnitState.Repair;
                            else
                                enemyUnit.State = UnitState.Ready;
                        }
                    }
                }
            }
        }

        void ProcessMovingAnimation(ref State state)
        {
            for (int n = 0; n < 2; n++)
                for (int i = 0; i < Const.NumberOfLines; i++)
                {
                    Unit unit = state.players[n].Units[i];
                    if (unit == null || unit.State == UnitState.Dead)
                        continue;
                    if (unit.Type == UnitTypes.Cannon && unit.FireEnabled)
                        continue;
                    switch (unit.State)
                    {
                        case UnitState.Forward:
                            unit.MoveForward();
                            break;
                        case UnitState.Backward:
                            unit.MoveBackward();
                            break;
                    }
                }
        }

        void ProcessFireControl(ref State state)
        {
            EnableFire(state.players[0], state.players[1], ref state);
            EnableFire(state.players[1], state.players[0], ref state);
        }

        void ProcessFire (ref State state)
        {
            ProcessShots(ref state, state.players[0], state.players[1]);
            ProcessShots(ref state, state.players[1], state.players[0]);
           
            List<Unit> UnitsForCollision = new List<Unit>();
            for (int i = 0; i < Const.NumberOfLines; i++)
            {
                Unit unit1 = state.players[0].Units[i];
                Unit unit2 = state.players[1].Units[i];
                if (unit1 != null && unit1.State != UnitState.Dead)
                {
                    UnitsForCollision.Add(unit1);
                }
                if (unit2 != null && unit2.State != UnitState.Dead)
                {
                    UnitsForCollision.Add(unit2);
                }
            }


            StartCollisionsProcessing:
            List<Collision> collisions = Geometry.GetAllCollisionsDuringTheTurn(UnitsForCollision, state.shells); 
            for (int i = 0; i < collisions.Count; i++)
            {
                Collision collision = collisions[i];

                Point2 finish = (Point2)collision.shell.GetCoordinate(collision.stage);
                
                state.shells.Remove(collision.shell);
                if (collision.unit != null)
                {
                    //debug code
                    double _rightSide = collision.unit.GetNextCoordinateXWithStage(collision.stage) +
                                        collision.unit.HitBox.Width / 2;
                    double _leftSide = collision.unit.GetNextCoordinateXWithStage(collision.stage) -
                                       collision.unit.HitBox.Width / 2;
                    //finish.x = _rightSide;
                    //end
                    if (collision.unit.Owner != collision.shell.Owner)
                    {
                        collision.unit.Hp -= collision.shell.Damage;
                        collision.unit.HpDuringTurn[collision.stage] = collision.unit.Hp;
                    }
                    if (collision.unit.Hp <= 0)
                    {
                        double lastX = collision.unit.GetNextCoordinateXWithStage(0);
                        double deathX = collision.unit.GetNextCoordinateXWithStage(collision.stage);
                        collision.unit.CancelMove();
                        collision.unit.AddMove(deathX - lastX,
                                               0, collision.stage);
                        collision.unit.State = UnitState.Dead;
                        collision.unit.DeathStage = collision.stage;
                        AddProjectile(ref state, collision.shell.StartPosition,
                                      finish,
                                      collision.stage - collision.shell.LaunchStage, false, collision.shell.LaunchStage);
                        Effect eff = new Effect();
                        eff.AddNextStep(new DynamicObject(new Sprite(ESprite.explosion, Const.TileWidth*1.2, Const.TileHeight*1.2,
                                                                     new Vector2(deathX, collision.unit.GetCoordinate(0).y))));
                        eff.AddAnimationToLastStep(0, 0, 0, 1.0, collision.stage);
                        state.effects.AddEffect(eff);
                        for (int j = 0; j < collisions.Count; j++)
                        {
                            if (collisions[j].shell.OwnerUnit == collision.unit && 
                                Utility.doubleGreaterOrEqual(collisions[j].shell.LaunchStage, collision.stage))
                                collisions.Remove(collisions[j]);
                        }
                        for (int j = 0; j < state.shells.Count; j++)
                        {
                            if (state.shells[j].OwnerUnit == collision.unit &&
                                state.shells[j].LaunchStage > collision.stage + 0.0001)
                                state.shells.Remove(state.shells[j]);
                        }
                        UnitsForCollision.Remove(collision.unit);
                        goto StartCollisionsProcessing;
                    }
                    else
                    {
                        AddProjectile(ref state, collision.shell.StartPosition,
                                      finish,
                                      collision.stage - collision.shell.LaunchStage, false, collision.shell.LaunchStage);
                    }
                }
                else
                {
                    AddProjectile(ref state, collision.shell.StartPosition,
                                      finish,
                                      collision.stage - collision.shell.LaunchStage, false, collision.shell.LaunchStage);
                }
            }
            foreach(Shell shell in state.shells)
            {
                Point2 start = shell.StartPosition;
                Point2 finish = (Point2)shell.GetCoordinate(1.0);
                Effect pr = new Effect();
                pr.AddNextStep(new DynamicObject(new Sprite(ESprite.shell1, 2, 2, new Vector2(start.x, start.y))));
                pr.AddAnimationToLastStep(finish.x - start.x, finish.y - start.y, 0, 1.0 - shell.LaunchStage, shell.LaunchStage);
                state.effects.AddEffect(pr);
                shell.StartPosition = finish;
                shell.LaunchStage = 0.0;
            }
        }

        void AddTurnComments(State state, Turn turn)
        {
            state.turnCommentList.Add(TurnReceiver.GetShortTurnComment(turn));
            state.inputList.Add(turn.input);
            state.outputList.Add(TurnReceiver.GetOutputWithComments(turn));
        }

#region secondary classes and functions
        void EnableFire(Player player, Player enemyPlayer, ref State state)
        {
            for (int i = 0; i < Const.NumberOfLines; i++)
            {
                Unit unit = player.Units[i];
                if (unit == null || unit.Type == UnitTypes.Combine || unit.State == UnitState.Dead)
                    continue;
                double radius = unit.ThreatRadius;
                unit.FireEnabled = false;
                DisableActive(ref state, unit);
                if (unit.AtBase() || unit.GetPreviousTile() == unit.GetBase())
                    continue;
                for (int j = 0; j < Const.NumberOfLines; j++)
                {
                    Unit enemyUnit = enemyPlayer.Units[j];
                    if (enemyUnit == null || enemyUnit.AtBase() || enemyUnit.State == UnitState.Dead)
                        continue;
                    if (Utility.doubleGreaterOrEqual(Math.Pow(radius, 2), Math.Pow(Math.Abs(i - j), 2) + Math.Pow(Math.Abs(unit.PosTileX - enemyUnit.PosTileX), 2)))
                    {
                        /*if (unit.Type == UnitTypes.Cannon)
                            unit.CancelMove();*/
                        unit.FireEnabled = true;
                        EnableActive(ref state, unit);
                        break;
                    }
                }
            }
        }

        Unit GetNearestEnemy(Point2 from, Player enemyPlayer, double stage, out Point2 point)
        {
            Unit ret = null;
            point = new Point2();
            double minDist = double.MaxValue;
            for (int i = 0; i < Const.NumberOfLines; i++)
            {
                Unit enemyUnit = enemyPlayer.Units[i];
                if (enemyUnit == null || enemyUnit.AtBase() || enemyUnit.State == UnitState.Dead)
                    continue;
                Point2 temp = new Point2(enemyUnit.GetNextCoordinateXWithStage(stage),
                                         Const.FieldOriginY + Const.TileHeight * (i + 0.5));
                if (from.DistTo(temp) < minDist)
                {
                    minDist = from.DistTo(temp);
                    point = temp;
                    ret = enemyUnit;
                }
            }
            return ret;
        }

        double? GetAngleToNearestEnemy (Unit unit, int line, Player enemyPlayer, double stage)
        {
            Point2 from = new Point2(unit.GetNextCoordinateXWithStage(stage),
                                     Const.FieldOriginY + Const.TileHeight * (line + 0.5));
            Point2 to;
            Unit enemyUnit = GetNearestEnemy(from, enemyPlayer, stage, out to);
            if (enemyUnit == null)
                return null;
            double time = from.DistTo(to) / (Const.ProjectileSpeed * Const.TileWidth);
            to.x = enemyUnit.GetNextCoordinateXWithStage(stage + time);
            double angle_to = from.angleTo(to);
            if ((unit.Owner == OwnerType.Player1 && from.x > to.x) ||
                (unit.Owner == OwnerType.Player2 && from.x <= to.x))
                angle_to -= 180;
            if (angle_to < -180)
                angle_to += 360;
            double ret_angle = (angle_to - unit.CurrAngle);
            ret_angle = Utility.LimitAngle(ret_angle);
            return (ret_angle);
        }

        List<GunPosition> ProcessGunRotate(ref State state, Unit unit, Player enemyPlayer)
        {
            List<GunPosition> ret = new List<GunPosition>();
            double currStage = 0.0;
            double lastStage = currStage;
            double lastAngle = unit.CurrAngle;
            double step = 1.0 / 10;
            while (Utility.doubleLess(currStage, 1))
            {
                double x = unit.GetCoordinate(currStage).x;
                double y = unit.GetCoordinate(currStage).y;
                
                double? possibleAngleToEnemy = GetAngleToNearestEnemy(unit, unit.Line, enemyPlayer, currStage);
                if (possibleAngleToEnemy == null)
                {
                    ret.Add(new GunPosition(lastAngle, GunPosition.DirectionType.None));
                    return ret;
                }
                double angleToEnemy = (double)possibleAngleToEnemy;
                if (angleToEnemy > 0)
                    unit.CurrAngle += Utility.MinAbs(angleToEnemy, unit.RotationRate * (currStage - lastStage));
                else
                    unit.CurrAngle -= Utility.MinAbs(angleToEnemy, unit.RotationRate * (currStage - lastStage));
                if (Utility.doubleGreater(currStage - lastStage, 0.0))
                {
                    unit.AddGunRotate(unit.CurrAngle - lastAngle, currStage - lastStage);
                    GunPosition.DirectionType dir;
                    if (unit.CurrAngle > lastAngle)
                        dir = GunPosition.DirectionType.Clockwise;
                    else
                        dir = GunPosition.DirectionType.AntiClockwise;
                    ret.Add(new GunPosition(lastAngle, dir));
                }
                //unit.CurrAngle = Utility.LimitAngle(unit.CurrAngle);
                lastAngle = unit.CurrAngle;
                lastStage = currStage;
                currStage += step;
            }
            ret.Add(new GunPosition(lastAngle, GunPosition.DirectionType.None));
            return ret;
        }

        void ProcessShellLaunch(ref State state, Unit unit, List<GunPosition> gunPositions)
        {
            double shotStage;
            if (Utility.doubleEqual(unit.StageOfLastShot, -1))
                shotStage = 0.0;
            else
            {
                shotStage = unit.StageOfLastShot + 1.0 / unit.DamageRate;
                if (Utility.doubleGreaterOrEqual(shotStage, 1.0))
                    shotStage -= 1.0;
            }
            double currStage = 0.0;
            double lastStage = currStage;
            double step = 1.0 / unit.DamageRate;
            int stepCount = gunPositions.Count;
            while (Utility.doubleLess(currStage, 1))
            {
                double x = unit.GetCoordinate(currStage).x;
                double y = unit.GetCoordinate(currStage).y;
                int index = (int) Math.Truncate(currStage * stepCount);
                double stageShift = (currStage - index * 1.0 / stepCount) * stepCount; 
                GunPosition gunPosition = gunPositions[index];
                double angle = gunPosition.Angle;
                double currAngle;
                if (index + 1 < stepCount)
                {
                    double nextAngle = gunPositions[index + 1].Angle;
                    if (gunPosition.Direction != GunPosition.DirectionType.None)
                    {
                        currAngle = angle + (nextAngle - angle) * stageShift;
                    }
                    else
                        currAngle = angle;
                }
                else
                    currAngle = angle;
                if (unit.Owner == OwnerType.Player2)
                {
                    currAngle -= 180;
                    currAngle = Utility.LimitAngle(currAngle);
                }
                state.shells.Add(new Shell(new Point2(x, y), currAngle, unit.Owner, unit, currStage));
                unit.StageOfLastShot = currStage;
                shotStage += 1.0 / unit.DamageRate;
                currStage += step;
            }
            if (Utility.doubleGreaterOrEqual(shotStage, 1.0 + 1.0 / unit.DamageRate))
                unit.StageOfLastShot -= 1.0;
        }

        void ProcessShots(ref State state, Player player, Player enemyPlayer)
        {
            OwnerType owner = player.Owner;
            for (int i = 0; i < Const.NumberOfLines; i++)
            {
                Unit unit = player.Units[i];
                if (unit == null || !unit.FireEnabled || unit.State == UnitState.Dead)
                    continue;
                ProcessShellLaunch(ref state, unit, ProcessGunRotate(ref state, unit, enemyPlayer));
                /*double shotStage;
                if (Utility.doubleEqual(unit.StageOfLastShot, -1))
                    shotStage = 0.0;
                else
                {
                    shotStage = unit.StageOfLastShot + 1.0 / unit.DamageRate;
                    if (Utility.doubleGreaterOrEqual(shotStage, 1.0))
                        shotStage -= 1.0;
                }
                double currStage = 0.0;
                double lastStage = currStage;
                double lastAngle = unit.CurrAngle;
                double step = 1.0 / 10;
                while (Utility.doubleLess(currStage, 1))
                {
                    double x = unit.GetCoordinate(currStage).x;
                    double y = unit.GetCoordinate(currStage).y;
                    if (Utility.doubleGreaterOrEqual(currStage, shotStage))
                    {
                        double angle = unit.CurrAngle;
                        if (player.Owner == OwnerType.Player2)
                        {
                            angle -= 180;
                            angle = Utility.LimitAngle(angle);
                        }
                        state.shells.Add(new Shell(new Point2(x, y), angle, owner, unit, currStage));
                        unit.StageOfLastShot = currStage;
                        shotStage += 1.0 / unit.DamageRate;
                    }
                    double? possibleAngleToEnemy = GetAngleToNearestEnemy(unit, i, enemyPlayer, currStage);
                    if (possibleAngleToEnemy == null)
                        return;
                    double angleToEnemy = (double)possibleAngleToEnemy;
                    if (angleToEnemy > 0)
                        unit.CurrAngle += Utility.MinAbs(angleToEnemy, unit.RotationRate * (currStage - lastStage));
                    else
                        unit.CurrAngle -= Utility.MinAbs(angleToEnemy, unit.RotationRate * (currStage - lastStage));
                    if (Utility.doubleGreater(currStage - lastStage, 0.0))
                        unit.AddGunRotate(unit.CurrAngle - lastAngle, currStage - lastStage);
                    //unit.CurrAngle = Utility.LimitAngle(unit.CurrAngle);
                    lastAngle = unit.CurrAngle;
                    lastStage = currStage;
                    currStage += step;
                }
                if (Utility.doubleGreaterOrEqual(shotStage, 1.0 + 1.0 / unit.DamageRate))
                    unit.StageOfLastShot -= 1.0;*/
            }
        }

        void MineExplosion(ref State state, Mine mine, Player enemyPlayer)
        {
            Effect pr = new Effect();

            //todo denis
            // надо при взрыве бомбы рисовать следующую строку. Там будет меняться последний параметр альфа. Он сначала увеличивается от 0 до 1 (до половины времени), затем уменьшается до 0
            //Animator.Rectangle(ref frame, ESprite.explosionPoint, mine.obj.objs[0].Pos.x - Const.TileWidth * 2.5, mine.obj.objs[0].Pos.y - Const.TileHeight * 2.5, Const.TileWidth * 5, Const.TileHeight * 5, alpha);

            pr.AddNextStep(new DynamicObject(new Sprite(ESprite.explosionNuclear, 5 * Const.TileWidth*1.7791, 5 * Const.TileHeight, new Vector2(mine.obj.objs[0].Pos.x, 
                                                                                                                                  mine.obj.objs[0].Pos.y))));
            pr.AddAnimationToLastStep(0, 0, 0, 2.0);
            state.effects.AddEffect(pr);
            int radius = mine.ExplosionRadius;
            for (int i = 0; i < Const.NumberOfLines; i++)
            {
                Unit enemyUnit = enemyPlayer.Units[i];
                if (enemyUnit == null || enemyUnit.AtBase() || enemyUnit.State == UnitState.Dead)
                    continue;
                if (Math.Abs(enemyUnit.PosTileX - mine.PosTileX) <= mine.ExplosionRadius / 2 && 
                    Math.Abs(enemyUnit.Line - mine.Line) <= mine.ExplosionRadius / 2)
                {
                    enemyUnit.Hp -= mine.Damage;
                    if (enemyUnit.Hp <= 0)
                        enemyUnit.State = UnitState.Dead;
                }
            }
        }

        void AddProjectile(ref State state, Point2 from, Point2 to, double duration, bool destroyed, double latency)
        {
            Effect pr = new Effect();
            pr.AddNextStep(new DynamicObject(new Sprite(ESprite.shell1, 2, 2, new Vector2(from.x, from.y))));
            pr.AddAnimationToLastStep(to.x - from.x, to.y - from.y, 0, duration, latency);
            if (destroyed)
            {
                pr.AddNextStep(new DynamicObject(new Sprite(ESprite.explosion, Const.TileWidth, Const.TileHeight, new Vector2(0, 0))));
                pr.AddAnimationToLastStep(0, 0, 0, 1.0);
            }
            else
            {
                pr.AddNextStep(new DynamicObject(new Sprite(ESprite.explosion, 5, 5, new Vector2(0, 0))));
                pr.AddAnimationToLastStep(0, 0, 0, 0.2);
            }
            state.effects.AddEffect(pr);
        }

        void OpenFireOnWall(ref State state, Point2 from, double angle, double time)
        {
            double upperBorder = Const.FieldOriginY;
            double lowerBorder = Const.FieldOriginY + Const.FieldHeight;
            double leftBorder = Const.FieldOriginX + Const.TileWidth;
            double rightBorder = Const.FieldOriginX + Const.FieldWidth + Const.TileWidth;
            double speedX = Const.ProjectileSpeed * Math.Cos(angle / 180 * Math.PI) * Const.TileWidth;
            double speedY = Const.ProjectileSpeed * Math.Sin(angle / 180 * Math.PI) * Const.TileHeight;
            if (speedY < 0)
            {
                double t = (from.y - upperBorder) / (-speedY);
                double x = from.x + speedX * t;
                if (Utility.inClosedInterval(x, leftBorder, rightBorder))
                {
                    AddProjectile(ref state, from, new Point2(x, upperBorder), t, false, time);
                    return;
                }
            }
            else
            {
                double t = (lowerBorder - from.y) / speedY;
                double x = from.x + speedX * t;
                if (Utility.inClosedInterval(x, lowerBorder, rightBorder))
                {
                    AddProjectile(ref state, from, new Point2(x, lowerBorder), t, false, time);
                    return;
                }
            }
            if (speedX < 0)
            {
                double t = (from.x - leftBorder) / (-speedX);
                double y = from.y + speedY * t;
                if (Utility.inClosedInterval(y, upperBorder, lowerBorder))
                {
                    AddProjectile(ref state, from, new Point2(leftBorder, y), t, false, time);
                    return;
                }
            }
            else
            {
                double t = (rightBorder - from.x) / speedX;
                double y = from.y + speedY * t;
                if (Utility.inClosedInterval(y, upperBorder, lowerBorder))
                {
                    AddProjectile(ref state, from, new Point2(rightBorder, y), t, false, time);
                    return;
                }
            }
        }

        void EnableActive(ref State state, Unit unit)
        {
            bool lastActiveState = unit.Active;
            unit.Active = true;
            if (lastActiveState == false)
            {
                state.objects.Add(unit.obj.objs.Last());
            }
        }

        void DisableActive(ref State state, Unit unit)
        {
            if (Const.EveryoneCanTakeStone && unit.WithStone)
                return;
            if (unit.Active)
                state.objects.Remove(unit.obj.objs.Last());
            unit.Active = false;
        }

        void CollectStone (ref State state, Unit unit)
        {
            int line = unit.Line;
            Debug.Assert(state.Stones[line] != -1 && state.Stones[line] != -2 && unit.PosTileX == state.Stones[line]);
            unit.WithStone = true;
            EnableActive(ref state, unit);
            state.DeleteStone(line, unit.PosTileX);
            state.stoneCount++;
            state.Stones[line] = -2;
        }

        struct GunPosition
        {
            public enum DirectionType { Clockwise, AntiClockwise, None };
 
            public double Angle;
            public DirectionType Direction;
            public GunPosition(double angle, DirectionType direction)
            {
                Angle = angle;
                Direction = direction;
            }
        }
        /*class Hit
        {
            public Unit fromUnit;
            public Unit toUnit;
            public Point2 from;
            public Point2 to;
            public double time;
            public int damage;
        }*/
#endregion

    }
}
