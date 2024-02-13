using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IProgrammeRepository : ICrmEntityRepository<invln_programme, DataverseContext>
    {
        invln_programme GetProgrammeById(string programmeId);
        List<invln_programme> GetProgrammes();
    }
}
