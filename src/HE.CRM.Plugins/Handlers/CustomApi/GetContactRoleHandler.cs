using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Contacts;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class GetContactRoleHandler : CrmActionHandlerBase<invln_getcontactroleRequest, DataverseContext>
    {
        #region Fields

        private string email => ExecutionData.GetInputParameter<string>(invln_getcontactroleRequest.Fields.invln_email);
        private string portalId => ExecutionData.GetInputParameter<string>(invln_getcontactroleRequest.Fields.invln_portalid);
        private string ssid => ExecutionData.GetInputParameter<string>(invln_getcontactroleRequest.Fields.invln_ssid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(portalId) && !string.IsNullOrEmpty(ssid);
        }

        public override void DoWork()
        {
            var roleName = CrmServicesFactory.Get<IContactService>().GetContactRole(email, ssid, portalId);
            ExecutionData.SetOutputParameter(invln_getcontactroleResponse.Fields.invln_rolename, roleName);
        }

        #endregion
    }
}
