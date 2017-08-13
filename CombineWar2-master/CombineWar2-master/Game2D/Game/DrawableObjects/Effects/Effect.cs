using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Game2D.Game.DrawableObjects.Effects
{
    class Effect : IDrawable
    {
        Queue<DynamicObject> anims = new Queue<DynamicObject>();
        DynamicObject currAnim = null;
        public bool isFinished = false;
        double lastFinish;

        Vector2 lastPos = new Vector2(-1, -1);

        public void Draw(ref Frame frame, double stage, int turnNumber)
        {
            if (currAnim == null)
            {
                if (anims.Count == 0)
                {
                    isFinished = true;
                    return;
                }
                currAnim = anims.Dequeue();
                if (!Utility.doubleEqual(lastPos.x, -1))
                    currAnim.Pos = lastPos;
            }
            
            if (Utility.doubleGreaterOrEqual(stage, currAnim.GetAnimationsStart()))
            {
                currAnim.Draw(ref frame, stage, turnNumber);
            }
            if (currAnim.CurrentSprite == ESprite.explosionNuclear && !currAnim.IsAnimationsFinished())
            {
                double pointSz = Const.TileHeight / 15;
                ESprite sprite = ESprite.explosionPoint;
                double x = currAnim.Pos.x - Const.TileWidth * 2.5;
                double y = currAnim.Pos.y - Const.TileHeight * 2.5;
                double w = Const.TileWidth * 5;
                double h = Const.TileHeight * 5;
                double alpha = -2 * Math.Pow((currAnim.CurrentInnerStage - 0.5), 2) + 1;
                frame.Add(new Sprite(sprite, w, pointSz, new Vector2(x + w / 2, y)) { Alpha = alpha });
                frame.Add(new Sprite(sprite, pointSz, h, new Vector2(x, y + h / 2)) { Alpha = alpha });
                frame.Add(new Sprite(sprite, w, pointSz, new Vector2(x + w / 2, y + h)) { Alpha = alpha });
                frame.Add(new Sprite(sprite, pointSz, h, new Vector2(x + w, y + h / 2)) { Alpha = alpha });
            }
            if (currAnim.IsAnimationsFinished())
            {
                lastPos = currAnim.Pos;
                currAnim = null;
                Draw(ref frame, stage, turnNumber);
            }
        }

        public void AddNextStep(DynamicObject nextStep)
        {
            Debug.Assert(!nextStep.HasAnimations());
            anims.Enqueue(nextStep);
        }

        public void AddAnimationToLastStep(double x, double y, double angle, double duration, double latency = 0)
        {
            double start = Utility.GetFractionalPart(lastFinish) + latency;
            anims.Last().AddAnimation(x, y, angle, duration, start);
            lastFinish = anims.Last().GetAnimationsFinish();
        }
    }
}
