using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHomeTypeDetailsCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<SaveHomeTypeDetailsCommand, OperationResult<HomeTypeId?>>
{
    private readonly IHomeTypeRepository _repository;

    public SaveHomeTypeDetailsCommandHandler(IHomeTypeRepository repository, ILogger<SaveHomeTypeDetailsCommand> logger)
        : base(logger)
    {
        _repository = repository;
    }

    public async Task<OperationResult<HomeTypeId?>> Handle(SaveHomeTypeDetailsCommand request, CancellationToken cancellationToken)
    {
        var applicationId = new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId);
        var homeTypes = await _repository.GetByApplicationId(applicationId, HomeTypeSegmentTypes.All, cancellationToken);

        return request.HomeTypeId.IsNotProvided()
            ? await CreateNewHomeType(homeTypes, request, cancellationToken)
            : await UpdateExistingHomeType(homeTypes, request, cancellationToken);
    }

    private async Task<OperationResult<HomeTypeId?>> CreateNewHomeType(
        HomeTypesEntity homeTypes,
        SaveHomeTypeDetailsCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var homeType = homeTypes.CreateHomeType(request.HomeTypeName, request.HousingType);
            await _repository.Save(homeType, HomeTypeSegmentTypes.All, cancellationToken);

            return new OperationResult<HomeTypeId?>(homeType.Id);
        }
        catch (DomainValidationException domainValidationException)
        {
            return new OperationResult<HomeTypeId?>(domainValidationException.OperationResult.Errors, null);
        }
    }

    private async Task<OperationResult<HomeTypeId?>> UpdateExistingHomeType(
        HomeTypesEntity homeTypes,
        SaveHomeTypeDetailsCommand request,
        CancellationToken cancellationToken)
    {
        var homeType = homeTypes.GetEntityById(new HomeTypeId(request.HomeTypeId!));
        var validationErrors = PerformWithValidation(
            () => homeTypes.ChangeName(homeType, request.HomeTypeName),
            () => homeType.ChangeHousingType(request.HousingType));
        if (validationErrors.Any())
        {
            return new OperationResult<HomeTypeId?>(validationErrors, null);
        }

        await _repository.Save(homeType, HomeTypeSegmentTypes.All, cancellationToken);

        return new OperationResult<HomeTypeId?>(homeType.Id);
    }
}
