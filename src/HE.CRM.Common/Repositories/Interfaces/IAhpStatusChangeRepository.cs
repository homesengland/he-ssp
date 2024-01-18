using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IAhpStatusChangeRepository : ICrmEntityRepository<invln_AHPStatusChange, DataverseContext>
    {
    }
}
