using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Account.Domain.Data;

public interface IUsersCrmContext
{
    Task<IList<ContactDto>> GetUsers();

    Task<ContactDto> GetUser(string id);

    Task<string?> GetUserRole(string id, string email);

    Task ChangeUserRole(string userId, string role);
}
