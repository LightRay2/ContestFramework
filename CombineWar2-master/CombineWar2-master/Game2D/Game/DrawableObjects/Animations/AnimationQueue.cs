using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Game2D.Game.DrawableObjects.Animations
{
    class AnimationQueue
    {
      
        public double StartStage
        {
            get
            {
                Debug.Assert(!Utility.doubleEqual(startStage, -1));
                if (currentAnimation != null && currentAnimation.IsStarted)
                    return 0.0;
                return startStage;
            }
        }

        public double FinishStage
        {
            get
            {
                Debug.Assert(!Utility.doubleEqual(finishStage, -1));
                return finishStage;
            }
        }
        
        public void AddAnimation(Animation animation)
        {
            if (queue.Count == 0 && currentAnimation == null)
            {
                startStage = animation.StartStage;
                queue.Enqueue(animation);
                finishStage = animation.FinishStage;
            }
            else
            {
                double currentFinishStage;
                if (queue.Count == 0)
                    currentFinishStage = currentAnimation.FinishStage;
                else
                    currentFinishStage = queue.Last().FinishStage;
                currentFinishStage = Utility.GetFractionalPart(currentFinishStage);
                double lastAnimationStart = Utility.GetFractionalPart(currentFinishStage + animation.StartStage);
                animation.StartStage = lastAnimationStart;
                queue.Enqueue(animation);
                finishStage = animation.FinishStage;
            }
        }

        public void ProcessAnimation(ref Sprite initialSprite, ref Sprite temporarySprite, double stage, int turnNumber)
        {
            if (currentAnimation == null)
            {
                if (queue.Count > 0)
                    currentAnimation = queue.Dequeue();
                else
                    return;
            }

            if (currentAnimation.Duration == 0.0)
            {
                currentAnimation = null;
                ProcessAnimation(ref initialSprite, ref temporarySprite, stage, turnNumber);
                return;
            }
            currentAnimation.Process(ref initialSprite, ref temporarySprite, stage, turnNumber);
            if (currentAnimation.IsFinished)
            {
                currentAnimation = null;
                ProcessAnimation(ref initialSprite, ref temporarySprite, stage, turnNumber);
            }
        }

        public bool IsAnimationsFinished()
        {
            return (queue.Count == 0) && currentAnimation == null;
        }

        public bool HasAnimations()
        {
            return (currentAnimation != null) || (queue.Count > 0);
        }

        public void MakeFirstAnimationPermanent()
        {
            queue.Peek().MakePermanent();
        }

        public double CurrentInnerStage
        {
            get
            {
                return currentAnimation.InnerAnimationStage;
            }
        }

        double startStage = -1;
        double finishStage = -1;
        Queue<Animation> queue = new Queue<Animation>();
        Animation currentAnimation;
    }
}
