using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Common.StaticClass
{
    public class SimpleMethods
    {




        public static DateTime TimeToTehran(DateTime DateValue)
        {
            TimeZoneInfo tehranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");

            // Ensure the DateValue is treated as UTC
            DateValue = DateTime.SpecifyKind(DateValue, DateTimeKind.Utc);

            DateTime tehranTime = TimeZoneInfo.ConvertTimeFromUtc(DateValue, tehranTimeZone);

            return tehranTime;
        }


        public static DateTime ConvertToTehran(long DateValue)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateTime = epoch.AddMilliseconds(DateValue);
            TimeZoneInfo tehranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");
            var tehrantime= TimeZoneInfo.ConvertTimeFromUtc(dateTime, tehranTimeZone);
            return tehrantime;




        }

        //public static long DateTimeToTimeStamp(DateTime dateTime)
        //{
        //    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //    TimeSpan elapsedTime = dateTime - TimeToTehran(epoch);
        //    long timestamp = (long)elapsedTime.TotalMilliseconds;
        //    return timestamp;
        //}
        public static long DateTimeToTimeStamp(DateTime dateTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan elapsedTime = dateTime.ToUniversalTime() - epoch;
            long timestamp = (long)elapsedTime.TotalMilliseconds;
            return timestamp;
        }
        public static DateTime InsertDateTime(long DateValue)
        {
            return ConvertToTehran(DateValue);
        }

        public static DateTime InsertDateTime(string DateValue)
        {
            return ConvertToTehran(long.Parse(DateValue));
        }


        


    }
}
