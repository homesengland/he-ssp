using System.Globalization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Pipelines.Sockets.Unofficial.Arenas;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
public class ApplicationProjects
{
    public ApplicationProjects(LoanApplicationId loanApplicationId)
    {
        LoanApplicationId = loanApplicationId;
        Projects = new List<Project>();
        AddDefaultProject();
    }

    public LoanApplicationId LoanApplicationId { get; }

    public IList<Project> Projects { get; }

    public IList<Project> ActiveProjects => Projects.Where(p => !p.IsSoftDeleted).ToList();

    public void AddAnotherProject()
    {
        var project = new Project
        {
            Name = GenerateNextProjectName(),
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

    private void AddDefaultProject()
    {
        var project = new Project
        {
            Id = new ProjectId(Guid.NewGuid()),
            Name = "Project 01",
        };

        Projects.Add(project);
    }

    private string GenerateNextProjectName()
    {
        return $"Project {(Projects.Count + 1).ToString("D2", CultureInfo.InvariantCulture)}";
    }
}
