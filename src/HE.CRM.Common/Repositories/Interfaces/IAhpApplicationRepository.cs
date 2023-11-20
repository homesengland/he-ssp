using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using System;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IAhpApplicationRepository : ICrmEntityRepository<invln_scheme, DataverseContext>
    {
        List<invln_scheme> GetApplicationsForOrganisationAndContact(string organisationId, string contactId, string attributes, string additionalRecordFilters);
        bool ApplicationWithGivenNameExists(string name);
        bool ApplicationWithGivenIdExistsForOrganisationAndContract(Guid applicationId, Guid organisationId, string userId);
    }
}
