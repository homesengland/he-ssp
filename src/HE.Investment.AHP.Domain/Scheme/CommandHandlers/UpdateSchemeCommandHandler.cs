using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

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
        var applicationId = new ApplicationId(request.ApplicationId);
        var scheme = await _repository.GetByApplicationId(applicationId, account, _includeFiles, cancellationToken);

        Update(scheme, request);

        await _repository.Save(scheme, account.SelectedOrganisationId(), cancellationToken);

        return new OperationResult<ApplicationId?>(applicationId);
    }

    protected abstract void Update(SchemeEntity scheme, TCommand request);
}
