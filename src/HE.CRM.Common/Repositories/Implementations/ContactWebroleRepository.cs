using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ContactWebroleRepository : CrmEntityRepository<invln_contactwebrole, DataverseContext>, IContactWebroleRepository
    {
        public ContactWebroleRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}