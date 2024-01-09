using HE.Investments.Common.Contract;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
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
