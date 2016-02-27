using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Game2D.Game.DrawableObjects.Animations;

namespace Game2D.Game.DrawableObjects
{
    class DynamicObject : IDrawable
    {
        Sprite currentSprite;
        Sprite temporarySprite;
        AnimationQueue movingAnimations = new AnimationQueue();
        AnimationQueue rotatingAnimations = new AnimationQueue();
        AnimationQueue framingAnimations = new AnimationQueue();
        public Vector2 Pos
        {
            get
            {
                return currentSprite.pos;
            }
            set
            {
                currentSprite.pos = value;
            }
        }
        public DynamicObject(Sprite sprite)
        {
            currentSprite = sprite;
        }
   
        public void Draw(ref Frame frame, double stage, int turnNumber)
        {
            temporarySprite = currentSprite;
            movingAnimations.ProcessAnimation(ref currentSprite, ref temporarySprite, stage, turnNumber);
            rotatingAnimations.ProcessAnimation(ref currentSprite, ref temporarySprite, stage, turnNumber);
            framingAnimations.ProcessAnimation(ref currentSprite, ref temporarySprite, stage, turnNumber);
            frame.Add(temporarySprite);
        }


        public void AddAnimation(double x, double y, double angle, double duration, double latency = 0, bool permanent = false)
        {
            AddMovingAnimation(x, y, duration, latency);
            AddRotatingAnimation(angle, duration, latency);
            AddFramingAnimation(duration, 0, latency);
            if (permanent)
            {
                movingAnimations.MakeFirstAnimationPermanent();
                rotatingAnimations.MakeFirstAnimationPermanent();
                framingAnimations.MakeFirstAnimationPermanent();
            }
        }

        public void AddMovingAnimation (double x, double y, double duration, double latency = 0, bool permanent = false)
        {
            movingAnimations.AddAnimation(new MovingAnimation(latency, duration,
                                                              new Point2(x, y)));
            if (permanent)
                movingAnimations.MakeFirstAnimationPermanent();
        }

        public void AddRotatingAnimation(double angle, double duration, double latency = 0, bool permanent = false)
        {
            rotatingAnimations.AddAnimation(new RotatingAnimation(latency, duration, angle));
            if (permanent)
                rotatingAnimations.MakeFirstAnimationPermanent();
        }

        public void AddFramingAnimation(double duration, int frame = 0, double latency = 0, bool permanent = false)
        {
            framingAnimations.AddAnimation(new FramingAnimation(latency, duration, frame));
            if (permanent)
                framingAnimations.MakeFirstAnimationPermanent();
        }

        public void ClearMovingAnimationQueue()
        {
            movingAnimations = new AnimationQueue();
        }

        public void ClearRotatingAnimationQueue()
        {
            rotatingAnimations = new AnimationQueue();
        }

        public void ClearFramingAnimationQueue()
        {
            framingAnimations = new AnimationQueue();
        }

        public void ClearAllAnimationQueues()
        {
            ClearFramingAnimationQueue();
            ClearMovingAnimationQueue();
            ClearRotatingAnimationQueue();
        }


        #region functions for effect support
        public double GetAnimationsStart()
        {
            double start = double.MaxValue;
            if (movingAnimations.HasAnimations())
                start = Math.Min(start, movingAnimations.StartStage);
            if (rotatingAnimations.HasAnimations())
                start = Math.Min(start, rotatingAnimations.StartStage);
            if (framingAnimations.HasAnimations())
                start = Math.Min(start, framingAnimations.StartStage);
            return start;
        }

        public double GetAnimationsFinish()
        {
            double finish = double.MinValue;
            if (movingAnimations.HasAnimations())
                finish = Math.Max(finish, movingAnimations.FinishStage);
            if (rotatingAnimations.HasAnimations())
                finish = Math.Max(finish, rotatingAnimations.FinishStage);
            if (framingAnimations.HasAnimations())
                finish = Math.Max(finish, framingAnimations.FinishStage);
            return finish;
        }

        public bool IsAnimationsFinished()
        {
            return movingAnimations.IsAnimationsFinished() &&
                   rotatingAnimations.IsAnimationsFinished() &&
                   framingAnimations.IsAnimationsFinished();
        }

        public bool HasAnimations()
        {
            return movingAnimations.HasAnimations() ||
                   rotatingAnimations.HasAnimations() ||
                   framingAnimations.HasAnimations();
        }
        #endregion
    }
}
