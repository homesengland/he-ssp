using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

public class SiteDetailsRepository : CrmEntityRepository<invln_SiteDetails, DataverseContext>, ISiteDetailsRepository
{
    public SiteDetailsRepository(CrmRepositoryArgs args) : base(args)
    {
    }

    public void DeleteSiteDetailsRelatedToLoanApplication(EntityReference loanApplicationId)
    {
        if (loanApplicationId != null)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                var siteDetails = ctx.CreateQuery<invln_SiteDetails>()
                    .Where(x => x.invln_Loanapplication.Id == loanApplicationId.Id).ToList();
                if (siteDetails != null)
                {
                    foreach (var site in siteDetails)
                    {
                        service.Delete(invln_SiteDetails.EntityLogicalName, site.Id);
                    }
                }
            }
        }
    }

    public List<invln_SiteDetails> GetSiteDetailRelatedToLoanApplication(EntityReference loanApplicationId)
    {
        using (var ctx = new OrganizationServiceContext(service))
        {
            return ctx.CreateQuery<invln_SiteDetails>()
                .Where(x => x.invln_Loanapplication.Id == loanApplicationId.Id && x.StateCode.Value == (int)invln_SiteDetailsState.Active).ToList();
        }
    }

    public invln_SiteDetails GetSiteDetailForAccountAndContact(Guid siteDetailsGuid, string accountId, string contactExternalId, string attributes = null)
    {
        logger.Trace($"GetSiteDetailForAccountAndContact");
        var fetchXml = $@"<fetch>
                          <entity name='invln_sitedetails'>
                            {attributes}
                            <filter>
                              <condition attribute=""invln_sitedetailsid"" operator=""eq"" value=""{siteDetailsGuid}"" />
                            </filter>
                                <link-entity name=""invln_loanapplication"" from=""invln_loanapplicationid"" to=""invln_loanapplication"">
                                    <filter>
                                        <condition attribute=""invln_account"" operator=""eq"" value=""{accountId}"" />
                                    </filter>
                                    <link-entity name=""contact"" from=""contactid"" to=""invln_contact"">
                                        <filter>
                                            <condition attribute=""invln_externalid"" operator=""eq"" value=""{contactExternalId}"" />
                                        </filter>
                                    </link-entity>
                                </link-entity>
                          </entity>
                        </fetch>";
        EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
        return result.Entities.Select(x => x.ToEntity<invln_SiteDetails>()).FirstOrDefault();
    }
}

