using HE.Investment.AHP.Domain.Common.ValueObjects;
using Microsoft.Extensions.Configuration;

namespace HE.Investment.AHP.Domain.Documents.Config;

public class AhpDocumentSettings : IAhpDocumentSettings
{
    private const string BasePath = "AppConfiguration:Documents";
    private const string DefaultTitle = "AHP Application";
    private const string DefaultAlias = "invln_scheme";
    private const string DefaultExtensions = "jpg;png;pdf;docx";
    private const int DefaultMaxFileSize = 50;

    private readonly IList<FileExtension> _allowedExtensions;

    public AhpDocumentSettings(IConfiguration configuration)
    {
        ListAlias = configuration.GetValue<string>($"{BasePath}:ListAlias") ?? DefaultAlias;
        ListTitle = configuration.GetValue<string>($"{BasePath}:ListTitle") ?? DefaultTitle;
        MaxFileSize = FileSize.FromMegabytes(configuration.GetValue<int?>($"{BasePath}:MaxFileSizeInMegabytes") ?? DefaultMaxFileSize);
        _allowedExtensions = (configuration.GetValue<string>($"{BasePath}:AllowedExtensions") ?? DefaultExtensions).Split(";")
            .Select(x => new FileExtension(x))
            .ToList();
    }

    public string ListAlias { get; }

    public string ListTitle { get; }

    public FileSize MaxFileSize { get; }

    public IEnumerable<FileExtension> AllowedExtensions => _allowedExtensions;
}
