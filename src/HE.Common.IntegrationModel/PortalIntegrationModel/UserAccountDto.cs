namespace HE.Common.IntegrationModel.PortalIntegrationModel;

public class UserAccountDto
{
    public string ContactFirstName { get; set; }

    public string ContactLastName { get; set; }

    public string ContactEmail { get; set; }

    public string ContactTelephoneNumber { get; set; }

    public Guid AccountId { get; set; }

    public string ContactExternalId { get; set; }
}
