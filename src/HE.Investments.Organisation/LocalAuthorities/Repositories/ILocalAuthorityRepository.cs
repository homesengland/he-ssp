using HE.Investments.Common.Contract;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investments.Organisation.LocalAuthorities.Repositories;
public interface ILocalAuthorityRepository
{
    Task<(IList<LocalAuthority> Items, int TotalItems)> Search(string phrase, int startPage, int pageSize, CancellationToken cancellationToken);

    Task<LocalAuthority> GetById(StringIdValueObject localAuthorityId, CancellationToken cancellationToken);
}
