using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Contacts;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    internal class UpdateUserProfileHandler : CrmActionHandlerBase<invln_updateuserprofileRequest, DataverseContext>
    {
        #region Fields

        private string contactExternalId => ExecutionData.GetInputParameter<string>(invln_updateuserprofileRequest.Fields.invln_contactexternalid);
        private string serializedContact => ExecutionData.GetInputParameter<string>(invln_updateuserprofileRequest.Fields.invln_contact);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(contactExternalId);
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IContactService>().UpdateUserProfile(contactExternalId, serializedContact);
        }

        #endregion
    }
}
