using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Contacts;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    public class SendRequestToAssignContactToExistingOrganisationHandler : CrmActionHandlerBase<invln_sendrequesttoassigncontacttoexistingorganisationRequest, DataverseContext>
    {
        #region Fields

        private string organisationId => ExecutionData.GetInputParameter<string>(invln_sendrequesttoassigncontacttoexistingorganisationRequest.Fields.invln_organisationid);
        private string contactId => ExecutionData.GetInputParameter<string>(invln_sendrequesttoassigncontacttoexistingorganisationRequest.Fields.invln_contactid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return organisationId != null && contactId != null;
        }

        public override void DoWork()
        {
            TracingService.Trace("SendReminderEmailForRefferedBackToApplicantHandler");
            if (Guid.TryParse(organisationId, out var applicationGuid) && Guid.TryParse(contactId, out var contactGuid))
            {
                CrmServicesFactory.Get<IContactService>().SendRequestToAssignContactToExistingOrganisation(applicationGuid, contactGuid);
            }
        }

        #endregion
    }
}
