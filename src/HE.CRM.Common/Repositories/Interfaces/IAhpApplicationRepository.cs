using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using System;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IAhpApplicationRepository : ICrmEntityRepository<invln_scheme, DataverseContext>
    {
        List<invln_scheme> GetApplicationsForOrganisationAndContact(string organisationId, string contactFilter, string attributes, string additionalRecordFilters);
        bool ApplicationWithGivenNameExists(string name);
        bool ApplicationWithGivenIdExistsForOrganisation(Guid applicationId, Guid organisationId);
        bool ApplicationWithGivenNameAndOrganisationExists(string name, Guid organisationId);
        List<invln_scheme> GetApplicationsForAhpProject(Guid ahpProjectGuid, invln_Permission contactWebRole, Contact contact, Guid organisationGuid, bool isLeadPartner, string consortiumId = null);
        List<invln_scheme> GetByConsortiumId(Guid consortiumId);
    }
}
