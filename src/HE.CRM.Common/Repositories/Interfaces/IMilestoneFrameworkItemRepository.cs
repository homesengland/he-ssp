using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IMilestoneFrameworkItemRepository : ICrmEntityRepository<invln_milestoneframeworkitem, DataverseContext>
    {
        List<invln_milestoneframeworkitem> GetMilestoneFrameworkItemByProgrammeId(string programmeId);
    }
}




