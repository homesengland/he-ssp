extern alias Org;

using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using MediatR;

using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;
using LocalAuth = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.FrontDoor.Domain.LocalAuthority.QueryHandlers;

public class GetLocalAuthorityQueryHandler : IRequestHandler<GetLocalAuthorityQuery, LocalAuth>
{
    private readonly ILocalAuthorityRepository _repository;

    public GetLocalAuthorityQueryHandler(ILocalAuthorityRepository repository)
    {
        _repository = repository;
    }

    public async Task<LocalAuth> Handle(GetLocalAuthorityQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetById(new StringIdValueObject(request.LocalAuthorityId.Value), cancellationToken);
    }
}
