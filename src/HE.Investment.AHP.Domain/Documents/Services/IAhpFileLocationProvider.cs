using HE.Investments.DocumentService.Models;

namespace HE.Investment.AHP.Domain.Documents.Services;

public interface IAhpFileLocationProvider<in TFileParams>
{
    Task<FileLocation> GetFileLocation(TFileParams fileParams, CancellationToken cancellationToken);
}
