using HE.Investments.FrontDoor.IntegrationTests.Pages;

namespace HE.Investments.FrontDoor.IntegrationTests.Config;

public class ProgrammeUrlConfig : WWW.Config.ProgrammeUrlConfig
{
    public ProgrammeUrlConfig()
    {
        Loans = $"https://localhost/{ProjectsPagesUrl.List}";
    }
}
