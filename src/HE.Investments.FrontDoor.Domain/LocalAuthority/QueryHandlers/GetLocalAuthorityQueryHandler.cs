extern alias Org;

using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Contract.LocalAuthority.Queries;
using MediatR;

using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investments.FrontDoor.Domain.LocalAuthority.QueryHandlers;

public class GetLocalAuthorityQueryHandler : IRequestHandler<GetLocalAuthorityQuery, Common.Contract.LocalAuthority>
{
    private readonly ILocalAuthorityRepository _repository;

    public GetLocalAuthorityQueryHandler(ILocalAuthorityRepository repository)
    {
        _repository = repository;
    }

    public async Task<Common.Contract.LocalAuthority> Handle(GetLocalAuthorityQuery request, CancellationToken cancellationToken)
    {
        var localAuthority = await _repository.GetById(new StringIdValueObject(request.LocalAuthorityId.Value), cancellationToken);
        return new Common.Contract.LocalAuthority(localAuthority.Id.Value, localAuthority.Name);
    }
}
