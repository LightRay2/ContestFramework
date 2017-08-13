using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.DrawableObjects.Animations
{
    
    class MovingAnimation : Animation
    {
        public Point2 Destination { get; set; } // relative to the initial value

        public MovingAnimation (double start, double duration, Point2 destination):
            base(start, duration)
        {
            Destination = destination;
        }

        protected override void _process(ref Sprite temporarySpriteState, double stage)
        {
            temporarySpriteState.pos.x = initialSpriteState.pos.x + stage * Destination.x;
            temporarySpriteState.pos.y = initialSpriteState.pos.y + stage * Destination.y;
        }

        protected override void _writeLastSpriteState(ref Sprite initialSpriteState, ref Sprite temporarySpriteState)
        {
            initialSpriteState.pos.x = initialSpriteState.pos.x + Destination.x;
            initialSpriteState.pos.y = initialSpriteState.pos.y + Destination.y;
        }
        
    }
}
