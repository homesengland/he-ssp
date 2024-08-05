using System;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface ISharepointDocumentLocationRepository : ICrmEntityRepository<SharePointDocumentLocation, DataverseContext>
    {
        SharePointDocumentLocation GetDocumentLocationRelatedToRecordWithGivenGuid(Guid recordId);

        SharePointDocumentLocation GetHomeTypeDocumentLocationForGivenApplicationLocationRecord(Guid documentLocation);
    }
}
