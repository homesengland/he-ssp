using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using System;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IFrontDoorProjectRepository : ICrmEntityRepository<invln_FrontDoorProjectPOC, DataverseContext>
    {
        List<invln_FrontDoorProjectPOC> GetFrontDoorProjectForOrganisationAndContact(string organisationCondition, string contactExternalIdFilter, string attributes, string frontDoorProjectFilters, string statecodeCondition);
        List<invln_FrontDoorProjectPOC> GetAccountFrontDoorProjects(Guid accountId);
        bool CheckIfFrontDoorProjectWithGivenNameExists(string frontDoorProjectName, Guid organisationId);
    }
}

