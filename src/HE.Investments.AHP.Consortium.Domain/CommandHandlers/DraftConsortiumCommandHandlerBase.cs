using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public abstract class DraftConsortiumCommandHandlerBase<TRequest> : IRequestHandler<TRequest, OperationResult>
    where TRequest : IConsortiumCommand
{
    private readonly IConsortiumRepository _repository;

    private readonly IDraftConsortiumRepository _draftConsortiumRepository;

    private readonly IAccountUserContext _accountUserContext;

    protected DraftConsortiumCommandHandlerBase(
        IConsortiumRepository repository,
        IDraftConsortiumRepository draftConsortiumRepository,
        IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
        _draftConsortiumRepository = draftConsortiumRepository;
    }

    public async Task<OperationResult> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var draftConsortium = _draftConsortiumRepository.Get(request.ConsortiumId);
        if (draftConsortium.IsProvided())
        {
            await Perform(draftConsortium!, request, cancellationToken);
            _draftConsortiumRepository.Save(draftConsortium!);
        }
        else
        {
            var consortium = await _repository.GetConsortium(request.ConsortiumId, account, cancellationToken);

            await Perform(consortium, request, cancellationToken);
            await _repository.Save(consortium, account, cancellationToken);
        }

        return OperationResult.Success();
    }

    protected virtual Task Perform(IConsortiumEntity consortium, TRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
