using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;

namespace HE.CRM.AHP.Plugins.Services.Claim
{
    public interface IClaimService : ICrmService
    {
        void ChangeExternalStatus(invln_Claim target);
    }
}
