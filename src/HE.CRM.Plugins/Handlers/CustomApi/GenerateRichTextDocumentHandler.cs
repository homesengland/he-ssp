using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Services;
using HE.CRM.Plugins.Services.Contacts;
using HE.CRM.Plugins.Services.RichTextService;
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
            var generatedDocument = CrmServicesFactory.Get<IRichTextService>().GenerateRichTextDocument(entityId, entityName, richText);
            ExecutionData.SetOutputParameter(invln_generaterichtextdocumentResponse.Fields.invln_generatedrichtext, generatedDocument);
        }

        #endregion
    }
}
