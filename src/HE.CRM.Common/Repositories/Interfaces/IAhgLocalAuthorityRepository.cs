using System.Collections.Generic;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IAhgLocalAuthorityRepository : ICrmEntityRepository<invln_AHGLocalAuthorities, DataverseContext>
    {
        List<invln_AHGLocalAuthorities> GetAll();
        invln_AHGLocalAuthorities GetLocalAuthorityWithGivenCode(string code);
    }
}
