using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace client_server
{
    public class ShutdownManager : Opener
    {

        
        public enum ShutdownType{
            Shutdown,
            Restart,
            Hibernate
        }

        public void Shutdown(ShutdownType type, UInt32 seconds = 0)
        {
            string typeString = new Func<string>(() =>
            {
                return type == ShutdownType.Shutdown ? "-s" :
                       type == ShutdownType.Restart ? "-r" :
                       type == ShutdownType.Hibernate ? "-h" :
                       throw new ArgumentException($"Theres no valid argument for shutdown called {type}, try -s, -r or -h");
                       
            })();
            if (seconds != 0)
            {
                Process.Start($"shutdown {typeString} -t{seconds}");
                return;
            }
            Process.Start($"shutdown {typeString}");
        }

    }
}
