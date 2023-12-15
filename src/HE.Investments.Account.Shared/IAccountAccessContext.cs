namespace HE.Investments.Account.Shared;

public interface IAccountAccessContext
{
    Task<bool> CanAccessOrganisationView();
    Task<bool> CanManageUsers();
}
