using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
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

    public ApplicationProjects Build() => _applicationProjects;
}
