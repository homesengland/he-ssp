namespace HE.Investments.Consortium.Shared.UserContext;

public interface IConsortiumAccessContext
{
    Task<ConsortiumUserAccount> GetSelectedAccount();

    Task<bool> IsConsortiumLeadPartner();

    Task<bool> CanManageConsortium();

    Task<bool> CanEditApplication();

    Task<bool> CanSubmit();
}
