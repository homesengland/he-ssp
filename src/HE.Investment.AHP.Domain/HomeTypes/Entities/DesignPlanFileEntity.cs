using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Services;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class DesignPlanFileEntity
{
    private const int MaxFileSizeInMegabytes = 20;

    private static readonly IList<FileExtension> AllowedExtensions =
        new[] { new FileExtension("jpg"), new FileExtension("png"), new FileExtension("pdf"), new FileExtension("docx") };

    private readonly Stream _content;

    private DesignPlanFileEntity(FileName name, FileSize size, Stream content)
    {
        if (!AllowedExtensions.Contains(name.Extension))
        {
            OperationResult.New()
                .AddValidationError("File", GenericValidationError.InvalidFileType(name.Value, AllowedExtensions.Select(x => x.Value)))
                .CheckErrors();
        }

        if (size > FileSize.FromMegabytes(MaxFileSizeInMegabytes))
        {
            OperationResult.New()
                .AddValidationError("File", GenericValidationError.FileTooBig(MaxFileSizeInMegabytes))
                .CheckErrors();
        }

        Name = name;
        _content = content;
    }

    public FileId? Id { get; private set; }

    public FileName Name { get; }

    public static DesignPlanFileEntity ForUpload(FileName name, FileSize size, Stream content) => new(name, size, content);

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
