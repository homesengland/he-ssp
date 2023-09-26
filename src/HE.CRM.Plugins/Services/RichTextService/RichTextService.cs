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
        #region Fields
        private readonly IGlobalRepository globalRepository;
        #endregion

        #region Constructors
        public RichTextService(CrmServiceArgs args) : base(args)
        {
            globalRepository = CrmRepositoriesFactory.Get<IGlobalRepository>();
        }
        #endregion

        #region Public Methods

        public string GenerateRichTextDocument(string entityId, string entityName, string richText)
        {
            TracingService.Trace("method");
            TracingService.Trace($"richtext: {richText}");
            if (Guid.TryParse(entityId, out Guid id))
            {
                TracingService.Trace("if");
                List<string>fields = new List<string>();
                foreach (Match match in Regex.Matches(richText, "{[^}]+}"))
                {
                    fields.Add(match.Value.Trim('{', '}'));
                    TracingService.Trace(match.Value.Trim('{', '}'));
                }
                if (fields.Count > 0)
                {
                    TracingService.Trace("length");
                    var retrievedEntity = globalRepository.RetrieveEntityOfGivenTypeWithGivenFields(entityName, id, fields.ToArray());
                    List<string> fieldsValues = new List<string>();
                    foreach (string fieldName in fields)
                    {
                        TracingService.Trace($"Fieldname: {fieldName}");
                        TracingService.Trace($"value: {retrievedEntity.Attributes[fieldName].ToString()}");
                        TracingService.Trace($"newname: {{{{{fieldName}}}}}");
                        fieldsValues.Add(retrievedEntity.Attributes[fieldName].ToString());
                        richText = richText.Replace($"{{{{{fieldName}}}}}", retrievedEntity.Attributes[fieldName].ToString());
                    }
                }
            }
            return richText;
        }
        #endregion
    }
}
