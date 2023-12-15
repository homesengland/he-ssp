using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Contacts;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi
{
    internal class InviteContactToJoinExistingOrganisationHandler : CrmActionHandlerBase<invln_invitecontacttojoinexistingorganisationRequest, DataverseContext>
    {
        #region Fields

        private string invitedContactId => ExecutionData.GetInputParameter<string>(invln_invitecontacttojoinexistingorganisationRequest.Fields.invln_invitedcontactid);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_invitecontacttojoinexistingorganisationRequest.Fields.invln_organisationid);
        private string inviterContactId => ExecutionData.GetInputParameter<string>(invln_invitecontacttojoinexistingorganisationRequest.Fields.invln_invitercontactid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return invitedContactId != null && organisationId != null && inviterContactId != null;
            ;
        }

        public override void DoWork()
        {
            TracingService.Trace("InviteContactToJoinExistingOrganisationHandler");
            if (Guid.TryParse(invitedContactId, out var invitedContactGuid) && Guid.TryParse(organisationId, out var organisationGuid) && Guid.TryParse(inviterContactId, out var inviterContactGuid))
            {
                CrmServicesFactory.Get<IContactService>().InviteContactToJoinExistingOrganisation(inviterContactGuid, invitedContactGuid, organisationGuid);
            }
        }

        #endregion
    }
}
