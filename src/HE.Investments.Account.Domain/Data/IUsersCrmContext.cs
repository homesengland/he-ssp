using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Account.Domain.Data;

public interface IUsersCrmContext
{
    Task<IList<ContactDto>> GetUsers();

    Task<ContactDto> GetUser(string id);

    Task<int?> GetUserRole(string id);

    Task ChangeUserRole(string userId, int role);
}
