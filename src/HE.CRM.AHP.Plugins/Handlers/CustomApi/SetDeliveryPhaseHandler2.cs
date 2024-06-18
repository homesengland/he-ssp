using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.DeliveryPhase;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class SetDeliveryPhaseHandler2 : CrmActionHandlerBase<invln_setdeliveryphaseRequest, DataverseContext>
    {
        #region Fields

        private string DeliveryPhaseData => ExecutionData.GetInputParameter<string>(invln_setdeliveryphaseRequest.Fields.invln_deliveryPhase);
        private string UserId => ExecutionData.GetInputParameter<string>(invln_setdeliveryphaseRequest.Fields.invln_userId);
        private string OrganisationId => ExecutionData.GetInputParameter<string>(invln_setdeliveryphaseRequest.Fields.invln_organisationId);
        private string ApplicationId => ExecutionData.GetInputParameter<string>(invln_setdeliveryphaseRequest.Fields.invln_applicationId);
        private string FieldsToSet => ExecutionData.GetInputParameter<string>(invln_setdeliveryphaseRequest.Fields.invln_fieldstoset);

        #endregion Fields

        #region Base Methods Overrides

        public override bool CanWork()
        {
            return DeliveryPhaseData != null && UserId != null && OrganisationId != null && ApplicationId != null;
        }

        public override void DoWork()
        {
            Logger.Trace($"{DateTime.Now} - Start executing {GetType().Name}. UserId: {ExecutionData.Context.UserId}");

            Logger.Trace($"{invln_setdeliveryphaseRequest.Fields.invln_deliveryPhase}: {DeliveryPhaseData}");
            Logger.Trace($"{invln_setdeliveryphaseRequest.Fields.invln_userId}: {UserId}");
            Logger.Trace($"{invln_setdeliveryphaseRequest.Fields.invln_organisationId}: {OrganisationId}");
            Logger.Trace($"{invln_setdeliveryphaseRequest.Fields.invln_applicationId}: {ApplicationId}");
            Logger.Trace($"{invln_setdeliveryphaseRequest.Fields.invln_fieldstoset}: {FieldsToSet}");


            var createdRecordGuid = CrmServicesFactory.Get<IDeliveryPhaseService2>().SetDeliveryPhase(DeliveryPhaseData, UserId, OrganisationId, ApplicationId, FieldsToSet);

            ExecutionData.SetOutputParameter(invln_setdeliveryphaseResponse.Fields.invln_deliveryphaseid, createdRecordGuid.ToString());
        }

        #endregion Base Methods Overrides
    }
}
