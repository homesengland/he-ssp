using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using static HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData.ProjectDateTestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
internal static class StartDateTestData
{
    public static readonly StartDate CorrectDate = new(true, CorrectDateAsStrings.Day, CorrectDateAsStrings.Month, CorrectDateAsStrings.Year);

    public static readonly StartDate OtherCorrectDate = new(true, OtherCorrectDateAsStrings.Day, OtherCorrectDateAsStrings.Month, OtherCorrectDateAsStrings.Year);
}
