namespace HE.Investment.AHP.WWW.Config;

public class FrontDoorLinks : IFrontDoorLinks
{
    private readonly IConfiguration _configuration;

    public FrontDoorLinks(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string StartNewProject => _configuration.GetValue<string>("AppConfiguration:FrontDoorService:StartFrontDoorProject") ?? string.Empty;
}
