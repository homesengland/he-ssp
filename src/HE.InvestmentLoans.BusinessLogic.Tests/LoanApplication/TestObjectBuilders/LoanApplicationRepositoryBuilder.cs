using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
internal class LoanApplicationRepositoryBuilder
{
    private readonly Mock<ILoanApplicationRepository> _mock;

    public LoanApplicationRepositoryBuilder()
    {
        _mock = new Mock<ILoanApplicationRepository>();
    }

    public static LoanApplicationRepositoryBuilder New() => new();

    public LoanApplicationRepositoryBuilder ReturnsNoApplication()
    {
        _mock
            .Setup(m => m.GetLoanApplication(It.IsAny<LoanApplicationId>(), It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoanApplicationEntity)null!);

        return this;
    }

    //public LoanApplicationRepositoryBuilder ReturnsApplicationInDraftStatus()
    //{
    //    _mock
    //        .Setup(m => m.GetLoanApplication(It.IsAny<LoanApplicationId>(), It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()))
    //        .ReturnsAsync(loanapplicationtetsbu);

    //    return this;
    //}

    public ILoanApplicationRepository Build()
    {
        return _mock.Object;
    }

    public LoanApplicationRepositoryBuilder Register(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();

        registerDependency.RegisterDependency(mockedObject);

        return this;
    }
}
