using System;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Client;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class SharepointSiteRepository : CrmEntityRepository<SharePointSite, DataverseContext>, ISharepointSiteRepository
    {
        public SharepointSiteRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
