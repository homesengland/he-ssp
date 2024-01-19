using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
internal static class StartDateTestData
{
    public static readonly StartDate CorrectDate = new(true, ProjectDateTestData.CorrectDate);

    public static readonly StartDate OtherCorrectDate = new(true, ProjectDateTestData.OtherCorrectDate);
}
