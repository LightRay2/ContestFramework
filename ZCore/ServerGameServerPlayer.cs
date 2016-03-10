using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class ServerGameServerPlayer
    {
        public int id { get; set; }
        public int ServerGameId { get; set; }
        public int ServerPlayerId { get; set; }
        public int Number { get; set; }

        public virtual ServerGame ServerGame { get;set; }
        public virtual ServerPlayer ServerPlayer { get; set; }
    }
}
