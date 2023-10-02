using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Moq;
using Xunit.Sdk;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;

internal sealed class ApplicationProjectsRepositoryBuilder : IDependencyTestBuilder<IApplicationProjectsRepository>
{
    private readonly Mock<IApplicationProjectsRepository> _mock;

    private LoanApplicationId _applicationId;

    public ApplicationProjectsRepositoryBuilder()
    {
        _mock = new Mock<IApplicationProjectsRepository>();
    }

    public static ApplicationProjectsRepositoryBuilder New() => new();

    public ApplicationProjectsRepositoryBuilder ReturnsNoProjects()
    {
        _mock
            .Setup(m => m.GetById(It.IsAny<LoanApplicationId>(), It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApplicationProjects)null!);

        return this;
    }

    public ApplicationProjectsRepositoryBuilder For(LoanApplicationId applicationId)
    {
        _applicationId = applicationId;

        return this;
    }

    public ApplicationProjectsRepositoryBuilder Returns(ApplicationProjects applicationProjects)
    {
        if (_applicationId.IsNotProvided())
        {
            throw new InvalidOperationException($"Expected applicationId is not provided. Consider using {nameof(For)}() method");
        }

        _mock
            .Setup(m => m.GetById(_applicationId, It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(applicationProjects);

        return this;
    }

    public IApplicationProjectsRepository Build()
    {
        return _mock.Object;
    }
}
