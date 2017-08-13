using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DenisovFootballPowerfulStrategy
{
    public class Player
    {
        public Points current { get; set; }
        public bool IsOur { get; set; }

        public Player(Points current, bool IsOur)
        {
            this.current = current;
            this.IsOur = IsOur;
        }
    }
}
