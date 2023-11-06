using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetDisabledPeopleHomeTypeDetailsQueryHandler : IRequestHandler<GetDisabledPeopleHomeTypeDetailsQuery, DisabledPeopleHomeTypeDetails>
{
    private readonly IHomeTypeRepository _repository;

    public GetDisabledPeopleHomeTypeDetailsQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DisabledPeopleHomeTypeDetails> Handle(GetDisabledPeopleHomeTypeDetailsQuery request, CancellationToken cancellationToken)
    {
        var homeType = await _repository.GetById(
            request.ApplicationId,
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSegmentType.DisabledAndVulnerablePeople },
            cancellationToken);

        return new DisabledPeopleHomeTypeDetails(
            homeType.Name?.Value,
            homeType.DisabledPeopleHomeTypeDetails.HousingType,
            homeType.DisabledPeopleHomeTypeDetails.ClientGroupType);
    }
}
