using HE.Investment.AHP.Domain.Common.ValueObjects;

namespace HE.Investment.AHP.Domain.Documents.Config;

public interface IAhpDocumentSettings
{
    string ListAlias { get; }

    string ListTitle { get; }

    FileSize MaxFileSize { get; }

    IEnumerable<FileExtension> AllowedExtensions { get; }
}
