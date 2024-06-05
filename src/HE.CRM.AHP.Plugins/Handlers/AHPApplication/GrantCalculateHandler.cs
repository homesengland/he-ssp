using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Plugins.Attributes;
using HE.Base.Plugins.Common.Constants;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

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
            var disabledStatusesForCalculation = new List<invln_scheme_StatusCode>()
            {
                invln_scheme_StatusCode.Draft,
                invln_scheme_StatusCode.Deleted,
                invln_scheme_StatusCode.ReferredBackToApplicant,
                invln_scheme_StatusCode.Inactive
            };
            var currentStatuCode = (invln_scheme_StatusCode)CurrentState.StatusCode.Value;

            var isSubmitting = (currentStatuCode == invln_scheme_StatusCode.ApplicationSubmitted || currentStatuCode == invln_scheme_StatusCode.UnderReviewInAssessment)
                && ValueChanged(invln_scheme.Fields.StatusCode);

            return isSubmitting ||
                (
                    !disabledStatusesForCalculation.Contains(currentStatuCode) &&
                        (
                            ValueChanged(invln_scheme.Fields.invln_fundingrequired) ||
                            ValueChanged(invln_scheme.Fields.invln_noofhomes) ||
                            ValueChanged(invln_scheme.Fields.invln_expectedacquisitioncost) ||
                            ValueChanged(invln_scheme.Fields.invln_actualacquisitioncost) ||
                            ValueChanged(invln_scheme.Fields.invln_expectedoncosts) ||
                            ValueChanged(invln_scheme.Fields.invln_expectedonworks) ||
                            ValueChanged(invln_scheme.Fields.invln_Site) ||
                            ValueChanged(invln_scheme.Fields.invln_Tenure)
                        )
                );
        }

        public override void DoWork()
        {
            Logger.Trace($"{DateTime.Now} - Start executing {GetType().Name}. UserId: {ExecutionData.Context.UserId}");
            try
            {
                CrmServicesFactory.Get<IApplicationService>().GrantCalculate(CurrentState);
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred in plugin handler '{nameof(GrantCalculateHandler)}'");
                Logger.Error($"PrimaryEntityName: {ExecutionData.Context.PrimaryEntityName}, PrimaryEntityId: {ExecutionData.Context.PrimaryEntityId}");
                Logger.Error(ex.Message);
            }
        }
    }
}
