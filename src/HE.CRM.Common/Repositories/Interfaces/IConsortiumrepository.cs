using System;
using System.Collections.Generic;
using DataverseModel;
using HE.Base.Repositories;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IConsortiumRepository : ICrmEntityRepository<invln_Consortium, DataverseContext>
    {
        List<invln_Consortium> GetByLeadPartnerInConsoriumMember(string organisationId);
        bool GetByProgrammeAndOrganization(Guid programmeId, Guid organizationId);
        EntityCollection GetConsortiumIdForAhpApplication(Guid ahpApplicationId);
        EntityCollection GetConsortiumIdForAhpSite(Guid siteId);
    }
}
