namespace HE.Investment.AHP.Domain.UserContext;

public interface IAhpAccessContext
{
    Task<bool> CanManageConsortium();

    Task<bool> CanEditApplication();
}
