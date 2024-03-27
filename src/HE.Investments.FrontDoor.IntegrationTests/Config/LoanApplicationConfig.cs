using HE.Investments.FrontDoor.IntegrationTests.Pages;

namespace HE.Investments.FrontDoor.IntegrationTests.Config;

public class LoanApplicationConfig : WWW.Config.LoanApplicationConfig
{
    public LoanApplicationConfig()
    {
        StartNewLoanApplicationUrl = $"https://localhost/{ProjectsPagesUrl.List}";
    }
}
