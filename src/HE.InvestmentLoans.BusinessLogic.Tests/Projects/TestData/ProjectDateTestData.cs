using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Tests.TestData;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
internal class ProjectDateTestData
{
    public static readonly (string Year, string Month, string Day) IncorrectDateAsStrings = DateTimeTestData.IncorrectDateAsStrings;

    public static readonly (string Year, string Month, string Day) CorrectDateAsStrings = DateTimeTestData.CorrectDateAsStrings;

    public static readonly DateTime CorrectDateTime = new(2022, 1, 31);

    public static readonly ProjectDate CorrectDate = new(new DateTime(2022, 1, 31));

    public static readonly ProjectDate OtherCorrectDate = new(new DateTime(2022, 3, 31));
}
