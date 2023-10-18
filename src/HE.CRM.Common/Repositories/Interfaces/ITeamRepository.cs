using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface ITeamRepository : ICrmEntityRepository<Team, DataverseContext>
    {
        Team GetTeamByName(string teamName);
    }
}
