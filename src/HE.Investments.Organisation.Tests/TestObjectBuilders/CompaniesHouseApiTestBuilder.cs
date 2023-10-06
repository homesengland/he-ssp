using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Organisation.Tests.TestObjectBuilders;

public class CompaniesHouseApiTestBuilder : TestObjectBuilder<ICompaniesHouseApi>
{
    public static CompaniesHouseApiTestBuilder New()
    {
        return new CompaniesHouseApiTestBuilder();
    }

    public CompaniesHouseApiTestBuilder SearchReturnsError()
    {
        Mock.Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException());
        return this;
    }

    public CompaniesHouseApiTestBuilder GetByCompanyNumberReturnsError()
    {
        Mock.Setup(c => c.GetByCompanyNumber(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException());
        return this;
    }

    public CompaniesHouseApiTestBuilder GetByCompanyNumberReturns(CompanyDetailsItem organizationToReturn)
    {
        Mock.Setup(c => c.GetByCompanyNumber(It.IsAny<string>(), It.IsAny<CancellationToken>()))
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
        Mock.Setup(c => c.GetByCompanyNumber(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CompaniesHouseGetByCompanyNumberResult)null!);

        return this;
    }

    public CompaniesHouseApiTestBuilder SearchReturns(params CompanyDetailsItem[] organizationsToReturn)
    {
        Mock.Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompaniesHouseSearchResult.New(organizationsToReturn.ToList(), organizationsToReturn.Length));

        return this;
    }

    public CompaniesHouseApiTestBuilder SearchReturnsNothing()
    {
        Mock.Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompaniesHouseSearchResult.New(Enumerable.Empty<CompanyDetailsItem>().ToList(), 0));

        return this;
    }

    public CompaniesHouseApiTestBuilder SearchReturnsTotalOrganizations(int numberOfOrganizations)
    {
        Mock.Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompaniesHouseSearchResult.New(Enumerable.Empty<CompanyDetailsItem>().ToList(), numberOfOrganizations));

        return this;
    }

    public CompaniesHouseApiTestBuilder SearchReturnsTotalOrganizations(PagingQueryParams pagingQueryParams, int numberOfOrganizations)
    {
        Mock.Setup(c => c.Search(It.IsAny<string>(), pagingQueryParams, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompaniesHouseSearchResult.New(Enumerable.Empty<CompanyDetailsItem>().ToList(), numberOfOrganizations));

        return this;
    }
}
