namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
public class ProjectsSection
{
    private readonly List<ProjectBasicData> _projects;

    public ProjectsSection(IEnumerable<ProjectBasicData> projects)
    {
        _projects = new List<ProjectBasicData>();

        _projects.AddRange(projects);
    }

    public IReadOnlyCollection<ProjectBasicData> Projects => _projects.AsReadOnly();

    public static ProjectsSection Empty() => new(Enumerable.Empty<ProjectBasicData>());

    public bool IsCompleted() => _projects.Any() && _projects.All(project => project.IsCompleted());

    public int TotalHomesBuilt() => _projects.Aggregate(0, (sum, project) => sum += project?.HomesCount?.AsInt() ?? 0);
}
