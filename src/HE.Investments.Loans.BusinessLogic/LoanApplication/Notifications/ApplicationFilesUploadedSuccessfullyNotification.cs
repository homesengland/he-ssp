using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;

public class ApplicationFilesUploadedSuccessfullyNotification : Notification
{
    public const string FilesCountParameterName = "FilesCount";

    public ApplicationFilesUploadedSuccessfullyNotification(string filesCount)
        : base(new Dictionary<string, string> { { FilesCountParameterName, filesCount } })
    {
    }
}
