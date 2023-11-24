namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsBaseModel
{
    public FinancialDetailsBaseModel()
    {
    }

    public FinancialDetailsBaseModel(Guid applicationId, string applicationName)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
    }

    public Guid ApplicationId { get; set; }

    public string ApplicationName { get; set; }
}
