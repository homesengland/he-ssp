namespace HE.InvestmentLoans.IntegrationTests.Config;

public class IntegrationTestConfig
{
    public UserConfig User { get; } = new();

    public IntegrationTestConfig()
    {
        
    }

    public IntegrationTestConfig(UserConfig user)
    {
        User = user;
    }
}
