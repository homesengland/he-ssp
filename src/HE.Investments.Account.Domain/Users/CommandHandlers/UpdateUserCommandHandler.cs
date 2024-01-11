using HE.Investments.Account.Contract.Users.Commands;
using HE.Investments.Account.Domain.Users.Entities;
using HE.Investments.Account.Domain.Users.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Domain.Users.CommandHandlers;

public abstract class UpdateUserCommandHandler<TCommand> : IRequestHandler<TCommand, OperationResult>
    where TCommand : IUpdateUserCommand, IRequest<OperationResult>
{
    private readonly IUserRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    protected UpdateUserCommandHandler(IUserRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var entity = await _repository.GetUser(request.UserId, userAccount.SelectedOrganisationId(), cancellationToken);

        Update(entity, request);

        await _repository.Save(entity, userAccount.SelectedOrganisationId(), cancellationToken);

        return OperationResult.Success();
    }

    protected abstract void Update(UserEntity entity, TCommand request);
}
