using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;

public class LoanApplicationRepositoryTestBuilder
{
    private readonly Mock<ILoanApplicationRepository> _mock;

    private LoanApplicationRepositoryTestBuilder()
    {
        _mock = new Mock<ILoanApplicationRepository>();
    }

    public static LoanApplicationRepositoryTestBuilder New() => new();

    public LoanApplicationRepositoryTestBuilder ReturnLoanApplication(LoanApplicationId loanApplicationId, UserAccount userAccount, LoanApplicationEntity loanApplication)
    {
        _mock.Setup(x => x
                    .GetLoanApplication(loanApplicationId, userAccount, CancellationToken.None))
                .ReturnsAsync(loanApplication);

        return this;
    }

    public ILoanApplicationRepository Build()
    {
        return _mock.Object;
    }

    public Mock<ILoanApplicationRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
