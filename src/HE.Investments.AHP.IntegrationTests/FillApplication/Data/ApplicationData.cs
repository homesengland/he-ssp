using System.Globalization;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class ApplicationData
{
    public ApplicationData()
    {
        ApplicationId = "97593982-a6ab-ee11-be37-0022480041cf";
        ApplicationName = "IT_01/05/2024 08:43:41";
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
