using System.Collections.Generic;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface ISiteRepository : ICrmEntityRepository<invln_Sites, DataverseContext>
    {
        List<invln_Sites> GetAll(string fieldsToRetrieve);

        invln_Sites GetById(string id, string fieldsToRetrieve);

        bool Exist(string name);
    }
}
