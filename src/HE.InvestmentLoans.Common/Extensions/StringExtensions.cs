using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Extensions
{
    public static class StringExtensions
    {
        public static int? TryParseNullableInt(this string val)
        {
            int outValue;
            return int.TryParse(val, out outValue) ? (int?)outValue : null;
        }
    }
}
