using HE.Investments.Common.Contract;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using LocalAuthority = HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.Organisation.LocalAuthorities.Repositories;

public interface ILocalAuthorityRepository
{
    Task<(IList<LocalAuthority> Items, int TotalItems)> Search(string phrase, int startPage, int pageSize, CancellationToken cancellationToken);

    Task<LocalAuthority> GetByCode(LocalAuthorityCode localAuthorityCode, CancellationToken cancellationToken);
}
