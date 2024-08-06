namespace HE.UtilsService.BannerNotification.Contract;

public class BannerNotificaiton
{
    public string OrganisationId { get; set; }

    public ApplicationType ApplicationType { get; set; }

    public ApplicationArea? ApplicationArea { get; set; }

    public string NotificationKey { get; set; }

    public List<BannerNotificationParameter> NotificationParameters { get; set; } // { "ClaimId", "1234" }
}
