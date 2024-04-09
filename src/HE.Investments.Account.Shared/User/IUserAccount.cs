namespace HE.Investments.Account.Shared.User;

public interface IUserAccount
{
    bool CanManageUsers { get; }

    bool CanAccessOrganisationView { get; }

    bool CanSubmitApplication { get; }

    bool CanEditApplication { get; }
}
