namespace HE.Investments.Consortium.Shared.UserContext;

public interface IConsortiumAccessContext
{
    Task<bool> IsConsortiumLeadPartner();

    Task<bool> CanManageConsortium();

    Task<bool> CanEditApplication();
}