using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHelper
{
    public class Log
    {
        public static void CheckIfDebug(params bool[] condition){
            if(Debugger.IsAttached && condition.Any(x=>x==false))
            {
                throw new Exception();
            }
        }
    }
}
