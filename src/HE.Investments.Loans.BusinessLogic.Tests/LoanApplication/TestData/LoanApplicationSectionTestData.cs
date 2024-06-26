using HE.Investments.Common.Contract;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestData;
internal static class LoanApplicationSectionTestData
{
    public static readonly LoanApplicationSection CompletedSection = new(SectionStatus.Completed);

    public static readonly LoanApplicationSection IncompleteSection = new(SectionStatus.InProgress);

    public static readonly ProjectsSection CompletedProjectsSection = new([new(SectionStatus.Completed, ProjectIdTestData.AnyProjectId, HomesCountTestData.ValidHomesCount, new ProjectName("name"))]);

    public static readonly ProjectsSection IncompleteProjectsSection = new([new(SectionStatus.InProgress, ProjectIdTestData.AnyProjectId, HomesCountTestData.ValidHomesCount, new ProjectName("name"))]);
}
