using HE.Investments.Organisation.CompaniesHouse.Contract;

namespace HE.Investments.Organisation.CompaniesHouse;

public interface ICompaniesHouseApi
{
    Task<CompaniesHouseSearchResult> Search(string organisationName, PagingQueryParams? pagingQueryParams, CancellationToken cancellationToken);

    Task<CompaniesHouseGetByCompanyNumberResult?> GetByCompanyNumber(string companyNumber, CancellationToken cancellationToken);
}
