using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Contacts;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class ReturnUserProfileHandler : CrmActionHandlerBase<invln_returnuserprofileRequest, DataverseContext>
    {
        #region Fields

        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_returnuserprofileRequest.Fields.invln_contactexternalid);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(externalContactId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("ReturnUserProfile");
            var userProfile = CrmServicesFactory.Get<IContactService>().GetUserProfile(externalContactId);
            this.TracingService.Trace("End custom api");
            if (userProfile != null)
            {
                var serializedData = JsonSerializer.Serialize(userProfile);
                ExecutionData.SetOutputParameter(invln_returnuserprofileResponse.Fields.invln_userprofile, serializedData);
            }
        }

        #endregion
    }
}
