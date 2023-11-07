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
    private readonly Mock<ICanSubmitLoanApplication> _iCanSubmitmock;

    private LoanApplicationRepositoryTestBuilder()
    {
        _mock = new Mock<ILoanApplicationRepository>();
        _iCanSubmitmock = new Mock<ICanSubmitLoanApplication>();
    }

    public static LoanApplicationRepositoryTestBuilder New() => new();

    public LoanApplicationRepositoryTestBuilder ReturnLoanApplication(LoanApplicationId loanApplicationId, UserAccount userAccount, LoanApplicationEntity loanApplication)
    {
        _mock.Setup(x => x
                    .GetLoanApplication(loanApplicationId, userAccount, CancellationToken.None))
                .ReturnsAsync(loanApplication);

        return this;
    }

    public LoanApplicationRepositoryTestBuilder ReturnLoanApplications(UserAccount userAccount, IList<LoanApplicationEntity> loanApplicationEntities)
    {
        _mock.Setup(x => x
                .LoadAllLoanApplications(userAccount, CancellationToken.None))
                    .ReturnsAsync(loanApplicationEntities.Select(x =>
                        new UserLoanApplication(x.Id, x.Name, x.ExternalStatus, x.CreatedOn, x.LastModificationDate, x.LastModifiedBy))
                .ToList());

        return this;
    }

    public ILoanApplicationRepository Build()
    {
        return _mock.Object;
    }

    public ICanSubmitLoanApplication BuildICanSubmit()
    {
        return _iCanSubmitmock.Object;
    }

    public Mock<ILoanApplicationRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }

    public Mock<ICanSubmitLoanApplication> BuildICanSubmitMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = BuildICanSubmit();
        registerDependency.RegisterDependency(mockedObject);
        return _iCanSubmitmock;
    }
}
