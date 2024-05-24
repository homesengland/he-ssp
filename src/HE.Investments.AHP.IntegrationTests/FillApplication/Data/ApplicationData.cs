using System.Globalization;
using HE.Investment.AHP.Contract.Application;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class ApplicationData
{
    public ApplicationData()
    {
    }

    public string ProjectId { get; } = "634a3629-8119-ef11-840a-002248c83519";

    public string SiteId { get; private set; }

    public string ApplicationId { get; private set; }

    public string ApplicationName { get; private set; }

    public Tenure Tenure => Tenure.AffordableRent;

    public ApplicationData GenerateApplicationName()
    {
        ApplicationName = $"IT_{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
        return this;
    }

    public void SetApplicationId(string applicationId)
    {
        ApplicationId = applicationId;
    }

    public void SetSiteId(string siteId)
    {
        SiteId = siteId;
    }
}
