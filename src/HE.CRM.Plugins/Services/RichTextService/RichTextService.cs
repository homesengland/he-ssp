using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Repositories.Implementations;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Plugins.Services.LoanApplication;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace HE.CRM.Plugins.Services.RichTextService
{
    public class RichTextService : CrmService, IRichTextService
    {
        public RichTextService(CrmServiceArgs args) : base(args)
        {
        }

        public string GenerateRichTextDocument(string entityId, string entityName, string richText)
        {
            richText.Split('{', '}');
        }
    }
}
