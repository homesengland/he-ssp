using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Common.Tests.TestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
internal static class PublicSectorGrantFundingTestData
{
    public static readonly PublicSectorGrantFunding AnyGrantFunding = new(TextTestData.CorrectShortText, PoundsTestData.AnyAmount, TextTestData.CorrectShortText, TextTestData.CorrectLongText);
}
