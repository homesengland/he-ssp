using HE.InvestmentLoans.BusinessLogic.Funding.Entities;
using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestObjectBuilders;

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
