using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    public class TimeUtil
    {
        public static double GetNowTime()
        {
            double ticks = DateTime.Now.ToUniversalTime().Ticks;
            double time = ticks / (double)10000000;
            return time;
            return (DateTime.Now.ToUniversalTime().Ticks - 637592694636867284) / (double)10000000;
        }
    }
}
