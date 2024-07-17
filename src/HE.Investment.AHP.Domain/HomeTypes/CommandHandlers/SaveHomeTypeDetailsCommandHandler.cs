using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHomeTypeDetailsCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<SaveHomeTypeDetailsCommand, OperationResult<HomeTypeId?>>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IConsortiumUserContext _accountUserContext;

    public SaveHomeTypeDetailsCommandHandler(IHomeTypeRepository repository, IConsortiumUserContext accountUserContext, ILogger<SaveHomeTypeDetailsCommand> logger)
        : base(logger)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult<HomeTypeId?>> Handle(SaveHomeTypeDetailsCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeTypes = await _repository.GetByApplicationId(request.ApplicationId, account, cancellationToken);

        return request.HomeTypeId.IsNotProvided()
            ? await CreateNewHomeType(homeTypes, account, request, cancellationToken)
            : await UpdateExistingHomeType(homeTypes, account, request, cancellationToken);
    }

    private async Task<OperationResult<HomeTypeId?>> CreateNewHomeType(
        HomeTypesEntity homeTypes,
        UserAccount userAccount,
        SaveHomeTypeDetailsCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var homeType = homeTypes.CreateHomeType(request.HomeTypeName, request.HousingType);
            await _repository.Save(homeType, userAccount, cancellationToken);

            return new OperationResult<HomeTypeId?>(homeType.Id);
        }
        catch (DomainValidationException domainValidationException)
        {
            return new OperationResult<HomeTypeId?>(domainValidationException.OperationResult.Errors, null);
        }
    }

    private async Task<OperationResult<HomeTypeId?>> UpdateExistingHomeType(
        HomeTypesEntity homeTypes,
        UserAccount userAccount,
        SaveHomeTypeDetailsCommand request,
        CancellationToken cancellationToken)
    {
        var homeType = homeTypes.GetEntityById(request.HomeTypeId!);
        var validationErrors = PerformWithValidation(
            () => homeTypes.ChangeName(homeType, request.HomeTypeName),
            () => homeType.ChangeHousingType(request.HousingType));
        if (validationErrors.Any())
        {
            return new OperationResult<HomeTypeId?>(validationErrors, null);
        }

        await _repository.Save(homeType, userAccount, cancellationToken);

        return new OperationResult<HomeTypeId?>(homeType.Id);
    }
}
