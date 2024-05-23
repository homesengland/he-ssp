using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Plugins.Attributes;
using HE.Base.Plugins.Common.Constants;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Handlers.AHPApplication
{
    [CrmMessage(CrmMessage.Update)]
    [CrmProcessingStage(CrmProcessingStepStages.Postoperation)]

    public class GrantCalculateHandler : CrmEntityHandlerBase<invln_scheme, DataverseContext>
    {
        private invln_scheme Target => ExecutionData.Target;

        public GrantCalculateHandler()
        {
        }

        public override bool CanWork()
        {
            var disabledStatusesForCalculation = new List<int>()
            {
                (int)invln_scheme_StatusCode.Draft,
                (int)invln_scheme_StatusCode.Deleted,
                (int)invln_scheme_StatusCode.Inactive
            };
            var currentStatuCode = CurrentState.StatusCode.Value;

            var isSubmitting = (currentStatuCode == (int)invln_scheme_StatusCode.ApplicationSubmitted) && ValueChanged(invln_scheme.Fields.StatusCode);
            return isSubmitting ||
                (
                    !disabledStatusesForCalculation.Contains(currentStatuCode) &&
                        (
                            ValueChanged(invln_scheme.Fields.invln_fundingrequired) ||
                            ValueChanged(invln_scheme.Fields.invln_noofhomes) ||
                            ValueChanged(invln_scheme.Fields.invln_expectedacquisitioncost) ||
                            ValueChanged(invln_scheme.Fields.invln_actualacquisitioncost) ||
                            ValueChanged(invln_scheme.Fields.invln_oncosts) ||
                            ValueChanged(invln_scheme.Fields.invln_workscosts) ||
                            ValueChanged(invln_scheme.Fields.invln_Site) ||
                            ValueChanged(invln_scheme.Fields.invln_Tenure)
                        )
                );
        }

        public override void DoWork()
        {
            TracingService.Trace($"{DateTime.Now} - Start executing {GetType().Name}. UserId: {ExecutionData.Context.UserId}");
            try
            {
                CrmServicesFactory.Get<IApplicationService>().GrantCalculate(Target.Id);
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex.Message}");
            }
        }
    }
}
