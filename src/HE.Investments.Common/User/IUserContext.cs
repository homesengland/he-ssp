namespace HE.Investments.Common.User;

public interface IUserContext
{
    public string UserGlobalId { get; }

    public string Email { get; }

    public bool IsAuthenticated { get; }
}
