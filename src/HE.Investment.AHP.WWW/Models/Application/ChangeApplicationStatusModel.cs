namespace HE.Investment.AHP.WWW.Models.Application;

public class ChangeApplicationStatusModel
{
    public ChangeApplicationStatusModel()
    {
    }

    public ChangeApplicationStatusModel(string applicationId, string applicationName)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
    }

    public string ApplicationId { get; set; }

    public string ApplicationName { get; set; }

    public string? WithdrawReason { get; set; }

    public string? HoldReason { get; set; }

    public string? RequestToEditReason { get; set; }
}
