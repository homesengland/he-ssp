using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Projects.Entities;
public class ApplicationProjects
{
    private readonly List<Project> _projects;

    public ApplicationProjects(LoanApplicationId loanApplicationId, FrontDoorSiteId? frontDoorSiteId = null)
    {
        LoanApplicationId = loanApplicationId;
        _projects = [];
        AddEmptyProject(frontDoorSiteId);
    }

    public ApplicationProjects(LoanApplicationId loanApplicationId, IEnumerable<Project> projects)
    {
        LoanApplicationId = loanApplicationId;
        _projects = [];
        _projects.AddRange(projects);
    }

    public LoanApplicationId LoanApplicationId { get; }

    public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();

    public Project AddEmptyProject(FrontDoorSiteId? frontDoorSiteId = null)
    {
        var project = new Project(frontDoorSiteId);
        _projects.Add(project);

        return project;
    }

    public void DeleteProject(ProjectId projectId)
    {
        var projectToDelete = Projects.FirstOrDefault(p => p.Id == projectId) ?? throw new NotFoundException(nameof(Project), projectId);

        _projects.Remove(projectToDelete);

        projectToDelete.Delete();
    }

    internal Project Remove(ProjectId projectId)
    {
        var projectToRemove = Projects.FirstOrDefault(c => c.Id == projectId) ?? throw new NotFoundException(nameof(Project), projectId.Value);

        projectToRemove.Delete();

        return projectToRemove;
    }

    internal IList<Project> GetActiveProjects()
    {
        return Projects.Where(p => !p.IsSoftDeleted).ToList();
    }
}
