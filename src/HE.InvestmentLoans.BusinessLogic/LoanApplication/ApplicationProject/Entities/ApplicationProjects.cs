using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
public class ApplicationProjects
{
    public ApplicationProjects(LoanApplicationId loanApplicationId)
    {
        LoanApplicationId = loanApplicationId;
        Projects = new List<Project>();
        AddProject();
    }

    public ApplicationProjects(LoanApplicationId loanApplicationId, IEnumerable<Project> projects)
    {
        LoanApplicationId = loanApplicationId;
        Projects = projects.ToList();
    }

    public LoanApplicationId LoanApplicationId { get; }

    public IList<Project> Projects { get; }

    public IList<Project> ActiveProjects => Projects.Where(p => !p.IsSoftDeleted).ToList();

    public ProjectId AddProject()
    {
        var project = new Project();
        Projects.Add(project);

        return project.Id!;
    }

    public void UpdateProject(Project project)
    {
        var projectToUpdate = Projects.FirstOrDefault(p => p.Id == project.Id) ?? throw new NotFoundException(nameof(Project).ToString(), project.Id!);

        projectToUpdate = project;
    }

    public void DeleteProject(ProjectId projectId)
    {
        var projectToDelete = Projects.FirstOrDefault(p => p.Id == projectId) ?? throw new NotFoundException(nameof(Project).ToString(), projectId);
        projectToDelete.MarkAsDeleted();
    }
}
