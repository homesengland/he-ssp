using HE.Investment.AHP.Domain.Common.Entities;
using HE.Investment.AHP.Domain.Common.FilePolicies;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Documents.Config;

namespace HE.Investment.AHP.Domain.Scheme.Entities;

public class LocalAuthoritySupportFile : FileEntity
{
    public LocalAuthoritySupportFile(FileName name, FileSize size, Stream content, IAhpDocumentSettings documentSettings)
        : base(name, size, content, new ValidFileExtensionPolicy(nameof(LocalAuthoritySupportFile)), new FileSizePolicy(nameof(LocalAuthoritySupportFile), documentSettings.MaxFileSize))
    {
    }
}
