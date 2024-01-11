using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public abstract class UpdateSchemeCommandHandler<TCommand> : IRequestHandler<TCommand, OperationResult>
    where TCommand : IUpdateSchemeCommand, IRequest<OperationResult>
{
    private readonly ISchemeRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly bool _includeFiles;

    protected UpdateSchemeCommandHandler(ISchemeRepository repository, IAccountUserContext accountUserContext, bool includeFiles)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
        _includeFiles = includeFiles;
    }

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var scheme = await _repository.GetByApplicationId(request.ApplicationId, account, _includeFiles, cancellationToken);

        Update(scheme, request);

        await _repository.Save(scheme, account.SelectedOrganisationId(), cancellationToken);

        return new OperationResult<AhpApplicationId?>(request.ApplicationId);
    }

    protected abstract void Update(SchemeEntity scheme, TCommand request);
}
