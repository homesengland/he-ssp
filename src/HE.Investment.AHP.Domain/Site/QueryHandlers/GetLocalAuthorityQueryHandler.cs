extern alias Org;

using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investments.Common.Contract;
using MediatR;
using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investment.AHP.Domain.Site.QueryHandlers;

public class GetLocalAuthorityQueryHandler : IRequestHandler<GetLocalAuthorityQuery, LocalAuthority>
{
    private readonly ILocalAuthorityRepository _localAuthorityRepository;

    public GetLocalAuthorityQueryHandler(ILocalAuthorityRepository localAuthorityRepository)
    {
        _localAuthorityRepository = localAuthorityRepository;
    }

    public async Task<LocalAuthority> Handle(GetLocalAuthorityQuery request, CancellationToken cancellationToken)
    {
        var localAuthority = await _localAuthorityRepository.GetByCode(request.LocalAuthorityCode, cancellationToken);
        return LocalAuthorityMapper.Map(localAuthority)!;
    }
}
