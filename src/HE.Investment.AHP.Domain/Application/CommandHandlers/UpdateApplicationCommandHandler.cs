using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public abstract class UpdateApplicationCommandHandler<TRequest> : IRequestHandler<TRequest, OperationResult<AhpApplicationId>>
    where TRequest : IRequest<OperationResult<AhpApplicationId>>, IUpdateApplicationCommand
{
    private readonly IAccountUserContext _accountUserContext;

    protected UpdateApplicationCommandHandler(IApplicationRepository repository, IAccountUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
        Repository = repository;
    }

    protected IApplicationRepository Repository { get; }

    public async Task<OperationResult<AhpApplicationId>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await Repository.GetById(request.Id, account, cancellationToken);

        await Update(request, application, cancellationToken);

        await Repository.Save(application, account.SelectedOrganisationId(), cancellationToken);

        return new OperationResult<AhpApplicationId>(application.Id);
    }

    protected abstract Task Update(TRequest request, ApplicationEntity application, CancellationToken cancellationToken);
}
