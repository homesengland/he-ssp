namespace HE.Investment.AHP.WWW.Models.FinancialDetails;

public class FinancialDetailsBaseModel
{
    public FinancialDetailsBaseModel()
    {
    }

    public FinancialDetailsBaseModel(string applicationId, string applicationName)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
    }

    public string ApplicationId { get; set; }

    public string ApplicationName { get; set; }
}
