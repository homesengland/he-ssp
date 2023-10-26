using System.Security.Policy;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
internal sealed class ApplicationProjectsBuilder
{
    private readonly ApplicationProjects _applicationProjects;

    public ApplicationProjectsBuilder()
    {
        _applicationProjects = new ApplicationProjects(LoanApplicationId.From(Guid.NewGuid()));
    }

    public static ApplicationProjectsBuilder New() => new();

    public static ApplicationProjects EmptyProjects() => new ApplicationProjectsBuilder()
        .WithoutProjects()
        .Build();

    public ApplicationProjectsBuilder WithoutProjects()
    {
        return WithoutDefaultProject();
    }

    public ApplicationProjectsBuilder WithoutDefaultProject()
    {
        var projectToDeleteIds = _applicationProjects.Projects.Select(c => c.Id).ToList();

        foreach (var projectId in projectToDeleteIds)
        {
            _applicationProjects.DeleteProject(projectId);
        }

        return this;
    }

    public ApplicationProjectsBuilder WithDefaultProject()
    {
        return this;
    }

    public ApplicationProjectsBuilder WithProjectWithPlanningReferenceNumber(string number)
    {
        var project = _applicationProjects.AddEmptyProject();

        project.ProvidePlanningReferenceNumber(new PlanningReferenceNumber(true, number));

        return this;
    }

    public ApplicationProjects Build() => _applicationProjects;
}
