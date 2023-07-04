using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Services;
using HE.CRM.Plugins.Services.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class GenerateRichTextDocumentHandler : CrmActionHandlerBase<invln_generaterichtextdocumentRequest, DataverseContext>
    {
        #region Fields

        private string entityId => ExecutionData.GetInputParameter<string>(invln_generaterichtextdocumentRequest.Fields.invln_entityid);
        private string entityName => ExecutionData.GetInputParameter<string>(invln_generaterichtextdocumentRequest.Fields.invln_entityname);
        private string richText => ExecutionData.GetInputParameter<string>(invln_generaterichtextdocumentRequest.Fields.invln_richtext);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(entityId) && !string.IsNullOrEmpty(entityName) && !string.IsNullOrEmpty(richText);
        }

        public override void DoWork()
        {
            var roleName = CrmServicesFactory.Get<IContactService>().GetContactRole(email, ssid, portalId);
            ExecutionData.SetOutputParameter(invln_getcontactroleResponse.Fields.invln_rolename, roleName);
        }

        #endregion
    }
}
