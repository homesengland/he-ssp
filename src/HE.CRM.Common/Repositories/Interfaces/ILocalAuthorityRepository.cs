using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface ILocalAuthorityRepository : ICrmEntityRepository<invln_localauthority, DataverseContext>
    {
        List<invln_localauthority> GetAll();
        invln_localauthority GetLocalAuthorityWithGivenOnsCode(string onsCode);
    }
}
