using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Services.Claim
{
    public class ClaimService : CrmService, IClaimService
    {
        public ClaimService(CrmServiceArgs args) : base(args)
        {
        }

        public void ChangeExternalStatus(invln_Claim target)
        {
            target.invln_ExternalStatus = MapClaimInternalToExternalStatus(target.StatusCode.Value);
        }


        private OptionSetValue MapClaimInternalToExternalStatus(int internalStatus)
        {
            switch (internalStatus)
            {
                case (int)invln_Claim_StatusCode.Draft:
                    return new OptionSetValue((int)invln_ClaimExternalStatus.Draft);
                case (int)invln_Claim_StatusCode.Submitted:
                    return new OptionSetValue((int)invln_ClaimExternalStatus.Submitted);
                case (int)invln_Claim_StatusCode.UnderReview:
                    return new OptionSetValue((int)invln_ClaimExternalStatus.UnderReview);
                case (int)invln_Claim_StatusCode.OnHold:
                    return new OptionSetValue((int)invln_ClaimExternalStatus.UnderReview);
                case (int)invln_Claim_StatusCode.Reject:
                    return new OptionSetValue((int)invln_ClaimExternalStatus.Rejected);
                case (int)invln_Claim_StatusCode.Approve:
                    return new OptionSetValue((int)invln_ClaimExternalStatus.Approved);
                default:
                    return null;
            }
        }
    }
}
