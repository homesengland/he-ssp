using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class DesignPlanFileEntity
{
    private readonly Stream _content;

    private DesignPlanFileEntity(FileName name, FileSize size, Stream content, IAhpDocumentSettings documentSettings)
    {
        if (!documentSettings.AllowedExtensions.Contains(name.Extension))
        {
            OperationResult.New()
                .AddValidationError("File", GenericValidationError.InvalidFileType(name.Value, documentSettings.AllowedExtensions.Select(x => x.Value)))
                .CheckErrors();
        }

        if (size > documentSettings.MaxFileSize)
        {
            OperationResult.New()
                .AddValidationError("File", GenericValidationError.FileTooBig(documentSettings.MaxFileSize.Megabytes))
                .CheckErrors();
        }

        Name = name;
        _content = content;
    }

    public FileId? Id { get; private set; }

    public FileName Name { get; }

    public static DesignPlanFileEntity ForUpload(FileName name, FileSize size, Stream content, IAhpDocumentSettings documentSettings) =>
        new(name, size, content, documentSettings);

    public async Task<UploadedFile> Upload(
        IHomeTypeEntity homeType,
        IAhpFileService<DesignFileParams> designFileService,
        CancellationToken cancellationToken)
    {
        if (Id.IsProvided())
        {
            throw new InvalidOperationException($"Design File {Name} is already uploaded with Id {Id}.");
        }

        if (homeType.Id.IsNew)
        {
            throw new InvalidOperationException($"Design File {Name} cannot be uploaded because home type is not saved yet.");
        }

        var uploadedFile = await designFileService.UploadFile(Name, _content, new DesignFileParams(homeType.Application.Id, homeType.Id), cancellationToken);
        Id = uploadedFile.Id;

        return uploadedFile;
    }
}
