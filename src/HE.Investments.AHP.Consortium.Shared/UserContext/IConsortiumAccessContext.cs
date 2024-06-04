namespace HE.Investments.AHP.Consortium.Shared.UserContext;

public interface IConsortiumAccessContext
{
    Task<bool> IsConsortiumLeadPartner();

    Task<bool> CanManageConsortium();

    Task<bool> CanEditApplication();
}
