using System;
using System.Collections.Generic;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IConsortiumRepository : ICrmEntityRepository<invln_Consortium, DataverseContext>
    {
        List<invln_Consortium> GetByPartnerInConsoriumMember(string organisationId);
        bool GetByProgrammeAndOrganization(Guid programmeId, Guid organizationId);
    }
}
