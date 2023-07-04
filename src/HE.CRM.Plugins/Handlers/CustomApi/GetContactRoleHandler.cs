using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Contacts;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class GetContactRoleHandler : CrmActionHandlerBase<invln_getcontactroleRequest, DataverseContext>
    {
        #region Fields

        private string contactEmail => ExecutionData.GetInputParameter<string>(invln_getcontactroleRequest.Fields.invln_contactemail);
        private string portalType => ExecutionData.GetInputParameter<string>(invln_getcontactroleRequest.Fields.invln_portaltype);
        private string contactExternalId => ExecutionData.GetInputParameter<string>(invln_getcontactroleRequest.Fields.invln_contactexternalid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(contactEmail) && !string.IsNullOrEmpty(portalType) && !string.IsNullOrEmpty(contactExternalId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetContactRoles");
            var roles = CrmServicesFactory.Get<IContactService>().GetContactRoles(contactEmail, contactExternalId, portalType);
            this.TracingService.Trace("Send Response");
            if (roles != null)
            {
                ExecutionData.SetOutputParameter(invln_getcontactroleResponse.Fields.invln_portalroles, JsonSerializer.Serialize(roles));
            }
        }

        #endregion
    }
}
