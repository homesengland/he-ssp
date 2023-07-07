using System;

namespace HE.InvestmentLoans.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsBeforeOrEqualTo(this DateTime date, DateTime otherDate) => date <= otherDate;
    }
}
