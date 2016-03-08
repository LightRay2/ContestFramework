using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class ServerGame
    {
        public int Id { get; set; }
        public int RoomId { get; set; }

        public string Name { get; set; }
        public DateTime DateStart { get; set; }

        /// <summary>
        /// что-то, что сохраняется на каждом ходу и обновляется у клиентов, например , счет и текущий номер хода
        /// </summary>
        public string JsonCurrentGameInfo { get; set; }
        /// <summary>
        /// заполняем только после завершения игры
        /// </summary>
        public string JsonGameData { get; set; }
    }
}
