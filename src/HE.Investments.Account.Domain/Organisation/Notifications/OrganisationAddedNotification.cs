using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.Domain.Organisation.Notifications;

public class OrganisationAddedNotification : Notification
{
    public const string OrganisationNameParameterName = "OrganisationName";

    public OrganisationAddedNotification(string organisationName)
        : base(new Dictionary<string, string> { { OrganisationNameParameterName, organisationName } })
    {
    }
}
