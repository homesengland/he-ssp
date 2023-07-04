using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Repositories.Implementations;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Plugins.Services.LoanApplication;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace HE.CRM.Plugins.Services.RichTextService
{
    public class RichTextService : CrmService, IRichTextService
    {
        public RichTextService(CrmServiceArgs args) : base(args)
        {
        }

        public string GenerateRichTextDocument(string entityId, string entityName, string richText)
        {
            foreach (Match match in Regex.Matches(richText, "{[^}]+}"))
            {
                Console.WriteLine(match.Value);
            }
            return String.Empty;
        }
    }
}
