using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
public class ProjectBasicData : LoanApplicationSection
{
    public ProjectBasicData(SectionStatus status, ProjectId id, HomesCount? homesCount, ProjectName name)
        : base(status)
    {
        Id = id;
        HomesCount = homesCount;
        Name = name;
    }

    public ProjectId Id { get; }

    public HomesCount? HomesCount { get; }

    public ProjectName Name { get; }

    public static ProjectBasicData New(ProjectId id, HomesCount homesCount, ProjectName name) => new(SectionStatus.NotStarted, id, homesCount, name);
}
