using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public abstract class ConsortiumCommandHandlerBase<TRequest> : IRequestHandler<TRequest, OperationResult>
    where TRequest : IConsortiumCommand
{
    private readonly IConsortiumRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    protected ConsortiumCommandHandlerBase(IConsortiumRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var consortium = await _repository.GetConsortium(request.ConsortiumId, account, cancellationToken);

        await Perform(consortium, request, cancellationToken);
        await _repository.Save(consortium, account, cancellationToken);

        return OperationResult.Success();
    }

    protected virtual Task Perform(ConsortiumEntity consortium, TRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
