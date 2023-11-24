using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.BusinessLogic.CompanyStructure;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;

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
