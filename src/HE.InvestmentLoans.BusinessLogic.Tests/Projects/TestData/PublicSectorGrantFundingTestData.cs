using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Tests.TestData;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
internal static class PublicSectorGrantFundingTestData
{
    public static readonly PublicSectorGrantFunding AnyGrantFunding = new(TextTestData.CorrectShortText, PoundsTestData.AnyAmount, TextTestData.CorrectShortText, TextTestData.CorrectLongText);
}
