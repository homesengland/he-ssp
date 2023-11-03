using System;
using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IContactWebroleRepository : ICrmEntityRepository<invln_contactwebrole, DataverseContext>
    {
        List<invln_contactwebrole> GetAdminContactWebrolesForOrganisation(Guid organisationId, Guid adminWebrole);
    }
}
