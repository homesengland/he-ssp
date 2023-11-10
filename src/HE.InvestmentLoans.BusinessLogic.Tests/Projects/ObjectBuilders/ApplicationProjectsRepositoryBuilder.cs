using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Extensions;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;

internal sealed class ApplicationProjectsRepositoryBuilder : IDependencyTestBuilder<IApplicationProjectsRepository>
{
    private readonly Mock<IApplicationProjectsRepository> _mock;

    private LoanApplicationId _applicationId;
    private ProjectId _projectId;

    public ApplicationProjectsRepositoryBuilder()
    {
        _mock = new Mock<IApplicationProjectsRepository>();
    }

    public static ApplicationProjectsRepositoryBuilder New() => new();

    public ApplicationProjectsRepositoryBuilder ReturnsNoProjects()
    {
        _mock
            .Setup(m => m.GetAllAsync(It.IsAny<LoanApplicationId>(), It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApplicationProjects)null!);

        return this;
    }

    public ApplicationProjectsRepositoryBuilder For(LoanApplicationId applicationId)
    {
        _applicationId = applicationId;

        return this;
    }

    public ApplicationProjectsRepositoryBuilder ForProject(ProjectId projectId)
    {
        _projectId = projectId;

        return this;
    }

    public ApplicationProjectsRepositoryBuilder ReturnsAllProjects(ApplicationProjects applicationProjects)
    {
        if (_applicationId.IsNotProvided())
        {
            throw new InvalidOperationException($"Expected applicationId is not provided. Consider using {nameof(For)}() method");
        }

        _mock
            .Setup(m => m.GetAllAsync(_applicationId, It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(applicationProjects);

        return this;
    }

    public ApplicationProjectsRepositoryBuilder ReturnsOneProject(Project project)
    {
        if (_projectId.IsNotProvided())
        {
            throw new InvalidOperationException($"Expected projectId is not provided. Consider using {nameof(ForProject)}() method");
        }

        _mock
            .Setup(m => m.GetByIdAsync(_projectId, It.IsAny<UserAccount>(), It.IsAny<ProjectFieldsSet>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        return this;
    }

    public IApplicationProjectsRepository Build()
    {
        return _mock.Object;
    }
}
