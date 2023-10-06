using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Generic;
using StackExchange.Redis;

namespace HE.InvestmentLoans.Common.Tests.TestData;
public static class PoundsTestData
{
    public const string CorrectAmountAsString = "9.9";

    public const string IncorrectAmountAsString = "asd";

    public const string CorrectAmountDisplay = "£9.9";

    public const decimal CorrectAmount = 9.9M;

    public static readonly Pounds AnyAmount = new(CorrectAmount);
}
