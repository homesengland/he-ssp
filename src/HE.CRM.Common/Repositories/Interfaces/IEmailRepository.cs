using System;
using HE.Base.Repositories;
using DataverseModel;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IEmailRepository : ICrmEntityRepository<invln_email, DataverseContext>
    {
    }
}
