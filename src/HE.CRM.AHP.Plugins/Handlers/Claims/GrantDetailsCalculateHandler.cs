using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.Claims
{
    public sealed class GrantDetailsCalculateHandler : CrmEntityHandlerBase<invln_Claim, DataverseContext>
    {
        public override bool CanWork()
        {
            var currentStatuCode = (invln_Claim_StatusCode)CurrentState.StatusCode.Value;
            return this.ValueChanged(invln_Claim.Fields.StateCode) && currentStatuCode == invln_Claim_StatusCode.Approve;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IAllocationService>().CalculateGrantDetails(ExecutionData.Target.Id);
        }
    }
}
