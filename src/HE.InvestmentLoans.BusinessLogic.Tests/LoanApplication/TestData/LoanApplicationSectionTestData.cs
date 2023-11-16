using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
internal static class LoanApplicationSectionTestData
{
    public static readonly LoanApplicationSection CompletedSection = new(SectionStatus.Completed);

    public static readonly LoanApplicationSection IncompleteSection = new(SectionStatus.InProgress);

    public static readonly ProjectsSection CompletedProjectsSection = new(new List<ProjectBasicData> { new ProjectBasicData(SectionStatus.Completed, ProjectIdTestData.AnyProjectId, HomesCountTestData.ValidHomesCount, new ProjectName("name")) });

    public static readonly ProjectsSection IncompleteProjectsSection = new(new List<ProjectBasicData> { new ProjectBasicData(SectionStatus.InProgress, ProjectIdTestData.AnyProjectId, HomesCountTestData.ValidHomesCount, new ProjectName("name")) });
}
