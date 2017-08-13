using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.DrawableObjects.Animations
{
    class FramingAnimation : Animation
    {
        int frame;

        public FramingAnimation (double start, double duration, int frame)
            : base (start, duration)
        {
            this.frame = frame;
        }

        protected override void _process(ref Sprite temporarySpriteState, double stage)
        {
            temporarySpriteState.frame = (int)(Math.Round(initialSpriteState.frameCount * stage) - 1) % initialSpriteState.frameCount;
            if (temporarySpriteState.frame == -1)
                temporarySpriteState.frame = 0;
        }

        protected override void _writeLastSpriteState(ref Sprite initialSpriteState, ref Sprite temporarySpriteState)
        {
            initialSpriteState.frame = temporarySpriteState.frame;
        }
    }
}
