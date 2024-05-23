namespace HE.Investment.AHP.Domain.UserContext;

public interface IAhpAccessContext
{
    Task<bool> IsConsortiumLeadPartner();

    Task<bool> CanManageConsortium();

    Task<bool> CanEditApplication();
}
