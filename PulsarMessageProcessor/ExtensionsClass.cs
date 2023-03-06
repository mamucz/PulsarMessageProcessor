using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessorLib
{
    //Extension pro vytvoření DateTime z UnixTimeStamp
    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(this DateTime dateTime, long unixTimeSeconds)
        {
            return UnixEpoch.AddMilliseconds(unixTimeSeconds).ToLocalTime();
        }

        public static long ToUnixTime(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalMilliseconds;
        }
    }   
}
