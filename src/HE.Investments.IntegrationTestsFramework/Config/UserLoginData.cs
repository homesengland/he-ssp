using HE.Investments.IntegrationTestsFramework.Auth;

namespace HE.Investments.IntegrationTestsFramework.Config;

public class UserLoginData : ILoginData
{
    public string UserGlobalId { get; set; }

    public string Email { get; set; }

    public void Change(ILoginData loginData)
    {
        UserGlobalId = loginData.UserGlobalId;
        Email = loginData.Email;
    }

    public bool IsProvided()
    {
        return !string.IsNullOrWhiteSpace(UserGlobalId) && !string.IsNullOrWhiteSpace(Email);
    }
}
