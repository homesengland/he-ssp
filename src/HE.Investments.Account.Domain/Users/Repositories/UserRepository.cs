using HE.Investments.Account.Contract.User.Events;
using HE.Investments.Account.Domain.Data;
using HE.Investments.Account.Domain.Users.Entities;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
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

    public async Task<UserEntity> GetUser(UserGlobalId userGlobalId, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        var user = await _usersCrmContext.GetUser(userGlobalId.Value);
        var role = await _usersCrmContext.GetUserRole(userGlobalId.Value, organisationId.Value);

        return new UserEntity(
            UserGlobalId.From(user.contactExternalId),
            user.firstName,
            user.lastName,
            user.email,
            user.jobTitle,
            UserRoleMapper.ToDomain(role),
            null);
    }

    public async Task Save(UserEntity entity, string userAssigningId, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (entity.IsRoleModified)
        {
            var role = UserRoleMapper.ToDto(entity.Role);
            if (role != null)
            {
                await _usersCrmContext.ChangeUserRole(entity.Id.Value, userAssigningId, role.Value, organisationId.Value);
                await _mediator.Publish(new UserAccountsChangedEvent(entity.Id), cancellationToken);
            }
        }
    }
}
