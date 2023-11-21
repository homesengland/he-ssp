using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.Domain.HomeTypes.Notifications;

public class HomeTypeHasBeenCreatedNotification : Notification
{
    public const string HomeTypeNameParameterName = "HomeTypeNameParameterName";

    public HomeTypeHasBeenCreatedNotification(string homeTypeName)
        : base(nameof(HomeTypeHasBeenCreatedNotification), new Dictionary<string, string> { { HomeTypeNameParameterName, homeTypeName } })
    {
    }
}
