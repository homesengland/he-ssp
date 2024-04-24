using System;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IConsortiumRepository : ICrmEntityRepository<invln_Consortium, DataverseContext>
    {
        bool GetByProgrammeAndOrganization(Guid programmeId, Guid organizationId);
    }
}
