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
