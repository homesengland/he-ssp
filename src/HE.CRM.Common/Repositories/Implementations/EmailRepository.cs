using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class EmailRepository : CrmEntityRepository<invln_email, DataverseContext>, IEmailRepository
    {
        public EmailRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
