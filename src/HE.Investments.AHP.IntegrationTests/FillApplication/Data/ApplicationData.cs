using System.Globalization;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class ApplicationData
{
    public ApplicationData()
    {
    }

    public string ApplicationId { get; private set; }

    public string ApplicationName { get; private set; }

    public string GenerateApplicationName()
    {
        ApplicationName = $"IT_{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
        return ApplicationName;
    }

    public void SetApplicationId(string applicationId)
    {
        ApplicationId = applicationId;
    }
}
