using HE.Investments.IntegrationTestsFramework.Auth;

namespace HE.Investments.IntegrationTestsFramework;

public interface IIntegrationTestPrerequisite
{
    Task<string?> Verify(ILoginData loginData);
}
