using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;

public class CompanyStructureRepositoryTestBuilder
{
    private readonly Mock<ICompanyStructureRepository> _mock;

    private CompanyStructureRepositoryTestBuilder()
    {
        _mock = new Mock<ICompanyStructureRepository>();
    }

    public static CompanyStructureRepositoryTestBuilder New() => new();

    public CompanyStructureRepositoryTestBuilder ReturnCompanyStructureEntity(
        LoanApplicationId loanApplicationId,
        UserAccount userAccount,
        CompanyStructureEntity companyStructureEntity)
    {
        _mock.Setup(x => x.GetAsync(loanApplicationId, userAccount, It.IsAny<CompanyStructureFieldsSet>(), CancellationToken.None))
            .ReturnsAsync(companyStructureEntity);
        return this;
    }

    public ICompanyStructureRepository Build()
    {
        return _mock.Object;
    }

    public Mock<ICompanyStructureRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
