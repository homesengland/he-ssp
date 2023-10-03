using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
internal class ProjectDateTestData
{
    public static readonly ProjectDate CorrectDate = new(new DateTime(2022, 1, 31));

    public static readonly ProjectDate OtherCorrectDate = new(new DateTime(2022, 3, 31));
}
