using System.ComponentModel.DataAnnotations;
using HE.UtilsService.BannerNotification.Shared;

namespace HE.UtilsService.BannerNotification.Contract;

public class BannerNotification
{
    [MinLength(1, ErrorMessage = "OrganisationId must not be empty")]
    public string OrganisationId { get; set; }

    public ApplicationType ApplicationType { get; set; }

    public ApplicationArea? ApplicationArea { get; set; }

    [MinLength(1, ErrorMessage = "NotificationType must not be empty")]
    public string NotificationType { get; set; }

    public List<BannerNotificationParameter>? NotificationParameters { get; set; }
}
