using System.Globalization;
using HE.Investment.AHP.Contract.Application;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data;

public class ApplicationData
{
    public ApplicationData()
    {
    }

    public string SiteId { get; private set; }

    public string ApplicationId { get; private set; }

    public string ApplicationName { get; private set; }

    public Tenure Tenure => Tenure.AffordableRent;

    public ApplicationData GenerateApplicationName(string? customName = null)
    {
        if (!string.IsNullOrEmpty(customName))
        {
            customName += "_";
        }

        ApplicationName = $"IT_{customName}{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
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
