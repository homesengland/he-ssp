using HE.Investments.FrontDoor.Contract.LocalAuthority.Queries;
using HE.Investments.Organisation.LocalAuthorities.Repositories;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.LocalAuthority.QueryHandlers;

public class GetLocalAuthorityQueryHandler : IRequestHandler<GetLocalAuthorityQuery, Investments.Common.Contract.LocalAuthority>
{
    private readonly ILocalAuthorityRepository _repository;

    public GetLocalAuthorityQueryHandler(ILocalAuthorityRepository repository)
    {
        _repository = repository;
    }

    public async Task<Investments.Common.Contract.LocalAuthority> Handle(GetLocalAuthorityQuery request, CancellationToken cancellationToken)
    {
        var localAuthority = await _repository.GetByCode(request.LocalAuthorityCode, cancellationToken);
        return new Investments.Common.Contract.LocalAuthority(localAuthority.Code.Value, localAuthority.Name);
    }
}
