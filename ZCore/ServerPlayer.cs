using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class ServerPlayer
    {
        public ServerPlayer()
        {
            ServerGameServerPlayer = new HashSet<ServerGameServerPlayer>();
        }
        public int Id { get; set; }
        public string fileName { get; set; }
        public string physicalFileName { get; set; }
        public int ServerUserId { get; set; }

        [JsonIgnore]
        public virtual ICollection<ServerGameServerPlayer> ServerGameServerPlayer { get; set; }

    }
}
