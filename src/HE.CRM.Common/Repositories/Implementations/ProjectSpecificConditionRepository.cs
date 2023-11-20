using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ProjectSpecificConditionRepository : CrmEntityRepository<invln_ProjectSpecificCondition, DataverseContext>, IProjectSpecificConditionRepository
    {
        public ProjectSpecificConditionRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
