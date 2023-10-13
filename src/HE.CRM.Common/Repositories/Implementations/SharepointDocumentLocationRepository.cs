using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class SharepointDocumentLocationRepository : CrmEntityRepository<SharePointDocumentLocation, DataverseContext>, ISharepointDocumentLocationRepository
    {
        public SharepointDocumentLocationRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
