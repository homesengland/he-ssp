using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class StandardConditionsRepository : CrmEntityRepository<invln_StandardCondition, DataverseContext>, IStandardConditionsRepository
    {
        public StandardConditionsRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
