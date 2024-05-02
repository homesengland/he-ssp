using HE.Investment.AHP.Domain.Common.FilePolicies;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Common.Entities;

public class Files<TFileEntity>
{
    private readonly IFilePolicy<int>? _filesCountPolicy;

    private readonly List<UploadedFile> _filesToRemove = [];

    private readonly List<TFileEntity> _filesToUpload = [];

    public Files(IEnumerable<UploadedFile>? uploadedFiles = null, IFilePolicy<int>? filesCountPolicy = null)
    {
        _filesCountPolicy = filesCountPolicy;
        UploadedFiles = uploadedFiles?.ToList() ?? new List<UploadedFile>();
    }

    public IList<UploadedFile> UploadedFiles { get; }

    public bool IsModified => _filesToUpload.IsNotEmpty() || _filesToRemove.IsNotEmpty();

    public void AddFilesToUpload(IList<TFileEntity> files)
    {
        _filesCountPolicy?.Apply(UploadedFiles.Count + _filesToUpload.Count + files.Count - _filesToRemove.Count);

        foreach (var file in files)
        {
            _filesToUpload.Add(file);
        }
    }

    public void AddFileToUpload(TFileEntity file)
    {
        AddFilesToUpload(new List<TFileEntity> { file });
    }

    public void MarkFileToRemove(FileId fileId)
    {
        var fileToRemove = UploadedFiles.FirstOrDefault(x => x.Id == fileId) ?? throw new NotFoundException(nameof(FileEntity), fileId);

        _filesToRemove.Add(fileToRemove);
    }

    public async Task SaveChanges(Func<TFileEntity, Task<UploadedFile>> saveFile, Func<FileId, Task> removeFile)
    {
        for (var i = _filesToUpload.Count - 1; i >= 0; i--)
        {
            var uploadedFile = await saveFile(_filesToUpload[i]);
            UploadedFiles.Add(uploadedFile);
            _filesToUpload.RemoveAt(i);
        }

        for (var i = _filesToRemove.Count - 1; i >= 0; i--)
        {
            await removeFile(_filesToRemove[i].Id);
            UploadedFiles.Remove(_filesToRemove[i]);
            _filesToRemove.RemoveAt(i);
        }
    }
}
