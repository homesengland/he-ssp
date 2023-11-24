using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetHomeTypesQueryHandler : IRequestHandler<GetHomeTypesQuery, ApplicationHomeTypes>
{
    private readonly IHomeTypeRepository _repository;

    public GetHomeTypesQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationHomeTypes> Handle(GetHomeTypesQuery request, CancellationToken cancellationToken)
    {
        var homeTypes = await _repository.GetByApplicationId(
            new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId),
            new[] { HomeTypeSegmentType.HomeInformation },
            cancellationToken);

        return new ApplicationHomeTypes(
            homeTypes.ApplicationName.Name,
            homeTypes.HomeTypes.OrderByDescending(x => x.CreatedOn).Select(Map).ToList());
    }

    private static HomeTypeDetails Map(IHomeTypeEntity homeType)
    {
        return new HomeTypeDetails(
            homeType.Application.Name.Name,
            homeType.Id.Value,
            homeType.Name.Value,
            homeType.HomeInformation.NumberOfHomes?.Value,
            homeType.HousingType);
    }
}
