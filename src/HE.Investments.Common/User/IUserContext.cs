using HE.Investments.Common.Contract;

namespace HE.Investments.Common.User;

public interface IUserContext
{
    public string UserGlobalId { get; }

    public OrganisationId? OrganisationId { get; }

    public string Email { get; }

    public bool IsAuthenticated { get; }
}
