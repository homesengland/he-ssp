using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetTenureDetailsQueryHandler : IRequestHandler<GetTenureDetailsQuery, TenureDetails>
{
    private readonly IHomeTypeRepository _repository;

    public GetTenureDetailsQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<TenureDetails> Handle(GetTenureDetailsQuery request, CancellationToken cancellationToken)
    {
        var homeType = await _repository.GetById(
            new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId),
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSegmentType.TenureDetails },
            cancellationToken);
        var tenureDetails = homeType.TenureDetails;

        return new TenureDetails(
            homeType.Application.Name.Name,
            homeType.Name.Value,
            tenureDetails.HomeMarketValue?.Value,
            tenureDetails.HomeWeeklyRent?.Value,
            tenureDetails.AffordableWeeklyRent?.Value,
            tenureDetails.AffordableRentAsPercentageOfMarketRent?.Value,
            tenureDetails.TargetRentExceedMarketRent.IsNotProvided() ? YesNoType.Undefined : tenureDetails.TargetRentExceedMarketRent!.Value,
            tenureDetails.ExemptFromTheRightToSharedOwnership,
            tenureDetails.ExemptionJustification?.Value);
    }
}
