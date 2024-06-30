using HE.Investment.AHP.Domain.Common.FilePolicies;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Common.Entities;

public abstract class FileEntity
{
    protected FileEntity(FileName name, FileSize size, Stream content, IFilePolicy<FileName>? fileNamePolicy = null, IFilePolicy<FileSize>? fileSizePolicy = null)
    {
        var operationResult = OperationResult.New();
        fileNamePolicy?.Apply(name, operationResult);
        fileSizePolicy?.Apply(size, operationResult);
        operationResult.CheckErrors();

        Name = name;
        Content = content;
    }

    public FileName Name { get; }

    public Stream Content { get; }
}
