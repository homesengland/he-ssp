using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
internal sealed class StartDateTestData
{
    public static readonly StartDate CorrectDate = new(true, ProjectDateTestData.CorrectDate);

    public static readonly StartDate OtherCorrectDate = new(true, ProjectDateTestData.OtherCorrectDate);
}
