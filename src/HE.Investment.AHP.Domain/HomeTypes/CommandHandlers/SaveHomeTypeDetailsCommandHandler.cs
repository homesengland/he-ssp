using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHomeTypeDetailsCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<SaveHomeTypeDetailsCommand, OperationResult<HomeTypeId?>>
{
    private readonly IHomeTypeRepository _repository;

    public SaveHomeTypeDetailsCommandHandler(
        IHomeTypeRepository repository,
        ILogger<SaveHomeTypeDetailsCommand> logger)
        : base(logger)
    {
        _repository = repository;
    }

    public async Task<OperationResult<HomeTypeId?>> Handle(SaveHomeTypeDetailsCommand request, CancellationToken cancellationToken)
    {
        var applicationId = new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId);
        var homeTypes = await _repository.GetByApplicationId(applicationId, HomeTypeSegmentTypes.All, cancellationToken);
        var homeType = homeTypes.GetOrCreateNewHomeType(request.HomeTypeId.IsProvided() ? new HomeTypeId(request.HomeTypeId!) : null);

        var validationErrors = PerformWithValidation(
            () => homeTypes.ChangeName(homeType, request.HomeTypeName),
            () => homeType.ChangeHousingType(request.HousingType));
        if (validationErrors.Any())
        {
            return new OperationResult<HomeTypeId?>(validationErrors, null);
        }

        await _repository.Save(applicationId, homeType, HomeTypeSegmentTypes.All, cancellationToken);

        return new OperationResult<HomeTypeId?>(homeType.Id!);
    }
}
