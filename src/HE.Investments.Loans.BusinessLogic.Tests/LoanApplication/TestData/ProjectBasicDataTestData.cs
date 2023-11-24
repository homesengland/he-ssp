using HE.Investments.Common.Domain;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestData;
internal static class ProjectBasicDataTestData
{
    public static readonly ProjectBasicData IncompleteProject = new(SectionStatus.InProgress, ProjectIdTestData.AnyProjectId, HomesCountTestData.ValidHomesCount, new ProjectName("name"));

    public static readonly ProjectBasicData CompleteProject = new(SectionStatus.Completed, ProjectIdTestData.AnyProjectId, HomesCountTestData.ValidHomesCount, new ProjectName("name"));
}
