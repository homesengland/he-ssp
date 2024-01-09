using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class DuplicateHomeTypeCommandHandler : IRequestHandler<DuplicateHomeTypeCommand, OperationResult<HomeTypeId>>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public DuplicateHomeTypeCommandHandler(IHomeTypeRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult<HomeTypeId>> Handle(DuplicateHomeTypeCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var applicationId = new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId);
        var homeTypes = await _repository.GetByApplicationId(applicationId, account, HomeTypeSegmentTypes.All, cancellationToken);
        var duplicatedHomeType = homeTypes.Duplicate(new HomeTypeId(request.HomeTypeId));

        await _repository.Save(duplicatedHomeType, account.SelectedOrganisationId(), HomeTypeSegmentTypes.All, cancellationToken);

        return OperationResult.Success(duplicatedHomeType.Id);
    }
}
