using HE.Investments.Account.Contract.User.Events;
using HE.Investments.Account.Domain.Data;
using HE.Investments.Account.Domain.Users.Entities;
using HE.Investments.Account.Domain.Users.ValueObjects;
using HE.Investments.Account.Shared.Repositories;
using MediatR;

namespace HE.Investments.Account.Domain.Users.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IUsersCrmContext _usersCrmContext;

    private readonly IMediator _mediator;

    public UserRepository(IUsersCrmContext usersCrmContext, IMediator mediator)
    {
        _usersCrmContext = usersCrmContext;
        _mediator = mediator;
    }

    public async Task<UserEntity> GetUser(string id, Guid organisationId, CancellationToken cancellationToken)
    {
        var user = await _usersCrmContext.GetUser(id);
        var role = await _usersCrmContext.GetUserRole(id, organisationId);

        return new UserEntity(
            new UserId(user.contactExternalId),
            user.firstName,
            user.lastName,
            user.email,
            user.jobTitle,
            UserRoleMapper.ToDomain(role),
            null);
    }

    public async Task Save(UserEntity entity, Guid organisationId, CancellationToken cancellationToken)
    {
        if (entity.IsRoleModified)
        {
            var role = UserRoleMapper.ToDto(entity.Role);
            if (role != null)
            {
                await _usersCrmContext.ChangeUserRole(entity.Id.Value, role.Value, organisationId);
                await _mediator.Publish(new UserAccountsChangedEvent(entity.Id.Value), cancellationToken);
            }
        }
    }
}
