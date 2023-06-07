using System;

namespace HE.CRM.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime AddWorkdays(this DateTime date, int days)
        {
            if (days == 0)
                return date;

            int sign = Math.Sign(days);
            int unsignedDays = Math.Abs(days);
            for (int i = 0; i < unsignedDays; i++)
            {
                do
                {
                    date = date.AddDays(sign);
                } while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
            }
            return date;
        }
    }
}
