using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;

public class LoanApplicationRepositoryTestBuilder
{
    private readonly Mock<ILoanApplicationRepository> _mock;
    private readonly Mock<ICanSubmitLoanApplication> _iCanSubmitMock;

    private LoanApplicationRepositoryTestBuilder()
    {
        _mock = new Mock<ILoanApplicationRepository>();
        _iCanSubmitMock = new Mock<ICanSubmitLoanApplication>();
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
        return _iCanSubmitMock.Object;
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
        return _iCanSubmitMock;
    }
}
