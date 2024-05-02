using HE.Investments.Common.Extensions;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
public class ProjectsSection
{
    private readonly List<ProjectBasicData> _projects;

    public ProjectsSection(IEnumerable<ProjectBasicData> projects)
    {
        _projects = [];
        _projects.AddRange(projects);
    }

    public IReadOnlyCollection<ProjectBasicData> Projects => _projects.AsReadOnly();

    public static ProjectsSection Empty() => new([]);

    public bool IsCompleted() => _projects.IsNotEmpty() && _projects.TrueForAll(project => project.IsCompleted());

    public int TotalHomesBuilt() => _projects.Aggregate(0, (sum, project) => sum += project?.HomesCount?.AsInt() ?? 0);
}
