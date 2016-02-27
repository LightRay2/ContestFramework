using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.DrawableObjects.Animations
{
    class RotatingAnimation : Animation
    {
        double angle;
        public RotatingAnimation (double start, double duration, double angle)
            : base (start, duration)
        {
            this.angle = angle;
        }

        protected override void _process(ref Sprite temporarySpriteState, double stage)
        {
            temporarySpriteState.pos = new Vector2(temporarySpriteState.pos.x,
                                                   temporarySpriteState.pos.y,
                                                   initialSpriteState.pos.angleDeg + stage * angle);
        }

        protected override void _writeLastSpriteState(ref Sprite initialSpriteState, ref Sprite temporarySpriteState)
        {
            initialSpriteState.pos = new Vector2(initialSpriteState.pos.x,
                                                 initialSpriteState.pos.y,
                                                 initialSpriteState.pos.angleDeg + angle);
        }
    }
}
