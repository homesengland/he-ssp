using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Entities;
using HE.Investment.AHP.Domain.Common.FilePolicies;
using HE.Investment.AHP.Domain.Documents.Services;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Scheme.Entities;

public class LocalAuthoritySupportFileContainer
{
    private readonly Files<LocalAuthoritySupportFile> _files;

    public LocalAuthoritySupportFileContainer(UploadedFile? uploadedFile)
    {
        var list = uploadedFile != null ? new List<UploadedFile> { uploadedFile } : new List<UploadedFile>();
        _files = new Files<LocalAuthoritySupportFile>(list, new SingleFileCountPolicy(nameof(LocalAuthoritySupportFile)));
    }

    public UploadedFile? File => _files.UploadedFiles.FirstOrDefault();

    public bool IsModified => _files.IsModified;

    public void Add(LocalAuthoritySupportFile newFile)
    {
        _files.AddFileToUpload(newFile);
    }

    public void MarkFileToRemove(FileId fileId)
    {
        _files.MarkFileToRemove(fileId);
    }

    public async Task SaveChanges(
        AhpApplicationId applicationId,
        IAhpFileService<LocalAuthoritySupportFileParams> fileService,
        CancellationToken cancellationToken)
    {
        await _files.SaveChanges(
            async file => await fileService.UploadFile(file.Name, file.Content, new LocalAuthoritySupportFileParams(applicationId), cancellationToken),
            async fileId => await fileService.RemoveFile(fileId, new LocalAuthoritySupportFileParams(applicationId), cancellationToken));
    }
}
