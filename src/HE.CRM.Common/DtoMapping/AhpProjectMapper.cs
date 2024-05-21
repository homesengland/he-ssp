using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.dtomapping
{
    public class AhpProjectMapper
    {

        public static invln_ahpproject ToEntity(Contact createdByContact, string organisationId, string heProjectId, string ahpProjectName, string consortiumId = null)
        {
            var ahpProject = new invln_ahpproject();

            if (createdByContact != null)
            {
                ahpProject.invln_ContactId = createdByContact.ToEntityReference();
            }

            if (organisationId != null && Guid.TryParse(organisationId, out var organisationGuid))
            {
                ahpProject.invln_AccountId = new EntityReference(Account.EntityLogicalName, organisationGuid);
            }

            if (heProjectId != null && Guid.TryParse(heProjectId, out var heProjectGuid))
            {
                ahpProject.invln_HeProjectId = new EntityReference(he_Pipeline.EntityLogicalName, heProjectGuid);
            }

            if (consortiumId != null && Guid.TryParse(consortiumId, out var consortiumGuid))
            {
                ahpProject.invln_ConsortiumId = new EntityReference(invln_Consortium.EntityLogicalName, consortiumGuid);
            }

            ahpProject.invln_Name = ahpProjectName;

            return ahpProject;
        }

        public static AhpProjectDto MapRegularEntityToDto(invln_ahpproject ahpproject, List<SiteDto> listOfSites = null, List<AhpApplicationDto> listOfApplications = null)
        {
            return new AhpProjectDto()
            {
                AhpProjectId = ahpproject.Id.ToString(),
                AhpProjectName = ahpproject.invln_Name,
                ListOfSites = listOfSites,
                ListOfApplications = listOfApplications
            };
        }
    }
}
