using System.Linq;
using DataverseModel;
using HE.Base.Plugins.Attributes;
using HE.Base.Plugins.Common.Constants;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Handlers.AHPApplication
{
    /// <summary>
    /// Populate the following fields when application status is ApplicationSubmitted or UnderReviewInAssessment.
    /// - invln_RtSOExamption
    /// - invln_NumberofBedsits
    /// - invln_ContractExist
    /// - invln_ContractTypeConsortiumSingleEntity
    /// </summary>
    [CrmMessage(CrmMessage.Update)]
    [CrmProcessingStage(CrmProcessingStepStages.Preoperation)]
    public class PopulateFieldsForApplicationReportHandler : CrmEntityHandlerBase<invln_scheme, DataverseContext>
    {
        public override bool CanWork()
        {
            var currentStatuCode = (invln_scheme_StatusCode)CurrentState.StatusCode.Value;

            var isSubmitting = (currentStatuCode == invln_scheme_StatusCode.ApplicationSubmitted || currentStatuCode == invln_scheme_StatusCode.UnderReviewInAssessment)
                && ValueChanged(invln_scheme.Fields.StatusCode);

            return isSubmitting;
        }

        public override void DoWork()
        {
            var applicationReportCalculationService = CrmServicesFactory.Get<ICalculateDataForApplicationReportService>();
            var calculationReportData = applicationReportCalculationService.Calculate(CurrentState.Id);

            ExecutionData.Target.invln_RtSOExamption = calculationReportData.RtsoExamption;
            ExecutionData.Target.invln_NumberofBedsits = calculationReportData.NumberOfBedsits;
            ExecutionData.Target.invln_ContractExist = calculationReportData.IsContractExists;
            if (calculationReportData.ContractTypeConsortiumSingleEntity.HasValue)
            {
                ExecutionData.Target.invln_ContractTypeConsortiumSingleEntity = new OptionSetValue(calculationReportData.ContractTypeConsortiumSingleEntity.Value);
            }
            else
            {
                ExecutionData.Target.invln_ContractTypeConsortiumSingleEntity = null;
            }
        }
    }
}
