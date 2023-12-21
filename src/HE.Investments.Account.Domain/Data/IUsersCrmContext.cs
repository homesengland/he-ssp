using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Account.Domain.Data;

public interface IUsersCrmContext
{
    Task<IList<ContactDto>> GetUsers(Guid organisationId);

    Task<ContactDto> GetUser(string id);

    Task<int?> GetUserRole(string id, Guid organisationId);

    Task ChangeUserRole(string userId, int role, Guid organisationId);
}
