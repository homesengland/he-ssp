using System.Globalization;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

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

    public Project AddAnotherProject()
    {
        var project = new Project
        {
            Name = GenerateNextProjectName(),
        };

        Projects.Add(project);

        return project;
    }

    private void AddDefaultProject()
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Project 01",
        };

        Projects.Add(project);
    }

    private string GenerateNextProjectName()
    {
        return $"Project {(Projects.Count + 1).ToString("D2", CultureInfo.InvariantCulture)}";
    }
}
