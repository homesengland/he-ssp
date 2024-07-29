using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using System;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IAhpApplicationRepository : ICrmEntityRepository<invln_scheme, DataverseContext>
    {
        List<invln_scheme> GetApplicationsForOrganisationAndContact(string organisationId, string contactFilter, string attributes, string additionalRecordFilters);
        bool ApplicationWithGivenNameExists(string name);
        bool ApplicationWithGivenIdExistsForOrganisation(Guid applicationId, Guid organisationId);
        bool ApplicationWithGivenNameAndOrganisationExists(string name, Guid organisationId);
        List<invln_scheme> GetRecordsFromAhpApplicationsForAhpProject(Guid ahpProjectGuid, invln_Permission contactWebRole, Contact contact, Guid organisationGuid, bool isLeadPartner, bool IsSitePartner, bool isAllocation, bool useV2Version, string consortiumId = null);
        List<invln_scheme> GetByConsortiumId(Guid consortiumId);
        List<invln_scheme> GetListOfApplicationToSendReminder(string calculatedDate);
        invln_scheme GetAllocation(Guid allocationId, Guid organisationId, Contact contact = null);
        EntityCollection GetAllocationWithDeliveryPhaseAndClaims(string externalContactId, Guid accountId, Guid allocationId, Guid deliveryPhaseId);
    }
}
