using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Framework
{
    public enum EServerGameState { waitForStart, running, finish}
    public class ServerGame
    {
        public ServerGame(){
            ServerGameServerPlayer = new HashSet<ServerGameServerPlayer>();
    }
        public int Id { get; set; }
        public int RoomId { get; set; }

        public string Name { get; set; }
        public DateTime DateStart { get; set; }
        public EServerGameState state { get; set; }

        /// <summary>
        /// что-то, что сохраняется на каждом ходу и обновляется у клиентов, например , счет и текущий номер хода
        /// </summary>
        public string JsonCurrentGameInfo { get; set; }
        /// <summary>
        /// заполняем только после завершения игры
        /// </summary>
        public string JsonGameData { get; set; }

        [NotMapped]
        public List<ServerPlayer> players { get; set; }

        [NotMapped]
        public string participants { get { return ""; } } //todo // string.Join(" - ", players.Select(x => x.fileName)); } }
       [JsonIgnore]
        public virtual ICollection<ServerGameServerPlayer> ServerGameServerPlayer { get; set; }

        [NotMapped]
       public FormMainSettings StartSettings { get; set; }
        [JsonIgnore]
        public  string StartSettingsJson{
            get{ return StartSettings == null? null: JsonConvert.SerializeObject(  StartSettings);}
            set { StartSettings = value == null? null: JsonConvert.DeserializeObject<FormMainSettings>(value); }
        }
    }
}
