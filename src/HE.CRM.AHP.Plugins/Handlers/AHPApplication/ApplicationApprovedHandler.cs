using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.AHPApplication
{
    public class CreateAllocationWhenApprovedHandler : CrmEntityHandlerBase<invln_scheme, DataverseContext>
    {
        public override bool CanWork()
        {
            if (ExecutionData.PostImage.invln_isallocation == true)
            { // If not application
                return false;
            }

            // If application status changed to Approved
            return ValueChanged(invln_scheme.Fields.StatusCode, invln_scheme_StatusCode.Approved);
        }

        public override void DoWork()
        {
            var allocationId = CrmServicesFactory.Get<IAllocationService>().CreateAllocation(ExecutionData.Target.Id);
            Logger.Info($"Allocation was created (ID: {allocationId}) for application with ID: {ExecutionData.Target.Id}");
        }
    }
}
