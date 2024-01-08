using HE.Investments.Common.Exceptions;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Projects.Entities;
public class ApplicationProjects
{
    private readonly List<Project> _projects;

    public ApplicationProjects(LoanApplicationId loanApplicationId)
    {
        LoanApplicationId = loanApplicationId;
        _projects = new List<Project>();

        AddEmptyProject();
    }

    public ApplicationProjects(LoanApplicationId loanApplicationId, IEnumerable<Project> projects)
    {
        LoanApplicationId = loanApplicationId;

        _projects = new List<Project>();

        _projects.AddRange(projects);
    }

    public LoanApplicationId LoanApplicationId { get; }

    public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();

    public IList<Project> ActiveProjects => Projects.Where(p => !p.IsSoftDeleted).ToList();

    public Project AddEmptyProject()
    {
        var project = new Project();
        _projects.Add(project);

        return project;
    }

    public void UpdateProject(Project project)
    {
        project = Projects.FirstOrDefault(p => p.Id == project.Id) ?? throw new NotFoundException(nameof(Project).ToString(), project.Id!);
    }

    public void DeleteProject(ProjectId projectId)
    {
        var projectToDelete = Projects.FirstOrDefault(p => p.Id == projectId) ?? throw new NotFoundException(nameof(Project).ToString(), projectId);

        _projects.Remove(projectToDelete);

        projectToDelete.Delete();
    }

    internal Project Remove(ProjectId projectId)
    {
        var projectToRemove = Projects.FirstOrDefault(c => c.Id == projectId) ?? throw new NotFoundException(nameof(Project), projectId.Value);

        projectToRemove.Delete();

        return projectToRemove;
    }
}
