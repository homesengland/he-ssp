using HE.Investments.Common.Services.Notifications;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.Notifications;

public class FilesUploadedSuccessfullyNotification : Notification
{
    public const string FilesParameterName = "Files";

    public FilesUploadedSuccessfullyNotification(string files)
        : base(new Dictionary<string, string> { { FilesParameterName, files } })
    {
    }
}
