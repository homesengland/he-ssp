using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Repositories;
using HE.Base.Services;
using HE.CRM.Plugins.Services.Contract;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Plugins.Services.Teams
{
    public class TeamService : CrmService, ITeamService
    {
        #region Fields

        #endregion

        #region Constructors

        public TeamService(CrmServiceArgs args) : base(args)
        {
        }
        #endregion

        #region Public Methods

        #endregion
    }
}
