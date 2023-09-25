using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class GovNotifyEmailRepository : CrmEntityRepository<invln_govnotifyemail, DataverseContext>, IGovNotifyEmailRepository
    {
        public GovNotifyEmailRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
