using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
internal static class ProjectBasicDataTestData
{
    public static readonly ProjectBasicData IncompleteProject = new(SectionStatus.InProgress, ProjectIdTestData.AnyProjectId, HomesCountTestData.ValidHomesCount, new ProjectName("name"));

    public static readonly ProjectBasicData CompleteProject = new(SectionStatus.Completed, ProjectIdTestData.AnyProjectId, HomesCountTestData.ValidHomesCount, new ProjectName("name"));
}
