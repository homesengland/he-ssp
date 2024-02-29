using DataverseModel;
using HE.Base.Repositories;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface ISiteRepository : ICrmEntityRepository<invln_Sites, DataverseContext>
    {
        PagedResponseDto<invln_Sites> Get(PagingRequestDto paging, string fieldsToRetrieve);

        invln_Sites GetById(string id, string fieldsToRetrieve);

        bool Exist(string name);
    }
}
