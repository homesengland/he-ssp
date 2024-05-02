using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Account.Domain.Data;

public interface IUsersCrmContext
{
    Task<IList<ContactDto>> GetUsers(string organisationId);

    Task<ContactDto> GetUser(string id);

    Task<int?> GetUserRole(string id, string organisationId);

    Task ChangeUserRole(string userId, string userAssigningId, int role, string organisationId);
}
