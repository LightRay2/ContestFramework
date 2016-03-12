using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web;
using Framework;

[assembly: OwinStartup(typeof(Server.OWINStartup))]

namespace Server
{
    public class OWINStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
            TimerActions.Start();
            
            //using (var db = new MainContext())
            //{
            //    var serverUser = new ServerUser{
            //         Fio="Землянухин Михаил",
            //         Password = "rhbcnfkk",
            //         RegistrationDate = DateTime.Now,
            //         Name = "LightRay",
            //         IsAdmin = true
            //    };
            //    db.ServerUser.Add(serverUser);
            //    serverUser = new ServerUser
            //    {
            //        Fio = "Землянухин Михаил",
            //        Password = "rhbcnfkk",
            //        RegistrationDate = DateTime.Now,
            //        Name = "Inspiration",
            //        IsAdmin = false
            //    };
            //    db.ServerUser.Add(serverUser);
            //    db.SaveChanges();
            //}
        }
    }
}
