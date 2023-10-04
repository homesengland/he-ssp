using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Organisation.Tests.TestObjectBuilders;

public class CompaniesHouseApiTestBuilder
{
    private readonly Mock<ICompaniesHouseApi> _mock;

    private CompaniesHouseApiTestBuilder()
    {
        _mock = new Mock<ICompaniesHouseApi>();
    }

    public static CompaniesHouseApiTestBuilder New() => new();

    public CompaniesHouseApiTestBuilder SearchReturnsError()
    {
        _mock.Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException());
        return this;
    }

    public CompaniesHouseApiTestBuilder GetByCompanyNumberReturnsError()
    {
        _mock.Setup(c => c.GetByCompanyNumber(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException());
        return this;
    }

    public CompaniesHouseApiTestBuilder GetByCompanyNumberReturns(CompanyDetailsItem organizationToReturn)
    {
        _mock
            .Setup(c => c.GetByCompanyNumber(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CompaniesHouseGetByCompanyNumberResult
            {
                CompanyName = organizationToReturn.CompanyName,
                CompanyNumber = organizationToReturn.CompanyNumber,
                OfficeAddress = organizationToReturn.OfficeAddress
            });

        return this;
    }

    public CompaniesHouseApiTestBuilder GetByCompanyNumberReturnsNothing()
    {
        _mock.Setup(c => c.GetByCompanyNumber(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CompaniesHouseGetByCompanyNumberResult)null!);

        return this;
    }

    public CompaniesHouseApiTestBuilder SearchReturns(params CompanyDetailsItem[] organizationsToReturn)
    {
        _mock
            .Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompaniesHouseSearchResult.New(organizationsToReturn.ToList(), organizationsToReturn.Length));

        return this;
    }

    public CompaniesHouseApiTestBuilder SearchReturnsNothing()
    {
        _mock.Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompaniesHouseSearchResult.New(Enumerable.Empty<CompanyDetailsItem>().ToList(), 0));

        return this;
    }

    public CompaniesHouseApiTestBuilder SearchReturnsTotalOrganizations(int numberOfOrganizations)
    {
        _mock
            .Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompaniesHouseSearchResult.New(Enumerable.Empty<CompanyDetailsItem>().ToList(), numberOfOrganizations));

        return this;
    }

    public CompaniesHouseApiTestBuilder Register(IRegisterDependency registerDependency)
    {
        registerDependency.RegisterDependency(Build());
        return this;
    }

    public ICompaniesHouseApi Build()
    {
        return _mock.Object;
    }
}
