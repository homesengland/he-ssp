using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.BusinessLogic.Funding.Entities;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.TestObjectBuilders;

public class FundingRepositoryTestBuilder
{
    private readonly Mock<IFundingRepository> _mock;

    private FundingRepositoryTestBuilder()
    {
        _mock = new Mock<IFundingRepository>();
    }

    public static FundingRepositoryTestBuilder New() => new();

    public FundingRepositoryTestBuilder ReturnFundingEntity(
        LoanApplicationId loanApplicationId,
        UserAccount userAccount,
        FundingEntity fundingEntity)
    {
        _mock.Setup(x => x.GetAsync(loanApplicationId, userAccount, It.IsAny<FundingFieldsSet>(), CancellationToken.None)).ReturnsAsync(fundingEntity);
        return this;
    }

    public IFundingRepository Build()
    {
        return _mock.Object;
    }

    public Mock<IFundingRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
