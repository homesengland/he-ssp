using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public abstract class UpdateApplicationCommandHandler<TRequest> : IRequestHandler<TRequest, OperationResult<ApplicationId>>
    where TRequest : IRequest<OperationResult<ApplicationId>>, IUpdateApplicationCommand
{
    private readonly IAccountUserContext _accountUserContext;

    protected UpdateApplicationCommandHandler(IApplicationRepository repository, IAccountUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
        Repository = repository;
    }

    protected IApplicationRepository Repository { get; }

    public async Task<OperationResult<ApplicationId>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await Repository.GetById(new ApplicationId(request.Id), account, cancellationToken);

        await Update(request, application, cancellationToken);

        await Repository.Save(application, account.SelectedOrganisationId(), cancellationToken);

        return new OperationResult<ApplicationId>(application.Id);
    }

    protected abstract Task Update(TRequest request, ApplicationEntity application, CancellationToken cancellationToken);
}
