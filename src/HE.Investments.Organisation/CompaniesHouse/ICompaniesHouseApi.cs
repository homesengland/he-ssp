using HE.Investments.Organisation.CompaniesHouse.Contract;

namespace HE.Investments.Organisation.CompaniesHouse;

public interface ICompaniesHouseApi
{
    Task<CompaniesHouseSearchResult> Search(string organisationName, CancellationToken cancellationToken);
}
