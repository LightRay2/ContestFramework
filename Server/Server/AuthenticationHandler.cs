using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server
{
    public class AuthenticationHandler
    {
        public Client TryGetClient(string hubConnectionId)
        {
            Client t;
            if (!Manager.ClientList.TryGetValue(hubConnectionId, out t))
                return null;
            return t;
        }

        public static bool Authorize(string hubConnectionId, string login, string password)
        {
            using (var db = new MainContext())
            {
                var databaseUser = db.ServerUser.FirstOrDefault(x => x.Name.ToLower() == login.ToLower() && x.Password == password);
                if (databaseUser == null)
                    return false;
                var client = new Client
                {
                    id = databaseUser.Id,
                    Name = databaseUser.Name,
                    IsAdmin = databaseUser.IsAdmin
                };
                return Manager.ClientList.TryAdd(hubConnectionId, client);
            }
        }
    }
}