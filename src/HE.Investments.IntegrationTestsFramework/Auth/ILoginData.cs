namespace HE.Investments.IntegrationTestsFramework.Auth;

public interface ILoginData
{
    public string UserGlobalId { get; }

    public string Email { get; }

    public void Change(ILoginData loginData);

    bool IsProvided();
}
