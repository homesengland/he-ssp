using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetOlderPeopleHomeTypeDetailsQueryHandler : IRequestHandler<GetOlderPeopleHomeTypeDetailsQuery, OlderPeopleHomeTypeDetails>
{
    private readonly IHomeTypeRepository _repository;

    public GetOlderPeopleHomeTypeDetailsQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<OlderPeopleHomeTypeDetails> Handle(GetOlderPeopleHomeTypeDetailsQuery request, CancellationToken cancellationToken)
    {
        var homeType = await _repository.GetById(
            new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId),
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSegmentType.OlderPeople },
            cancellationToken);

        return new OlderPeopleHomeTypeDetails(homeType.Name.Value, homeType.OlderPeopleHomeTypeDetails.HousingType);
    }
}
