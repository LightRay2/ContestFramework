using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Client
{

    class FrameworkSetting2{
        /// <summary>
        /// если 0, не ограничено
        /// </summary>
        public int PlayerMaxCount {get;set;}
        public bool AllowHumanPlayers{get;set;}

        public static int ControlPlayerLimitPerGame { get; set; }
    }
    class ContestApplication
    {
        static void RunMainFormFromAssembly(Assembly assembly)
        {
            

          //  var type = assembly.GetTypes()
          //      .First(t => t.Name == className);

          //  return Activator.CreateInstance(type);
        }

        public static void Run()
        {
            var assembly = Assembly.GetExecutingAssembly();

        }
       // public static InitWithMainUserControl<TGameParams>(FrameworkSetting settings, Action RunGame, Func<ControlStateBase, TGameParams> )
    }
}
