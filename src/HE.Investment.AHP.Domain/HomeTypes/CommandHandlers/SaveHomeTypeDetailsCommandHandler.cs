using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHomeTypeDetailsCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<SaveHomeTypeDetailsCommand, OperationResult<HomeTypeId?>>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public SaveHomeTypeDetailsCommandHandler(IHomeTypeRepository repository, IAccountUserContext accountUserContext, ILogger<SaveHomeTypeDetailsCommand> logger)
        : base(logger)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult<HomeTypeId?>> Handle(SaveHomeTypeDetailsCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeTypes = await _repository.GetByApplicationId(request.ApplicationId, account, HomeTypeSegmentTypes.All, cancellationToken);
        var organisationId = account.SelectedOrganisationId();

        return request.HomeTypeId.IsNotProvided()
            ? await CreateNewHomeType(homeTypes, organisationId, request, cancellationToken)
            : await UpdateExistingHomeType(homeTypes, organisationId, request, cancellationToken);
    }

    private async Task<OperationResult<HomeTypeId?>> CreateNewHomeType(
        HomeTypesEntity homeTypes,
        OrganisationId organisationId,
        SaveHomeTypeDetailsCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var homeType = homeTypes.CreateHomeType(request.HomeTypeName, request.HousingType);
            await _repository.Save(homeType, organisationId, HomeTypeSegmentTypes.All, cancellationToken);

            return new OperationResult<HomeTypeId?>(homeType.Id);
        }
        catch (DomainValidationException domainValidationException)
        {
            return new OperationResult<HomeTypeId?>(domainValidationException.OperationResult.Errors, null);
        }
    }

    private async Task<OperationResult<HomeTypeId?>> UpdateExistingHomeType(
        HomeTypesEntity homeTypes,
        OrganisationId organisationId,
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

        await _repository.Save(homeType, organisationId, HomeTypeSegmentTypes.All, cancellationToken);

        return new OperationResult<HomeTypeId?>(homeType.Id);
    }
}
