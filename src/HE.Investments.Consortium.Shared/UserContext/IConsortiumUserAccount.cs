namespace HE.Investments.Consortium.Shared.UserContext;

public interface IConsortiumUserAccount
{
    public bool CanManageConsortium { get; }

    public bool CanEdit { get; }

    public bool CanSubmit { get; }
}
