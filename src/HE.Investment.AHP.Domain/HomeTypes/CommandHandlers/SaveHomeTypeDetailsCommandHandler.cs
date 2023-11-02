using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
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
        var homeTypes = await _repository.GetByApplicationId(request.ApplicationId, HomeTypeSegmentTypes.All, cancellationToken);
        var homeType = homeTypes.GetOrCreateNewHomeType(request.HomeTypeId.IsProvided() ? new HomeTypeId(request.HomeTypeId!) : null);

        var validationErrors = PerformWithValidation(
            () => homeTypes.ChangeName(homeType, request.HomeTypeName),
            () => homeType.ChangeHousingType(request.HousingType));
        if (validationErrors.Any())
        {
            return new OperationResult<HomeTypeId?>(validationErrors, null);
        }

        await _repository.Save(request.ApplicationId, homeType, HomeTypeSegmentTypes.All, cancellationToken);

        return new OperationResult<HomeTypeId?>(homeType.Id!);
    }
}
