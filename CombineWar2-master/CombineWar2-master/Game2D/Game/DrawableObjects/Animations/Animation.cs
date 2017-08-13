using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Game2D.Game.DrawableObjects.Animations
{
    ///<summary>
    ///Abstract animation class.
    ///Inheriting classes should implement _process and _writeLastSpriteState functions
    ///</summary>
    abstract class Animation
    {
        public bool IsStarted
        {
            get
            {
                return started;
            }
        }

        public bool IsFinished
        {
            get
            {
                return finished;
            }
        }

        public double InnerAnimationStage
        {
            get
            {
                return innerAnimationStage;
            }
        }

        // stage of animation beginning relative to the last animation or 0.0 in case of the first animation
        // assigning to this property lead to changing of finishStage according to duration
        public double StartStage
        {
            get
            {
                return startStage;
            }
            set
            {
                Debug.Assert(Utility.inClosedInterval(value, 0.0, 1.0));
                startStage = value;
                finishStage = startStage + duration;
            }
        }

        public double Duration  // in turns (stages)
        {
            get
            {
                return duration;
            }
        }

        public double FinishStage // stage of animation finishing
        {
            get
            {
                return finishStage;
            }
        }

       
        public Animation(double start, double duration)
        {
            Debug.Assert(Utility.inClosedInterval(start, 0.0, 1.0));
            Debug.Assert(Utility.doubleGreaterOrEqual(duration, 0));
            //this.duration = Math.Max(0.0000000001,duration);
            this.duration = duration;
            StartStage = start;
        }

        public void Process(ref Sprite previousSpriteState, ref Sprite temporarySpriteState, double stage, int turnNumber)
        {
            if (initialTurnNumber != -1)
            {
                if (initialTurnNumber != turnNumber || turnCounter == 0)
                    turnCounter = turnNumber - initialTurnNumber;
            }
            Debug.Assert(!finished);
            if (!started && isStarted(stage))
            {
                initialSpriteState = previousSpriteState;
                initialTurnNumber = turnNumber;
            }
            //Debug.Assert(!(turnNumber == 200 && initialSpriteState.name == ESprite.explosion));
            if (!started)
                return;
            
            if (permanent)
            {
                _process(ref temporarySpriteState, stage);
                /*if (Utility.doubleEqual(stage, 1.0) && !Utility.doubleEqual(stage, lastStage))
                    _writeLastSpriteState(ref previousSpriteState, ref temporarySpriteState);
                lastStage = stage;*/
                return;
            }
            innerAnimationStage = (stage - StartStage + turnCounter) / Duration;
            if (Utility.doubleEqual(StartStage - turnCounter, stage))
                innerAnimationStage = 0.0;
            if (Utility.doubleGreaterOrEqual(innerAnimationStage, 1.0))
                innerAnimationStage = 1.0;
            if (Utility.doubleLess(innerAnimationStage, 0.0))
            {
                Debug.Assert(false /*|| turnNumber == Const.NumberOfTurns*/);
               /* if (turnNumber == Const.NumberOfTurns)
                {
                    finished = true;
                }*/
                return;
            }

            _process(ref temporarySpriteState, innerAnimationStage);
            
            if (!finished && isFinished(stage))
            {
                _writeLastSpriteState(ref previousSpriteState, ref temporarySpriteState);
            }
            
            lastStage = stage;
        }

        double innerAnimationStage;
        double duration;
        double startStage;
        double finishStage;
        double lastStage = -1;
        int turnCounter = 0;
        bool started = false;
        bool finished = false;
        bool permanent = false;
        protected Sprite initialSpriteState;
        int initialTurnNumber = -1;
       
        public void MakePermanent()
        {
            permanent = true;
        }

        /// <summary>
        /// Computes current state of sprite using stage and initialSpriteState
        /// and writes it to temporarySpriteState.
        /// WARNING: write only those parameters which your animation changes.
        /// </summary>
        /// <param name="temporarySpriteState"></param>
        /// <param name="stage"></param>
        protected abstract void _process(ref Sprite temporarySpriteState, double stage);

        /// <summary>
        /// Writes changes to the InitialSpriteState when animation finishes.
        /// WARNING: write only those parameters which your animation changes.
        /// </summary>
        /// <param name="initialSpriteState"></param>
        /// <param name="temporarySpriteState"></param>
        protected abstract void _writeLastSpriteState(ref Sprite initialSpriteState, ref Sprite temporarySpriteState); 

        protected bool isStarted(double stage)
        {
            if (permanent)
            {
                started = true;
                return true;
            }
            if (started)
                return true;
            if (Utility.doubleLessOrEqual(StartStage - turnCounter, stage))
            {
                started = true;
                return true;
            }
            return false;
        }

        bool checkStart(double stage)
        {
            if (Utility.doubleLessOrEqual(StartStage - turnCounter, stage))
            {
                return true;
            }
            return false;
        }

        protected bool isFinished(double stage)
        {
            if (finished)
                return true;
            if (Utility.doubleLessOrEqual(FinishStage - turnCounter, stage))
            {
                finished = true;
                return true;
            }
            return false;
        }
    }
}
