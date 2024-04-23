using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.AHP.Plugins.Services.Consortium
{
    public class ConsortiumService : CrmService, IConsortiumService
    {
        public ConsortiumService(CrmServiceArgs args) : base(args)
        {

        }

    }
}
