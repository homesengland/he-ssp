using HE.Investment.AHP.Domain.Common.FilePolicies;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Common.Entities;

public abstract class FileEntity
{
    protected FileEntity(FileName name, FileSize size, Stream content, IFilePolicy<FileName>? fileNamePolicy = null, IFilePolicy<FileSize>? fileSizePolicy = null)
    {
        fileNamePolicy?.Apply(name);
        fileSizePolicy?.Apply(size);

        Name = name;
        Content = content;
    }

    protected FileName Name { get; }

    protected Stream Content { get; }

    protected FileId? Id { get; set; }
}
