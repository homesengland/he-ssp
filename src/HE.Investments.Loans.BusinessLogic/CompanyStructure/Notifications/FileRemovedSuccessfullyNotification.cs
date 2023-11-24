using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.Notifications;

public class FileRemovedSuccessfullyNotification : Notification
{
    public const string FileParameterName = "File";

    public FileRemovedSuccessfullyNotification(string fileName)
        : base(new Dictionary<string, string> { { FileParameterName, fileName } })
    {
    }
}
