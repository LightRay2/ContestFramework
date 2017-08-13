using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D.Game.DataClasses
{
    using Units;
    class Player
    {
        public string programAddress = null;  //если null, то это человек играет клавиатурой
        public OwnerType Owner;
        public int Score { get; set; }
        public int Money { get; set; }
        public Unit[] Units = new Unit[Const.NumberOfLines];

        //for turnreceiver
        public int moneySpent = 0;
        public string Memory;

    }


}
