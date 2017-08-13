using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.DrawableObjects.Effects
{
    class EffectsManager : IDrawable
    {
        List<Effect> effects = new List<Effect>();

        public void Draw(ref Frame frame, double stage, int turnNumber)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Draw(ref frame, stage, turnNumber);
                if (effects[i].isFinished)
                    effects.Remove(effects[i]);
            }
        }

        public void AddEffect(Effect effect)
        {
            effects.Add(effect);
        }
    }
}
