namespace HE.Investments.AHP.IntegrationTests.FillApplication;

public class ApplicationData
{
    public string ApplicationId { get; private set; }

    public string ApplicationName { get; private set; }

    public string GenerateApplicationName()
    {
        ApplicationName = $"TestApplication{Guid.NewGuid()}";
        return ApplicationName;
    }

    public void SetApplicationId(string applicationId)
    {
        ApplicationId = applicationId;
    }
}
