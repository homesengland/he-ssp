using HE.Base.Services;
using HE.CRM.Common.Repositories.Implementations;
using HE.CRM.Common.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms.VisualStyles;

namespace HE.CRM.Plugins.Services.RichTextService
{
    public class RichTextService : CrmService, IRichTextService
    {
        private readonly IGlobalRepository globalRepository;
        public RichTextService(CrmServiceArgs args) : base(args)
        {
            globalRepository = CrmRepositoriesFactory.Get<IGlobalRepository>();
        }

        public string GenerateRichTextDocument(string entityId, string entityName, string richText)
        {
            TracingService.Trace("method");
            if (Guid.TryParse(entityId, out Guid id))
            {
                TracingService.Trace("if");
                List<string>fields = new List<string>();
                foreach (Match match in Regex.Matches(richText, "{[^}]+}"))
                {
                    fields.Append(match.Value.Trim('{', '}'));
                    TracingService.Trace(match.Value.Trim('{', '}'));
                    Console.WriteLine(match.Value.Trim('{', '}'));
                }
                if (fields.Count > 0)
                {
                    TracingService.Trace("length");
                    var retrievedEntity = globalRepository.RetrieveEntityOfGivenTypeWithGivenFields(entityName, id, fields.ToArray());
                    List<string> fieldsValues = new List<string>();
                    foreach (string fieldName in fields)
                    {
                        TracingService.Trace($"Fieldname: {fieldName}");
                        fieldsValues.Append(retrievedEntity.Attributes[fieldName]);
                        richText.Replace($"{{{fieldName}}}", retrievedEntity.Attributes[fieldName].ToString());
                    }
                }
            }
            return richText;
        }
    }
}
