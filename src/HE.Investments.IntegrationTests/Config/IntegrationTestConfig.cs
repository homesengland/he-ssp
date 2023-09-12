namespace HE.InvestmentLoans.IntegrationTests.Config;

public class IntegrationTestConfig
{
    public IntegrationTestConfig(IUserConfig user)
    {
        User = user;
    }

    public IUserConfig User { get; }
}
