using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Exceptions;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
public class ApplicationProjects
{
    public ApplicationProjects(LoanApplicationId loanApplicationId)
    {
        LoanApplicationId = loanApplicationId;
        Projects = new List<Project>();
        AddProject();
    }

    public LoanApplicationId LoanApplicationId { get; }

    public IList<Project> Projects { get; }

    public IList<Project> ActiveProjects => Projects.Where(p => !p.IsSoftDeleted).ToList();

    public void AddProject()
    {
        const string newProjectName = "New project";

        var project = new Project
        {
            DefaultName = newProjectName,
        };
        Projects.Add(project);
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
