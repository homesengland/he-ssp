using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Notifications;

public class ProjectDeletedSuccessfullyNotification : Notification
{
    public const string ProjectNameParameterName = "ProjectName";

    public ProjectDeletedSuccessfullyNotification(string projectName)
        : base(new Dictionary<string, string> { { ProjectNameParameterName, projectName } })
    {
    }
}
