﻿using HE.Base.Services;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Services.RichTextService
{
    public interface IRichTextService : ICrmService
    {
        string GenerateRichTextDocument(string entityId, string entityName, string richText);
    }
}
