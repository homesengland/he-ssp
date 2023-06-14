using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;

public class SiteDetailsRepository : CrmEntityRepository<invln_SiteDetails, DataverseContext>, ISiteDetailsRepository
{
    

    public SiteDetailsRepository(CrmRepositoryArgs args) : base(args)
    {
    }
}

