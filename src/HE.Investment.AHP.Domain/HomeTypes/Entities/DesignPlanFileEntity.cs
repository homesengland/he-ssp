using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Services;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class DesignPlanFileEntity
{
    private const int MaxFileSizeInMegabytes = 20;

    private static readonly IList<FileExtension> AllowedExtensions =
        new[] { new FileExtension("jpg"), new FileExtension("png"), new FileExtension("pdf"), new FileExtension("docx") };

    private readonly FileName _name;

    private readonly Stream _content;

    private FileId? _id;

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

        _name = name;
        _content = content;
    }

    public static DesignPlanFileEntity ForUpload(FileName name, FileSize size, Stream content) => new(name, size, content);

    public async Task<UploadedFile> Upload(
        IHomeTypeEntity homeType,
        IDesignFileService designFileService,
        CancellationToken cancellationToken)
    {
        if (_id.IsProvided())
        {
            throw new InvalidOperationException($"Design File {_name} is already uploaded with Id {_id}.");
        }

        if (homeType.Id.IsNotProvided())
        {
            throw new InvalidOperationException($"Design File {_name} cannot be uploaded because home type is not saved yet.");
        }

        var uploadedFile = await designFileService.UploadFile(homeType.Application.Id, homeType.Id!, _name, _content, cancellationToken);
        _id = uploadedFile.Id;

        return uploadedFile;
    }
}
