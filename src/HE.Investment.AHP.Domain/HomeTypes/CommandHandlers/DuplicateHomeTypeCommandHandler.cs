using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class DuplicateHomeTypeCommandHandler : IRequestHandler<DuplicateHomeTypeCommand, OperationResult<HomeTypeId>>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IConsortiumUserContext _accountUserContext;

    public DuplicateHomeTypeCommandHandler(IHomeTypeRepository repository, IConsortiumUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult<HomeTypeId>> Handle(DuplicateHomeTypeCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeTypes = await _repository.GetByApplicationId(request.ApplicationId, account, cancellationToken);
        var duplicatedHomeType = homeTypes.Duplicate(request.HomeTypeId);

        await _repository.Save(duplicatedHomeType, account, cancellationToken);

        return OperationResult.Success(duplicatedHomeType.Id);
    }
}
