using HE.Investments.DocumentService.Models;

namespace HE.Investments.Loans.BusinessLogic.Files;

public interface ILoansFileLocationProvider<in TFileParams>
{
    Task<FileLocation> GetFilesLocationAsync(TFileParams fileParams, CancellationToken cancellationToken);
}
