using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.Services;
public class OrganizationService : IOrganizationService
{
    public OrganizationDetailsDto GetOrganizationDetails(IOrganizationServiceAsync2 service, string accountid, string contactExternalId)
    {
        var organizationDetailsDto = new OrganizationDetailsDto();
        if (Guid.TryParse(accountid, out var organizationId))
        {
            var account = service.Retrieve("account", organizationId, new ColumnSet(new string[]
            {
                    "name", "he_companieshousenumber", "address1_line1", "address1_line2", "address1_line3",
                    "address1_city", "address1_postalcode", "address1_country", "primarycontactid",
            }));

            organizationDetailsDto.registeredCompanyName = account.Contains("name") ? account["name"].ToString() : null;
            organizationDetailsDto.companyRegistrationNumber = account.Contains("he_companieshousenumber") ? account["he_companieshousenumber"].ToString() : null;
            organizationDetailsDto.addressLine1 = account.Contains("address1_line1") ? account["address1_line1"].ToString() : null;
            organizationDetailsDto.addressLine2 = account.Contains("address1_line2") ? account["address1_line2"].ToString() : null;
            organizationDetailsDto.addressLine3 = account.Contains("address1_line3") ? account["address1_line3"].ToString() : null;
            organizationDetailsDto.city = account.Contains("address1_city") ? account["address1_city"].ToString() : null;
            organizationDetailsDto.postalcode = account.Contains("address1_postalcode") ? account["address1_postalcode"].ToString() : null;
            organizationDetailsDto.country = account.Contains("address1_country") ? account["address1_country"].ToString() : null;

            if (account.Contains("primarycontactid") && account["primarycontactid"] != null)
            {
                var primaryContactReference = (EntityReference)account.Attributes["primarycontactid"];
                var contact = service.Retrieve("contact", primaryContactReference.Id, new ColumnSet(new string[]
                {
                        "fullname",
                        "emailaddress1",
                        "telephone1",
                }));

                organizationDetailsDto.compayAdminContactName = contact.Contains("fullname") ? contact.Attributes["fullname"].ToString() : null;
                organizationDetailsDto.compayAdminContactEmail = contact.Contains("emailaddress1") ? contact.Attributes["emailaddress1"].ToString() : null;
                organizationDetailsDto.compayAdminContactTelephone = contact.Contains("telephone1") ? contact.Attributes["telephone1"].ToString() : null;
            }
        }

        return organizationDetailsDto;
    }
}
