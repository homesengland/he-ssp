namespace HE.Investments.IntegrationTestsFramework.Auth;

public interface ILoginData
{
    public string UserGlobalId { get; }

    public string Email { get; }

    public string OrganisationId { get; }

    public void Change(ILoginData loginData);

    bool IsProvided();
}
