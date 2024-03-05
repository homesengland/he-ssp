using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using System;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IFrontDoorProjectRepository : ICrmEntityRepository<invln_FrontDoorProjectPOC, DataverseContext>
    {
        List<invln_FrontDoorProjectPOC> GetFrontDoorProjectForOrganisationAndContact(Guid organisationId, string externalContactId, string frontDoorProjectId = null, string fieldsToRetrieve = null);
        List<invln_FrontDoorProjectPOC> GetAccountFrontDoorProjects(Guid accountId);


    }
}
