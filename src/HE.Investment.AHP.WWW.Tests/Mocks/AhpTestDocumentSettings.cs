using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Documents.Config;

namespace HE.Investment.AHP.WWW.Tests.Mocks;

public class AhpTestDocumentSettings : IAhpDocumentSettings
{
    private readonly IList<FileExtension> _fileExtensions;

    public AhpTestDocumentSettings(int maxFileSize, string extensions)
    {
        MaxFileSize = FileSize.FromMegabytes(maxFileSize);
        _fileExtensions = extensions.Split(";").Select(x => new FileExtension(x)).ToList();
    }

    public string ListAlias => string.Empty;

    public string ListTitle => string.Empty;

    public FileSize MaxFileSize { get; }

    public IEnumerable<FileExtension> AllowedExtensions => _fileExtensions;
}
