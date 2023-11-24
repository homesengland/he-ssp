using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;

public class ApplicationWithdrawSuccessfullyNotification : Notification
{
    public const string ApplicationNameParameterName = "ApplicationName";

    public const string FundingSupportEmailParameterName = "FundingSupportEmail";

    public ApplicationWithdrawSuccessfullyNotification(string applicationName, string fundingSupportEmail)
        : base(new Dictionary<string, string> { { ApplicationNameParameterName, applicationName }, { FundingSupportEmailParameterName, fundingSupportEmail }, })
    {
    }
}
