using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.Contract.Application.Queries;
using HE.Investments.Loans.Contract.Common;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetSupportingDocumentsFileQueryHandler : IRequestHandler<GetSupportingDocumentsFileQuery, DownloadedFile>
{
    private readonly ILoansFileService<SupportingDocumentsParams> _fileService;

    public GetSupportingDocumentsFileQueryHandler(ILoansFileService<SupportingDocumentsParams> fileService)
    {
        _fileService = fileService;
    }

    public async Task<DownloadedFile> Handle(GetSupportingDocumentsFileQuery request, CancellationToken cancellationToken)
    {
        var result = await _fileService.DownloadFile(request.FileId, SupportingDocumentsParams.New(request.LoanApplicationId), cancellationToken);
        return new DownloadedFile(result.Name, result.Content);
    }
}
