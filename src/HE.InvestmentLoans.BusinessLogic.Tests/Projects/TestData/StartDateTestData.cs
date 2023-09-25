using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
internal class StartDateTestData
{
    public static readonly (string Day, string Month, string Year) IncorrectDateAsStrings = ("32", "1", "2022");

    public static readonly (string Day, string Month, string Year) CorrectDateAsStrings = ("31", "1", "2022");

    public static readonly StartDate CorrectDate = new(true, new DateTime(2022, 1, 31));

    public static readonly StartDate OtherCorrectDate = new(true, new DateTime(2022, 3, 31));
}
