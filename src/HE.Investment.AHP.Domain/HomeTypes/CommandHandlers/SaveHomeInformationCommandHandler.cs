using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHomeInformationCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<SaveHomeInformationCommand, OperationResult>
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    public SaveHomeInformationCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveHomeInformationCommandHandler> logger)
        : base(logger)
    {
        _homeTypeRepository = homeTypeRepository;
    }

    public async Task<OperationResult> Handle(SaveHomeInformationCommand request, CancellationToken cancellationToken)
    {
        var homeType = await _homeTypeRepository.GetById(
            request.ApplicationId,
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSegmentType.HomeInformation },
            cancellationToken);
        var homeInformation = homeType.HomeInformation;

        var errors = PerformWithValidation(
            () => homeInformation.ChangeNumberOfHomes(request.NumberOfHomes),
            () => homeInformation.ChangeNumberOfBedrooms(request.NumberOfBedrooms),
            () => homeInformation.ChangeMaximumOccupancy(request.MaximumOccupancy),
            () => homeInformation.ChangeNumberOfStoreys(request.NumberOfStoreys));

        if (errors.Any())
        {
            return new OperationResult(errors);
        }

        await _homeTypeRepository.Save(request.ApplicationId, homeType, new[] { HomeTypeSegmentType.HomeInformation }, cancellationToken);
        return OperationResult.Success();
    }
}
