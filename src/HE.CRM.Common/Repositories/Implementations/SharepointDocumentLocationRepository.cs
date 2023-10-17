using System;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Client;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class SharepointDocumentLocationRepository : CrmEntityRepository<SharePointDocumentLocation, DataverseContext>, ISharepointDocumentLocationRepository
    {
        public SharepointDocumentLocationRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public SharePointDocumentLocation GetDocumentLocationRelatedToLoanApplication(Guid loanApplicationId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<SharePointDocumentLocation>()
                    .Where(x => x.RegardingObjectId.Id == loanApplicationId).FirstOrDefault();
            }
        }
    }
}
