namespace HE.InvestmentLoans.Common.Utils.Constants.Notification;
public static class NotificationBody
{
    public static string ApplicationWithdrawnWithName(string applicatonName) => new($"{applicatonName} project has been withdrawn.");
}
