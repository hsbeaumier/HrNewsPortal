using System;
using SG.UnixTime;

namespace HrNewsPortal.Core.Extensions
{
    public static class UnixTimeExtension
    {
        #region method

        public static DateTime? GetDateTime<T>(this T unixTime)
        {
            DateTime? time = null;

            try
            {
                time = UnixTime.ToDateTime(unixTime, ConvertFormat.ToSeconds, Format.UTC);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return time;
        }

        #endregion
    }
}
