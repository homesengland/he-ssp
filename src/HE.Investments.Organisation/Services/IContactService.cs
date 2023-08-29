using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.Services;
public interface IContactService
{
    ContactDto RetrieveUserProfile(IOrganizationServiceAsync2 service, string contactExternalId);

    void UpdateUserProfile(IOrganizationServiceAsync2 service, string contactExternalId, ContactDto contactDto);
}
