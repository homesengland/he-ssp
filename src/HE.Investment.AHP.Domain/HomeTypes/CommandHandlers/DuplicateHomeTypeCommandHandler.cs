using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
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
        var homeTypes = await _repository.GetByApplicationId(request.ApplicationId, account, HomeTypeSegmentTypes.All, cancellationToken);
        var duplicatedHomeType = homeTypes.Duplicate(request.HomeTypeId);

        await _repository.Save(duplicatedHomeType, account.SelectedOrganisationId(), HomeTypeSegmentTypes.All, cancellationToken);

        return OperationResult.Success(duplicatedHomeType.Id);
    }
}
