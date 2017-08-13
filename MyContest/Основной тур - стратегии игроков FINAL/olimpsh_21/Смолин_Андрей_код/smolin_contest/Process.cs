using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smolin_contest
{
    public enum EChars
    {
        Jack = 0,
        Defender = 1,
        Support = 2,
        Attacker = 3,
        Pro = 4
    }

    class Process
    {
        private const int defDist = 20,
            supDist = 25,
            attDist = 25,
            proDist = 20,
            jackDist = 30,
            bewareDist = 5,
            bewareDistL = 10;

        private Input input;

        public List<Bot> Bots;
        public double ToX = -1, ToY = -1;

        public Process(Input input)
        {
            this.input = input;
            Bots = input.Me.Bots;

            // Defender logic
            if (!input.Me.HasBall && botToPoint(EChars.Defender, true) < defDist)
                goTo(EChars.Defender, true);
            else
            {
                // pass
                if (hasBall(EChars.Defender))
                    pass(EChars.Jack);
                else if (!input.Me.HasBall)
                    goTo(EChars.Defender, 15, 15);
            }


            // Support logic
            if (!input.Me.HasBall && botToPoint(EChars.Support, true) < supDist)
                goTo(EChars.Support, true);
            else
            {
                // pass
                if (hasBall(EChars.Support))
                {
                    if (input.Tick < 100) // TICK CHECK
                        passToNearest(EChars.Support, EChars.Attacker, EChars.Pro, EChars.Jack);
                    else
                    {
                        Bot sup = getBot(EChars.Support);
                        kick(sup.X + 30, sup.Y + 9);
                    }
                }
                else if (!input.Me.HasBall)
                    goTo(EChars.Support, 20, 40);
            }


            // Jack logic
            if (hasBall(EChars.Jack))
            {
                if (input.Tick < 100 && nearestEnemy(EChars.Jack) < bewareDist) // TICK CHECK
                {
                    Bot jack = getBot(EChars.Jack);
                    kick(jack.X + 10, 5);
                }
                else if (botToPoint(EChars.Jack, Input.MaxX, 10) < jackDist)
                    kick(Input.MaxX, 5);
                else
                    goTo(EChars.Jack, 100, 10);
            }
            else
                goTo(EChars.Jack, false);


            // Attacker logic
            if (!input.Me.HasBall)
                goTo(EChars.Attacker, true);
            else if (hasBall(EChars.Attacker))
            {
                if (botToPoint(EChars.Attacker, Input.MaxX, Input.MidY) < attDist)
                    kick(Input.MaxX, 35);
                else
                    goTo(EChars.Attacker, 100, 20);
            }
            else
            {
                Bot pro = getBot(EChars.Pro);
                goTo(EChars.Attacker, pro.X, pro.Y);
            }


            // Pro logic
            if (!input.Me.HasBall)
                goTo(EChars.Pro, true);
            else if (hasBall(EChars.Pro))
            {
                if (botToPoint(EChars.Pro, Input.MaxX, 45) < proDist)
                    kick(Input.MaxX, 45);
                else if (nearestEnemy(EChars.Pro) < bewareDist)
                {
                    if (input.Tick < 100) // TICK CHECK
                        pass(EChars.Attacker);
                    else
                    {
                        Bot pro = getBot(EChars.Pro);
                        kick(pro.X + 20, 45);
                    }
                }
                else
                {
                    Bot att = getBot(EChars.Attacker);
                    goTo(EChars.Pro, 100, 40);
                }
            }
        }

        private int getIdByName(EChars bot)
        {
            return (int)bot;
        }

        private void goTo(EChars bot, bool to = true)
        {
            goTo(bot, input.Ball.GetX(to), input.Ball.GetY(to));
        }

        private void goTo(EChars bot, double x, double y)
        {
            int id = getIdByName(bot);
            Bots[id].X = x;
            Bots[id].Y = y;
        }

        private double nearestEnemy(EChars bot)
        {
            double min = 1000,
                dist;
            Bot mine = getBot(bot);
            foreach (Bot enemy in input.Enemy.Bots)
            {
                dist = botToPoint(enemy, mine.X, mine.Y);
                if (dist < min)
                    min = dist;
            }
            return min;
        }

        private double botToPoint(EChars bot, bool to = true)
        {
            return botToPoint(bot, input.Ball.GetX(to), input.Ball.GetY(to));
        }

        private double botToPoint(EChars bot1, EChars bot2)
        {
            Bot bot2r = getBot(bot2);
            return botToPoint(bot1, bot2r.X, bot2r.Y);
        }

        private double botToPoint(EChars bot, double x, double y)
        {
            int id = getIdByName(bot);
            return Math.Sqrt(Math.Pow(Bots[id].X - x, 2) + Math.Pow(Bots[id].Y - y, 2));
        }

        private double botToPoint(Bot bot, double x, double y)
        {
            return Math.Sqrt(Math.Pow(bot.X - x, 2) + Math.Pow(bot.Y - y, 2));
        }

        private Bot getBot(EChars bot)
        {
            return Bots[(int)bot];
        }

        private void pass(EChars bot)
        {
            Bot receiver = getBot(bot);
            kick(receiver.X, receiver.Y);
        }

        private bool hasBall(EChars bot)
        {
            return getBot(bot).HasBall;
        }

        private void passToNearest(EChars sender, params EChars[] bots)
        {
            double min = 1000,
                dist = 0;
            EChars receiver = EChars.Attacker;
            foreach (EChars bot in bots)
            {
                dist = botToPoint(sender, bot);
                if (dist < min)
                {
                    receiver = bot;
                    min = dist;
                }
            }
            pass(receiver);
        }

        private void kick(double x, double y)
        {
            ToX = x;
            ToY = y;
        }
    }
}
